using Meadow.Foundation.Maple.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConnectedLed.Client.Client
{
    public class LedClient : MapleClient
    {
        public LedClient(int listenPort = 17756, int listenTimeout = 5000) :
            base(listenPort, listenTimeout)
        { }

        public async Task<bool> SetSignTextAsync(ServerModel server, string text)
        {
            return (await SendCommandAsync("SignText?text=" + text, server.IpAddress));
        }

        public async Task<HttpResponseMessage> SetSignText(ServerModel server, string text)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri($"http://{server.IpAddress}:5417/"),
                Timeout = TimeSpan.FromSeconds(ListenTimeout)
            };

            try
            {
                var response = await client.GetAsync("SignText?text=" + text, HttpCompletionOption.ResponseContentRead);
                return response;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
