using Caliburn.Micro;

namespace ServerBrowser.ViewModels
{
	public class PropertyViewModel : PropertyChangedBase
	{
		private string _key;
		private string _value;

		public PropertyViewModel(string key, string value)
		{
			_key = key;
			_value = value;
		}

		public string Key
		{
			get { return _key; }
			set
			{
				if (_key != value)
				{
					_key = value;
					NotifyOfPropertyChange();
				}
			}
		}

		public string Value
		{
			get { return _value; }
			set
			{
				if (_value != value)
				{
					_value = value;
					NotifyOfPropertyChange();
				}
			}
		}
	}
}