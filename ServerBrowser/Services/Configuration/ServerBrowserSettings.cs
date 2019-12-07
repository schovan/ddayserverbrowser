namespace ServerBrowser.Services.Configuration
{
    public class ServerBrowserSettings
    {
        public string ServerList { get; set; }
        public string Quake2Directory { get; set; }
        public string PlayerName { get; set; }
        public bool ScanLocalhost { get; set; }
        public bool EnableLogging { get; set; }
        public string Quake2Client { get; set; }
        public int ServerTimeout { get; set; }
        public int ServerRefresh { get; set; }
        public string Culture { get; set; }
    }
}