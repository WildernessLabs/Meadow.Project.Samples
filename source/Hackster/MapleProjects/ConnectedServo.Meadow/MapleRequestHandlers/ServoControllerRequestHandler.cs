using ConnectedServo.Meadow.Controllers;
using Meadow.Foundation.Web.Maple.Server;
using Meadow.Foundation.Web.Maple.Server.Routing;
using System;

namespace ConnectedServo.Meadow.MapleRequestHandlers
{
    public class ServoControllerRequestHandler : RequestHandlerBase
    {
        public ServoControllerRequestHandler() { }

        [HttpPost]
        public void RotateTo()
        {
            Console.WriteLine("POST: TurnOn!");
            ServoController.Current.RotateTo(20);
            StatusResponse();
        }

        [HttpPost]
        public void StartSweep()
        {
            Console.WriteLine("POST: TurnOff!");
            ServoController.Current.StartSweep();
            StatusResponse();
        }

        [HttpPost]
        public void StopSweep()
        {
            Console.WriteLine("POST: StartBlink!");
            ServoController.Current.StopSweep();
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
