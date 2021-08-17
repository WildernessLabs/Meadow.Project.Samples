using ConnectedServo.Meadow.Controllers;
using Meadow.Foundation.Web.Maple.Server;
using Meadow.Foundation.Web.Maple.Server.Routing;
using Meadow.Units;
using AU = Meadow.Units.Angle.UnitType;

namespace ConnectedServo.Meadow.MapleRequestHandlers
{
    public class ServoControllerRequestHandler : RequestHandlerBase
    {
        public ServoControllerRequestHandler() { }

        [HttpPost]
        public void RotateTo()
        {
            ServoController.Current.RotateTo(new Angle(int.Parse(Body), AU.Degrees));
            StatusResponse();
        }

        [HttpPost]
        public void StartSweep()
        {
            ServoController.Current.StartSweep();
            StatusResponse();
        }

        [HttpPost]
        public void StopSweep()
        {
            ServoController.Current.StopSweep();
            StatusResponse();
        }

        void StatusResponse()
        {
            Context.Response.ContentType = ContentTypes.Application_Text;
            Context.Response.StatusCode = 200;
            Send("OK");
        }
    }
}