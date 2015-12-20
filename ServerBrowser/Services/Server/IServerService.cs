using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ServerBrowser.Services.Server
{
    public interface IServerService
    {
        void Initialize(string serverList, bool scanLocalhost, int serverTimeout);
        Task<List<IPEndPoint>> GetServers();
        Task<ServerInfo> GetServerInfo(IPEndPoint endPoint);
    }
}
