namespace ServerBrowser.Services.Server
{
    public class PlayerInfo
    {
        public string Name;
        public int Ping;
        public int Score;

        public PlayerInfo(int score, int ping, string name)
        {
            this.Name = name;
            this.Ping = ping;
            this.Score = score;
        }
    }
}
