namespace ServerBrowser.Services.Configuration
{
    public interface IConfigurationService
    {
        ServerBrowserSettings Settings { get; }
        void Load();
        void Save();
    }
}
