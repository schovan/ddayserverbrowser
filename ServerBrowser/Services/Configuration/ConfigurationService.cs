namespace ServerBrowser.Services.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        public ServerBrowserSettings Settings { get; private set; }

        public void Load()
        {
            Settings = new ServerBrowserSettings()
            {
                ServerList = "http://www.quakeservers.net/quake2/servers/t=dday/so=8/", Quake2Directory = @"d:\games\quake2", PlayerName = "newbie", Quake2Client = "quake2.exe",
                ServerTimeout = 300, ServerRefresh = 10, Culture = "en-US"
            };
        }

        public void Save()
        {
        }
    }
}