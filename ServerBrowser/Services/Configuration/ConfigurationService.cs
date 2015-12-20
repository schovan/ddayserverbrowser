using System.Configuration;

namespace ServerBrowser.Services.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private System.Configuration.Configuration _configuration;

        private ServerBrowserSettings _settings;
        public ServerBrowserSettings Settings
        {
            get { return _settings; }
        }

        public void Load()
        {
            _configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            _settings = (ServerBrowserSettings)_configuration.Sections["ServerBrowserSettings"];
        }

        public void Save()
        {
            _configuration.Save(ConfigurationSaveMode.Modified);
        }
    }
}
