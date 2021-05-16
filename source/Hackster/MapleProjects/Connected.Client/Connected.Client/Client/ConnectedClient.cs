using Meadow.Foundation.Maple.Client;
using System.Threading.Tasks;

namespace Connected.Client
{
    public class ConnectedClient : MapleClient
    {
        public ConnectedClient(int listenPort = 17756, int listenTimeout = 5000) :
            base(listenPort, listenTimeout)
        { }

        public async Task<bool> SendCommand(ServerModel server, int serverPort, string command)
        {
            return (await SendCommandAsync(command, $"{server.IpAddress}:{serverPort}"));
        }
    }
}