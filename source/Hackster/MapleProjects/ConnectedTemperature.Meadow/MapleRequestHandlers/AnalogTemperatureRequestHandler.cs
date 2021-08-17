using ConnectedTemperature.Meadow.Controllers;
using Json.Net;
using Meadow.Foundation.Web.Maple.Server;
using Meadow.Foundation.Web.Maple.Server.Routing;

namespace ConnectedTemperature.Meadow.MapleRequestHandlers
{
    public class AnalogTemperatureRequestHandler : RequestHandlerBase
    {
        public AnalogTemperatureRequestHandler() { }

        [HttpGet]
        public void GetTemperature() 
        {
            var logs = AnalogTemperatureController.Current.GetTemperatureLog();

            string stringToJson = JsonNet.Serialize(logs);
            Context.Response.ContentType = ContentTypes.Application_Json;
            Context.Response.StatusCode = 200;
            Send(stringToJson).Wait();
        }
    }
}
