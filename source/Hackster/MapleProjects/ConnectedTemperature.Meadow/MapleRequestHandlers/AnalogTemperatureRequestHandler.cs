using ConnectedTemperature.Meadow.Controllers;
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
            Context.Response.ContentType = ContentTypes.Application_Json;
            Context.Response.StatusCode = 200;
            Send(logs).Wait();
        }
    }
}
