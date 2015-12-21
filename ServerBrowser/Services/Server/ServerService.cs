using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ServerBrowser.Services.Server
{
	public class ServerService : IServerService
	{
		private const int ServerListTimeout = 5000;
		private const int Start = 11;

		private readonly byte[] _sendBuffer;
		private readonly char[] _separator1;
		private readonly char[] _separator2;
		private readonly Stopwatch _stopwatch;
		private readonly ServerInfoFactory _serverInfoFactory;

		public ServerService()
		{
			_sendBuffer = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0x73, 0x74, 0x61, 0x74, 0x75, 0x73, 0x0A, 0x00 };
			_separator1 = new char[] { '\n' };
			_separator2 = new char[] { ':' };
			_stopwatch = new Stopwatch();
			_serverInfoFactory = new ServerInfoFactory();
		}

		public string ServerList { get; private set; }
		public bool ScanLocalhost { get; private set; }
		public int ServerTimeout { get; private set; }

		public void Initialize(string serverList, bool scanLocalhost, int serverTimeout)
		{
			ServerList = serverList;
			ScanLocalhost = scanLocalhost;
			ServerTimeout = serverTimeout;
		}

		public async Task<List<IPEndPoint>> GetServers()
		{
			WebRequest request = WebRequest.Create(new Uri(ServerList));
			request.Timeout = ServerListTimeout;
			WebResponse response = await request.GetResponseAsync();
			Stream s = response.GetResponseStream();
			StreamReader sr = new StreamReader(s);
			string data = sr.ReadToEnd();
			var servers = new List<IPEndPoint>();
			if (data.StartsWith("<"))
			{
				if (ScanLocalhost)
				{
					servers.Add(new IPEndPoint(IPAddress.Loopback, 27910));
				}
				try
				{
					data = data.Substring(122);
					data = data.Replace("<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\" lang=\"en\">", "<html>");
					data = data.Replace("&nbsp;", " ");
					XmlDocument doc = new XmlDocument();
					doc.LoadXml(data);
					XmlNodeList nodeList = doc.DocumentElement.SelectNodes("//table[@class='servers']/tr");
					for (int i = 1; i < nodeList.Count; i++)
					{
						try
						{
							XmlNode node = nodeList[i];
							string uri = node.ChildNodes[2].InnerText.Trim();
							string[] split2 = uri.Split(_separator2);
							servers.Add(new IPEndPoint(Dns.GetHostAddresses(split2[0])[0], int.Parse(split2[1])));
						}
						catch
						{
						}
					}
				}
				catch
				{
				}
			}
			else
			{
				string[] split = data.Split(_separator1);
				if (ScanLocalhost)
				{
					servers.Add(new IPEndPoint(IPAddress.Loopback, 27910));
				}
				foreach (string uri in split)
				{
					try
					{
						string[] split2 = uri.Split(_separator2);
						servers.Add(new IPEndPoint(Dns.GetHostAddresses(split2[0])[0], int.Parse(split2[1])));
					}
					catch
					{
					}
				}
			}
			return servers;
		}

		public async Task<ServerInfo> GetServerInfo(IPEndPoint endPoint)
		{
			var client = new UdpClient
			             {
				             Client =
				             {
					             ReceiveTimeout = ServerTimeout
				             }
			             };
			try
			{
				_stopwatch.Restart();
				int sendBytesCount = await client.SendAsync(_sendBuffer, _sendBuffer.Length, endPoint);
				if (sendBytesCount != _sendBuffer.Length)
				{
					return null;
				}
				var remoteEp = new IPEndPoint(IPAddress.Any, 0);
				var result = await Task.Run(() => client.Receive(ref remoteEp));
				if (result == null || result.Length == 0)
				{
					return null;
				}
				return _serverInfoFactory.Create(endPoint, _stopwatch.ElapsedMilliseconds, Encoding.UTF8.GetString(result, Start, result.Length - Start));
			}
			catch (Exception)
			{
				return null;
			}
			finally
			{
				client.Close();
			}
		}
	}
}