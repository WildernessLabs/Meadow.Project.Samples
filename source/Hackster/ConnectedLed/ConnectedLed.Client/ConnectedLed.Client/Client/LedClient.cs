using Meadow.Foundation.Maple.Client;
using System.Threading.Tasks;

namespace ConnectedLed.Client.Client
{
    public class LedClient : MapleClient
    {
        public LedClient(int listenPort = 17756, int listenTimeout = 5000) :
            base(listenPort, listenTimeout)
        { }

        public async Task<bool> SendLedCommand(ServerModel server, string command)
        {
            return (await SendCommandAsync(command, $"{server.IpAddress}:5417"));
        }
    }
}