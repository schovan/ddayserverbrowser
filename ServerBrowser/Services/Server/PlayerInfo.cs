namespace ServerBrowser.Services.Server
{
	public class PlayerInfo
	{
		public PlayerInfo(int score, int ping, string name)
		{
			Name = name;
			Ping = ping;
			Score = score;
		}

		public string Name { get; private set; }
		public int Ping { get; private set; }
		public int Score { get; private set; }
	}
}