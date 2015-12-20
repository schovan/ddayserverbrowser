using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Media.Imaging;
using ServerBrowser.Properties;
using ServerBrowser.Services.Server;
using ServerBrowser.ViewModels;

namespace ServerBrowser.Helpers
{
	public class Converter
	{
		public const int MaxPing = 300;

		private readonly List<BitmapSource> _bitmapSources;

		public Converter()
		{
			_bitmapSources = Bitmaps.GetBitmaps();
		}

		public ServerViewModel ToServerItem(ServerInfo serverInfo)
		{
			var bitmapSourceIndex = (int)((double)serverInfo.Ping / MaxPing * _bitmapSources.Count);
			if (bitmapSourceIndex > _bitmapSources.Count - 1)
			{
				bitmapSourceIndex = _bitmapSources.Count - 1;
			}
			return new ServerViewModel(
				_bitmapSources[bitmapSourceIndex],
				serverInfo.Properties["hostname"],
				serverInfo.Ping.ToString(CultureInfo.InvariantCulture),
				string.Format("{0}/{1}", serverInfo.Players.Count, serverInfo.Properties["maxclients"]),
				serverInfo.Properties["mapname"],
				serverInfo.EndPoint,
				serverInfo.Players.Select(q => new PlayerViewModel(q.Name, q.Ping.ToString(CultureInfo.InvariantCulture), q.Score.ToString(CultureInfo.InvariantCulture)))
					.ToList(),
				serverInfo.Properties.Select(q => new PropertyViewModel(q.Key, q.Value)).ToList());
		}

		public string ToCheckedClient(string quake2Client)
		{
			if (quake2Client == Resources.Quake2Exe)
			{
				return Resources.Quake2Header;
			}
			if (quake2Client == Resources.EglExe)
			{
				return Resources.EglHeader;
			}
			if (quake2Client == Resources.R1Q2Exe)
			{
				return Resources.R1Q2Header;
			}
			throw new ArgumentException(Resources.NotSupportedValue, "quake2Client");
		}

		public string ToQuake2Client(string checkedHeader)
		{
			if (checkedHeader == Resources.Quake2Header)
			{
				return Resources.Quake2Exe;
			}
			if (checkedHeader == Resources.EglHeader)
			{
				return Resources.EglExe;
			}
			if (checkedHeader == Resources.R1Q2Header)
			{
				return Resources.R1Q2Exe;
			}
			throw new ArgumentException(Resources.NotSupportedValue, "checkedHeader");
		}

		public string ToCheckedLanguage(string culture)
		{
			if (culture == Resources.EnglishCulture)
			{
				return Resources.EnglishHeader;
			}
			if (culture == Resources.CzechCulture)
			{
				return Resources.CzechHeader;
			}
			throw new ArgumentException(Resources.NotSupportedValue, "culture");
		}

		public string ToCulture(string checkedLanguage)
		{
			if (checkedLanguage == Resources.EnglishHeader)
			{
				return Resources.EnglishCulture;
			}
			if (checkedLanguage == Resources.CzechHeader)
			{
				return Resources.CzechCulture;
			}
			throw new ArgumentException(Resources.NotSupportedValue, "checkedLanguage");
		}
	}
}