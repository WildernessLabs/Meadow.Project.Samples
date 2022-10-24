using Meadow.Foundation.Web.Maple;
using Meadow.Foundation.Web.Maple.Routing;
using MeadowMapleLed.Controller;

namespace MeadowMapleLed.MapleRequestHandlers
{
    public class LedControllerRequestHandler : RequestHandlerBase
    {
        public LedControllerRequestHandler() { }

        [HttpPost("/turnon")]
        public IActionResult TurnOn()
        {
            LedController.Current.TurnOn();
            return new OkResult();
        }

        [HttpPost("/turnoff")]
        public IActionResult TurnOff()
        {
            LedController.Current.TurnOff();
            return new OkResult();
        }

        [HttpPost("/startblink")]
        public IActionResult StartBlink()
        {
            LedController.Current.StartBlink();
            return new OkResult();
        }

        [HttpPost("/startpulse")]
        public IActionResult StartPulse()
        {
            LedController.Current.StartPulse();
            return new OkResult();
        }

        [HttpPost("/startrunningcolors")]
        public IActionResult StartRunningColors()
        {
            LedController.Current.StartRunningColors();
            return new OkResult();
        }
    }
}