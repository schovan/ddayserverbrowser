namespace ServerBrowser.Services.Configuration
{
    public interface IConfigurationService
    {
        ServerBrowserSettings Settings { get; }
        string ServerListDefaultValue { get; }
        int ServerRefreshDisabledValue { get; }
        int ServerRefreshDefaultValue { get; }
        void Load();
        void Save();
    }
}
