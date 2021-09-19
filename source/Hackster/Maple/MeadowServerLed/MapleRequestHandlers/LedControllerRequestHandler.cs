using System;
using Meadow.Foundation.Web.Maple.Server;
using Meadow.Foundation.Web.Maple.Server.Routing;
using MeadowServerLed.Controller;

namespace MeadowServerLed.MapleRequestHandlers
{
    public class LedControllerRequestHandler : RequestHandlerBase
    {
        public LedControllerRequestHandler() { }

        [HttpPost]
        public void TurnOn()
        {
            LedController.Current.TurnOn();
            StatusResponse();
        }

        [HttpPost]
        public void TurnOff()
        {
            LedController.Current.TurnOff();
            StatusResponse();
        }

        [HttpPost]
        public void StartBlink()
        {
            LedController.Current.StartBlink();
            StatusResponse();
        }

        [HttpPost]
        public void StartPulse()
        {
            LedController.Current.StartPulse();
            StatusResponse();
        }

        [HttpPost]
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