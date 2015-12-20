using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using ServerBrowser.Helpers;
using ServerBrowser.Properties;
using ServerBrowser.Services.Configuration;
using ServerBrowser.Services.Server;

namespace ServerBrowser.ViewModels
{
	public class MainViewModel : Screen
	{
		private readonly IWindowManager _windowManager;
		private readonly IConfigurationService _configurationService;
		private readonly IServerService _serverService;
		private readonly OptionsViewModel _optionsViewModel;
		private readonly Converter _converter;
		private readonly AsyncManualResetEvent _refreshEvent;
		private readonly AsyncManualResetEvent _pauseEvent;
		private CancellationTokenSource _cts;
		private Task _task;
		private string _checkedClient;
		private string _checkedLanguage;
		private ServerViewModel _selectedServer;
		private bool _canExecuteCommands;
		private bool _isTrayVisible;
		private bool _isTimerRunning;

		public MainViewModel(
			IWindowManager windowManager,
			IConfigurationService configurationService,
			IServerService serverService,
			OptionsViewModel optionsViewModel)
		{
			_windowManager = windowManager;
			_configurationService = configurationService;
			_serverService = serverService;
			_optionsViewModel = optionsViewModel;
			_converter = new Converter();
			_refreshEvent = new AsyncManualResetEvent();
			_pauseEvent = new AsyncManualResetEvent();
			_canExecuteCommands = true;
			_isTimerRunning = true;
			Servers = new BindableCollection<ServerViewModel>();
			Players = new BindableCollection<PlayerViewModel>();
			Properties = new BindableCollection<PropertyViewModel>();
			CheckedClient = _converter.ToCheckedClient(_configurationService.Settings.Quake2Client);
			CheckedLanguage = _converter.ToCheckedLanguage(_configurationService.Settings.Culture);

			_serverService.Initialize(
				_configurationService.Settings.ServerList,
				_configurationService.Settings.ScanLocalhost,
				_configurationService.Settings.ServerTimeout);
			_pauseEvent.Set();
		}

		public BindableCollection<ServerViewModel> Servers { get; private set; }
		public BindableCollection<PlayerViewModel> Players { get; private set; }
		public BindableCollection<PropertyViewModel> Properties { get; private set; }

		public string CheckedClient
		{
			get { return _checkedClient; }
			set
			{
				if (_checkedClient != value)
				{
					_checkedClient = value;
					NotifyOfPropertyChange();
					_configurationService.Settings.Quake2Client = _converter.ToQuake2Client(_checkedClient);
				}
			}
		}

		public string CheckedLanguage
		{
			get { return _checkedLanguage; }
			set
			{
				if (_checkedLanguage != value)
				{
					_checkedLanguage = value;
					NotifyOfPropertyChange();
					_configurationService.Settings.Culture = _converter.ToCulture(_checkedLanguage);
				}
			}
		}

		public ServerViewModel SelectedServer
		{
			get { return _selectedServer; }
			set
			{
				if (_selectedServer != value)
				{
					_selectedServer = value;
					NotifyOfPropertyChange();
					UpdateSelectedServer();
				}
			}
		}
		
		public bool CanExecuteCommands
		{
			get { return _canExecuteCommands; }
			set
			{
				if (_canExecuteCommands != value)
				{
					_canExecuteCommands = value;
					NotifyOfPropertyChange();
				}
			}
		}

		public bool IsTrayVisible
		{
			get { return _isTrayVisible; }
			set
			{
				if (_isTrayVisible != value)
				{
					_isTrayVisible = value;
					NotifyOfPropertyChange();
				}
			}
		}

		public bool IsTimerRunning
		{
			get { return _isTimerRunning; }
			set
			{
				if (_isTimerRunning != value)
				{
					_isTimerRunning = value;
					NotifyOfPropertyChange();
					if (_isTimerRunning)
					{
						UnpauseTask();
					}
					else
					{
						PauseTask();
					}
				}
			}
		}

		protected override void OnInitialize()
		{
			base.OnInitialize();
			DisplayName = Resources.AppTitle;
		}

		public async void Exit()
		{
			await StopTask();
			_configurationService.Save();
			Application.Current.Shutdown();
		}

		public async void Options()
		{
			await StopTask();
			if (_windowManager.ShowDialog(_optionsViewModel) == true)
			{
			}
			StartTask();
		}

		public async void Scan()
		{
			CanExecuteCommands = false;
			await StopTask();

			var servers = await _serverService.GetServers();
			Servers.Clear();
			foreach (var server in servers)
			{
				var serverInfo = await _serverService.GetServerInfo(server);
				if (serverInfo == null)
				{
					continue;
				}
				Servers.Add(_converter.ToServerItem(serverInfo));
			}

			StartTask(true);
			CanExecuteCommands = true;
		}

		public void RefreshServers()
		{
			_refreshEvent.Set();
		}

		public void Connect()
		{
			if (SelectedServer == null)
			{
				return;
			}
			CanExecuteCommands = false;
			string fileName = Path.Combine(_configurationService.Settings.Quake2Directory, _configurationService.Settings.Quake2Client);
			string arguments = string.Format("+connect {0} +set game \"dday\" +name \"{1}\"", SelectedServer.Address, _configurationService.Settings.PlayerName);
			try
			{
				var processStartInfo = new ProcessStartInfo
				                       {
					                       FileName = fileName,
					                       Arguments = arguments,
					                       WorkingDirectory = _configurationService.Settings.Quake2Directory
				                       };
				Process.Start(processStartInfo);
			}
			catch (Win32Exception)
			{
				MessageBox.Show(Resources.CannotExecute + fileName);
			}
			CanExecuteCommands = true;
		}

		public void TrayDoubleClick()
		{
			IsTrayVisible = false;
		}

		private void UpdateSelectedServer()
		{
			if (SelectedServer == null)
			{
				return;
			}
			Players.Clear();
			Players.AddRange(SelectedServer.PlayersCol);
			Properties.Clear();
			Properties.AddRange(SelectedServer.PropertiesCol);
		}

		private void StartTask(bool delay = false)
		{
			if (_configurationService.Settings.ServerRefresh == ServerBrowserSettings.ServerRefreshDisabledValue)
			{
				return;
			}
			if (!delay)
			{
				_refreshEvent.Set();
			}
			_cts = new CancellationTokenSource();
			_task = PeriodicRefreshAsync(_cts.Token);
		}

		private async Task StopTask()
		{
			if (_task == null)
			{
				return;
			}
			_cts.Cancel();
			_pauseEvent.Set();
			_refreshEvent.Set();
			await _task;
		}

		private void PauseTask()
		{
			var isSet = _pauseEvent.WaitAsync(0).Result;
			if (isSet)
			{
				_pauseEvent.Reset();
				_refreshEvent.Set();
			}
		}

		private void UnpauseTask()
		{
			var isSet = _pauseEvent.WaitAsync(0).Result;
			if (!isSet)
			{
				_pauseEvent.Set();
			}
		}

		private async Task PeriodicRefreshAsync(CancellationToken token)
		{
			while (true)
			{
				await _refreshEvent.WaitAsync((int)TimeSpan.FromSeconds(_configurationService.Settings.ServerRefresh).TotalMilliseconds);
				_refreshEvent.Reset();
				await _pauseEvent.WaitAsync();
				if (token.IsCancellationRequested)
				{
					break;
				}
				await RefreshServerItems();
			}
		}

		private async Task RefreshServerItems()
		{
			foreach (var serverItem in Servers)
			{
				var serverInfo = await _serverService.GetServerInfo(serverItem.Address);
				if (serverInfo == null)
				{
					continue;
				}
				serverItem.Update(_converter.ToServerItem(serverInfo));
			}
			UpdateSelectedServer();
			Debug.Print("tick " + DateTime.Now);
		}
	}
}