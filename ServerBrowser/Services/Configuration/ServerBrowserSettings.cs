using System.Configuration;

namespace ServerBrowser.Services.Configuration
{
    public class ServerBrowserSettings : ConfigurationSection
    {
        public const string ServerListDefaultValue = "http://www.quakeservers.net/quake2/servers/t=dday/so=8/";
        public const int ServerRefreshDisabledValue = 0;
        public const int ServerRefreshDefaultValue = 10;

        [ConfigurationProperty("ServerList", DefaultValue = ServerListDefaultValue)]
        public string ServerList
        {
            get { return (string)this["ServerList"]; }
            set { this["ServerList"] = value; }
        }
        [ConfigurationProperty("Quake2Directory", DefaultValue = @"d:\games\quake2")]
        public string Quake2Directory
        {
            get { return (string)this["Quake2Directory"]; }
            set { this["Quake2Directory"] = value; }
        }
        [ConfigurationProperty("PlayerName", DefaultValue = "newbie")]
        public string PlayerName
        {
            get { return (string)this["PlayerName"]; }
            set { this["PlayerName"] = value; }
        }
        [ConfigurationProperty("ScanLocalhost", DefaultValue = false)]
        public bool ScanLocalhost
        {
            get { return (bool)this["ScanLocalhost"]; }
            set { this["ScanLocalhost"] = value; }
        }
        [ConfigurationProperty("EnableLogging", DefaultValue = false)]
        public bool EnableLogging
        {
            get { return (bool)this["EnableLogging"]; }
            set { this["EnableLogging"] = value; }
        }
        [ConfigurationProperty("Quake2Client", DefaultValue = "quake2.exe")]
        public string Quake2Client
        {
            get { return (string)this["Quake2Client"]; }
            set { this["Quake2Client"] = value; }
        }
        [ConfigurationProperty("ServerTimeout", DefaultValue = 300)]
        [IntegerValidator(MinValue = 10, MaxValue = 1000, ExcludeRange = false)]
        public int ServerTimeout
        {
            get { return (int)this["ServerTimeout"]; }
            set { this["ServerTimeout"] = value; }
        }
        [ConfigurationProperty("ServerRefresh", DefaultValue = ServerRefreshDefaultValue)]
        [IntegerValidator(MinValue = 0, MaxValue = 60, ExcludeRange = false)]
        public int ServerRefresh
        {
            get { return (int)this["ServerRefresh"]; }
            set { this["ServerRefresh"] = value; }
        }
        [ConfigurationProperty("Culture", DefaultValue = "en-US")]
        public string Culture
        {
            get { return (string)this["Culture"]; }
            set { this["Culture"] = value; }
        }
    }
}
