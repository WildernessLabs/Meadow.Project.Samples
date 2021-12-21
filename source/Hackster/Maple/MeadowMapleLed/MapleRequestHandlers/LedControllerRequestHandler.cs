using Meadow.Foundation.Web.Maple.Server;
using Meadow.Foundation.Web.Maple.Server.Routing;
using MeadowMapleLed.Controller;

namespace MeadowMapleLed.MapleRequestHandlers
{
    public class LedControllerRequestHandler : RequestHandlerBase
    {
        public LedControllerRequestHandler() { }

        [HttpPost("/turnon")]
        public void TurnOn()
        {
            LedController.Current.TurnOn();
            StatusResponse();
        }

        [HttpPost("/turnoff")]
        public void TurnOff()
        {
            LedController.Current.TurnOff();
            StatusResponse();
        }

        [HttpPost("/startblink")]
        public void StartBlink()
        {
            LedController.Current.StartBlink();
            StatusResponse();
        }

        [HttpPost("/startblink")]
        public void StartPulse()
        {
            LedController.Current.StartPulse();
            StatusResponse();
        }

        [HttpPost("/startblink")]
        public void StartRunningColors()
        {
            LedController.Current.StartRunningColors();
            StatusResponse();
        }

        void StatusResponse()
        {
            Context.Response.ContentType = ContentTypes.Application_Text;
            Context.Response.StatusCode = 200;
            Send();
        }
    }
}