using System.Collections.Generic;
using System.Net;

namespace ServerBrowser.Services.Server
{
    public class ServerInfo
    {
        #region private constants

        private const char Space = ' ';
        private const char DoubleQuotes = '"';

        #endregion

        #region static members

        private static char[] Separator1;
        private static char[] Separator2;
        private static char[] Separator3;

        #endregion

        #region static constructor

        static ServerInfo()
        {
            Separator1 = new char[] { '\\' };
            Separator2 = new char[] { '\n' };
            Separator3 = new char[] { ' ' };
        }

        #endregion

        #region private constants

        private const int PropertiesSize = 50;
        private const int PlayersSize = 30;

        #endregion

        #region private members

        private Dictionary<string, string> properties;
        private List<PlayerInfo> players;
        private long ping;
        private IPEndPoint endPoint;

        #endregion

        #region properties

        public Dictionary<string, string> Properties
        {
            get
            {
                return properties;
            }
        }

        public List<PlayerInfo> Players
        {
            get
            {
                return players;
            }
        }

        public long Ping
        {
            get
            {
                return ping;
            }
        }

        public IPEndPoint EndPoint
        {
            get
            {
                return endPoint;
            }
        }

        #endregion

        #region constructor

        public ServerInfo(IPEndPoint endPoint, long ping, string info)
        {
            this.endPoint = endPoint;
            this.ping = ping;
            string[] split1 = info.Split(Separator1);
            int length1 = split1.Length;
            string[] split2 = split1[length1 - 1].Split(Separator2);
            int length2 = split2.Length;
            int index = 0;
            properties = new Dictionary<string, string>(PropertiesSize);
            while (index < length1 - 2)
            {
                properties.Add(split1[index++], split1[index++]);
            }
            properties.Add(split1[index], split2[0]);
            index = 1;
            players = new List<PlayerInfo>(PlayersSize);
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
        }

        #endregion
    }
}
