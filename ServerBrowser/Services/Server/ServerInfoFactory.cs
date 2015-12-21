using System.Collections.Generic;
using System.Net;

namespace ServerBrowser.Services.Server
{
    internal class ServerInfoFactory
    {
        private const char Space = ' ';
        private const char DoubleQuotes = '"';
		private const int PropertiesSize = 50;
		private const int PlayersSize = 30;

		private readonly char[] _separator1;
        private readonly char[] _separator2;
        //private char[] Separator3;
		
        public ServerInfoFactory()
        {
            _separator1 = new char[] { '\\' };
            _separator2 = new char[] { '\n' };
            //Separator3 = new char[] { ' ' };
        }

        public ServerInfo Create(IPEndPoint endPoint, long ping, string info)
        {
            string[] split1 = info.Split(_separator1);
            int length1 = split1.Length;
            string[] split2 = split1[length1 - 1].Split(_separator2);
            int length2 = split2.Length;
            int index = 0;
            var properties = new Dictionary<string, string>(PropertiesSize);
            while (index < length1 - 2)
            {
                properties.Add(split1[index++], split1[index++]);
            }
            properties.Add(split1[index], split2[0]);
            index = 1;
            var players = new List<PlayerInfo>(PlayersSize);
            while (index < length2 - 1)
            {
                int pos = 0;
                int startPos1 = 0;
                int startPos2;
                int startPos3;
                while (split2[index][pos] != Space)
                {
                    pos++;
                }
                string scoreStr = split2[index].Substring(startPos1, pos - startPos1);
                startPos2 = ++pos;
                while (split2[index][pos] != Space)
                {
                    pos++;
                }
                string pingStr = split2[index].Substring(startPos2, pos - startPos2);
                pos += 2;
                startPos3 = pos;
                while (split2[index][pos] != DoubleQuotes)
                {
                    pos++;
                }
                string nameStr = split2[index].Substring(startPos3, pos - startPos3);
                players.Add(new PlayerInfo(int.Parse(scoreStr), int.Parse(pingStr), nameStr));
                index++;
            }
			return new ServerInfo(endPoint, ping, properties, players);
        }
    }
}
