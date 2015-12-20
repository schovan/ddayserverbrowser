using System.Collections.Generic;
using System.Net;
using System.Windows.Media.Imaging;
using Caliburn.Micro;

namespace ServerBrowser.ViewModels
{
	public class ServerViewModel : PropertyChangedBase
	{
		private BitmapSource _image;
		private string _name;
		private string _ping;
		private string _players;
		private string _map;
		private IPEndPoint _address;
		private List<PlayerViewModel> _playersCol;
		private List<PropertyViewModel> _propertiesCol;

		public ServerViewModel(BitmapSource image, string name, string ping, string players, string map, IPEndPoint address, List<PlayerViewModel> playersCol, List<PropertyViewModel> propertiesCol)
		{
			_image = image;
			_name = name;
			_ping = ping;
			_players = players;
			_map = map;
			_address = address;
			_playersCol = playersCol;
			_propertiesCol = propertiesCol;
		}

		public BitmapSource Image
		{
			get { return _image; }
			set
			{
				if (_image != value)
				{
					_image = value;
					NotifyOfPropertyChange();
				}
			}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				if (_name != value)
				{
					_name = value;
					NotifyOfPropertyChange();
				}
			}
		}

		public string Ping
		{
			get { return _ping; }
			set
			{
				if (_ping != value)
				{
					_ping = value;
					NotifyOfPropertyChange();
				}
			}
		}

		public string Players
		{
			get { return _players; }
			set
			{
				if (_players != value)
				{
					_players = value;
					NotifyOfPropertyChange();
				}
			}
		}

		public string Map
		{
			get { return _map; }
			set
			{
				if (_map != value)
				{
					_map = value;
					NotifyOfPropertyChange();
				}
			}
		}

		public IPEndPoint Address
		{
			get { return _address; }
			set
			{
				if (_address != value)
				{
					_address = value;
					NotifyOfPropertyChange();
				}
			}
		}

		public List<PlayerViewModel> PlayersCol
		{
			get { return _playersCol; }
			set
			{
				if (_playersCol != value)
				{
					_playersCol = value;
					NotifyOfPropertyChange();
				}
			}
		}

		public List<PropertyViewModel> PropertiesCol
		{
			get { return _propertiesCol; }
			set
			{
				if (_propertiesCol != value)
				{
					_propertiesCol = value;
					NotifyOfPropertyChange();
				}
			}
		}

		public void Update(ServerViewModel serverItem)
		{
			Image = serverItem.Image;
			Name = serverItem.Name;
			Ping = serverItem.Ping;
			Players = serverItem.Players;
			Map = serverItem.Map;
			Address = serverItem.Address;
			PlayersCol = serverItem.PlayersCol;
			PropertiesCol = serverItem.PropertiesCol;
		}
	}
}