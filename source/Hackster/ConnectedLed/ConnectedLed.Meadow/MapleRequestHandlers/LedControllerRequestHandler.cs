using System;
using Meadow.Foundation.Web.Maple.Server;
using Meadow.Foundation.Web.Maple.Server.Routing;

namespace ConnectedLed.Meadow
{
    public class LedControllerRequestHandler : RequestHandlerBase
    {
        public LedControllerRequestHandler() { }

        [HttpPost]
        public void RgbLed()
        {
            Console.WriteLine("GET::SignText");

            string text = base.QueryString["command"];
            LedController.Current.TurnOn();

            StatusResponse();
        }

        [HttpPost]
        public void TurnOn()
        {
            Console.WriteLine("POST: TurnOn!");
            LedController.Current.TurnOn();
            StatusResponse();
        }

        [HttpPost]
        public void TurnOff()
        {
            Console.WriteLine("POST: TurnOff!");
            LedController.Current.TurnOff();
            StatusResponse();
        }

        [HttpPost]
        public void StartBlink()
        {
            Console.WriteLine("POST: StartBlink!");
            LedController.Current.StartBlink();
            StatusResponse();
        }

        [HttpPost]
        public void StartPulse()
        {
            Console.WriteLine("POST: StartPulse!");
            LedController.Current.StartPulse();
            StatusResponse();
        }

        [HttpPost]
        public void StartRunningColors()
        {
            Console.WriteLine("POST: Turn On!");
            //StartRunningColors(this, EventArgs.Empty);
            StatusResponse();
        }

        [HttpPost]
        void StatusResponse()
        {
            Console.WriteLine("POST");

            Context.Response.ContentType = ContentTypes.Application_Text;
            Context.Response.StatusCode = 200;
            Send();
        }
    }
}