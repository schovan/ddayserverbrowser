using Caliburn.Micro;

namespace ServerBrowser.ViewModels
{
	public class PlayerViewModel : PropertyChangedBase
	{
		private string _name;
		private string _score;
		private string _ping;

		public PlayerViewModel(string name, string score, string ping)
		{
			_name = name;
			_score = score;
			_ping = ping;
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

		public string Score
		{
			get { return _score; }
			set
			{
				if (_score != value)
				{
					_score = value;
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
	}
}