using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using ServerBrowser.Properties;
using ServerBrowser.Services.Configuration;
using ServerBrowser.Services.Dialog;

namespace ServerBrowser.ViewModels
{
	public class OptionsViewModel : Screen
	{
		private readonly IDialogService _dialogService;
		private readonly IConfigurationService _configurationService;
		private bool? _dialogResult;
		private string _serverList;
		private string _quake2Path;
		private string _playerName;
		private bool _scanLocalhost;
		private bool _enableLogging;
		private int _serverTimeout;
		private int _serverRefresh;
		private bool _useGlobalServerList;
		private bool _disableServerRefresh;
		
		public OptionsViewModel(IDialogService dialogService, IConfigurationService configurationService)
		{
			_dialogService = dialogService;
			_configurationService = configurationService;
			LoadSettings();
		}

		public bool? DialogResult
		{
			get { return _dialogResult; }
			set
			{
				if (_dialogResult != value)
				{
					_dialogResult = value;
					NotifyOfPropertyChange();
				}
			}
		}

		public string ServerList
		{
			get { return _serverList; }
			set
			{
				if (_serverList != value)
				{
					_serverList = value;
					NotifyOfPropertyChange();
				}
			}
		}

		public string Quake2Path
		{
			get { return _quake2Path; }
			set
			{
				if (_quake2Path != value)
				{
					_quake2Path = value;
					NotifyOfPropertyChange();
				}
			}
		}

		public string PlayerName
		{
			get { return _playerName; }
			set
			{
				if (_playerName != value)
				{
					_playerName = value;
					NotifyOfPropertyChange();
				}
			}
		}

		public bool ScanLocalhost
		{
			get { return _scanLocalhost; }
			set
			{
				if (_scanLocalhost != value)
				{
					_scanLocalhost = value;
					NotifyOfPropertyChange();
				}
			}
		}

		public bool EnableLogging
		{
			get { return _enableLogging; }
			set
			{
				if (_enableLogging != value)
				{
					_enableLogging = value;
					NotifyOfPropertyChange();
				}
			}
		}

		public int ServerTimeout
		{
			get { return _serverTimeout; }
			set
			{
				if (_serverTimeout != value)
				{
					_serverTimeout = value;
					NotifyOfPropertyChange();
				}
			}
		}

		public int ServerRefresh
		{
			get { return _serverRefresh; }
			set
			{
				if (_serverRefresh != value)
				{
					_serverRefresh = value;
					NotifyOfPropertyChange();
				}
			}
		}

		public bool UseGlobalServerList
		{
			get { return _useGlobalServerList; }
			set
			{
				if (_useGlobalServerList != value)
				{
					_useGlobalServerList = value;
					NotifyOfPropertyChange();
				}
			}
		}

		public bool DisableServerRefresh
		{
			get { return _disableServerRefresh; }
			set
			{
				if (_disableServerRefresh != value)
				{
					_disableServerRefresh = value;
					NotifyOfPropertyChange();
				}
			}
		}

        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
		{
			DisplayName = Resources.Options;
            return Task.CompletedTask;
		}

		public void Ok()
		{
			DialogResult = true;
			SaveSettings();
			_configurationService.Save();
		}

		public void Cancel()
		{
			DialogResult = false;
		}

		public void BrowseQuake2Location()
		{
			string fileName = _dialogService.ShowOpenFileDialog("Executable files (*.exe)|*.exe");
			if (fileName != null)
			{
				Quake2Path = fileName;
			}
		}

		public void BrowseServerListLocation()
		{
			string fileName = _dialogService.ShowOpenFileDialog("All files (*.*)|*.*");
			if (fileName != null)
			{
				ServerList = fileName;
			}
		}

		private void LoadSettings()
		{
            _configurationService.Load();
			ServerList = _configurationService.Settings.ServerList;
			if (_configurationService.Settings.ServerList == ServerBrowserSettings.ServerListDefaultValue)
			{
				UseGlobalServerList = true;
			}
			Quake2Path = Path.Combine(_configurationService.Settings.Quake2Directory, _configurationService.Settings.Quake2Client);
			PlayerName = _configurationService.Settings.PlayerName;
			ScanLocalhost = _configurationService.Settings.ScanLocalhost;
			EnableLogging = _configurationService.Settings.EnableLogging;
			ServerTimeout = _configurationService.Settings.ServerTimeout;
			if (_configurationService.Settings.ServerRefresh == ServerBrowserSettings.ServerRefreshDisabledValue)
			{
				ServerRefresh = ServerBrowserSettings.ServerRefreshDefaultValue;
				DisableServerRefresh = true;
			}
			else
			{
				ServerRefresh = _configurationService.Settings.ServerRefresh;
			}
		}

		private void SaveSettings()
		{
			_configurationService.Settings.ServerList = ServerList;
			_configurationService.Settings.Quake2Directory = Path.GetDirectoryName(Quake2Path);
			_configurationService.Settings.PlayerName = PlayerName;
			_configurationService.Settings.ScanLocalhost = ScanLocalhost;
			_configurationService.Settings.EnableLogging = EnableLogging;
			_configurationService.Settings.ServerTimeout = ServerTimeout;
			_configurationService.Settings.ServerRefresh = DisableServerRefresh ? ServerBrowserSettings.ServerRefreshDisabledValue : ServerRefresh;
		}
	}
}