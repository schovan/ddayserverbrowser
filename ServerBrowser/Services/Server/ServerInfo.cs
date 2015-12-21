using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerBrowser.Services.Server
{
	public class ServerInfo
	{
		public IPEndPoint EndPoint { get; private set; }

		public long Ping { get; private set; }

		public Dictionary<string, string> Properties { get; private set; }

		public List<PlayerInfo> Players { get; private set; }

		public ServerInfo(IPEndPoint endPoint, long ping, Dictionary<string, string> properties, List<PlayerInfo> players)
		{
			EndPoint = endPoint;
			Ping = ping;
			Properties = properties;
			Players = players;
		}
	}
}
