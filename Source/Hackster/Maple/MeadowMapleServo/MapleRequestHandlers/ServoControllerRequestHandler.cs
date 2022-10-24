using Meadow.Foundation.Web.Maple;
using Meadow.Foundation.Web.Maple.Routing;
using Meadow.Units;
using MeadowMapleServo.Controllers;
using AU = Meadow.Units.Angle.UnitType;

namespace MeadowMapleServo.MapleRequestHandlers
{
    public class ServoControllerRequestHandler : RequestHandlerBase
    {
        public ServoControllerRequestHandler() { }

        [HttpPost("/rotateto")]
        public IActionResult RotateTo()
        {
            int angle = int.Parse(Body);
            ServoController.Current.RotateTo(new Angle(angle, AU.Degrees));
            return new OkResult();
        }

        [HttpPost("/startsweep")]
        public IActionResult StartSweep()
         {
            ServoController.Current.StartSweep();
            return new OkResult();
        }

        [HttpPost("/stopsweep")]
        public IActionResult StopSweep()
        {
            ServoController.Current.StopSweep();
            return new OkResult();
        }
    }
}