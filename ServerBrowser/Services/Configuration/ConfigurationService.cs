using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace ServerBrowser.Services.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        public ServerBrowserSettings Settings { get; private set; }
        public string ServerListDefaultValue { get; } = "http://www.quakeservers.net/quake2/servers/t=dday/so=8/";
        public int ServerRefreshDisabledValue { get; } = 0;
        public int ServerRefreshDefaultValue { get; } = 10;

        public void Load()
        {
            Settings = JsonConvert.DeserializeObject<ServerBrowserSettings>(File.ReadAllText(GetConfigFilePath()));
        }

        public void Save()
        {
            File.WriteAllText(GetConfigFilePath(), JsonConvert.SerializeObject(Settings, Formatting.Indented));
        }

        private string GetConfigFilePath()
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "appsettings.json");
        }
    }
}