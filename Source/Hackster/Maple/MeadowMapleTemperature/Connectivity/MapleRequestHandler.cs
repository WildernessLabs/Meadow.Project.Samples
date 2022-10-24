using Meadow.Foundation;
using Meadow.Foundation.Web.Maple;
using Meadow.Foundation.Web.Maple.Routing;
using MeadowMapleTemperature.Controller;
using MeadowMapleTemperature.Database;
using System.Collections.Generic;

namespace MeadowMapleTemperature
{
    public class MapleRequestHandler : RequestHandlerBase
    {
        public MapleRequestHandler() { }

        [HttpGet("/gettemperaturelogs")]
        public IActionResult GetTemperatureLogs()
        {
            LedController.Instance.SetColor(Color.Cyan);
            var logs = DatabaseManager.Instance.GetTemperatureReadings();

            var data = new List<TemperatureModel>();
            foreach (var log in logs)
            {
                data.Add(new TemperatureModel()
                {
                    Temperature = log.TemperatureCelcius?.ToString("00"),
                    DateTime = log.DateTime.ToString("yyyy-mm-dd hh:mm:ss tt")
                });
            }

            LedController.Instance.SetColor(Color.Green);
            return new JsonResult(data);
        }
    }
}