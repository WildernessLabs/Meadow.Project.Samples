using Meadow.Foundation.Web.Maple.Server;
using Meadow.Foundation.Web.Maple.Server.Routing;
using System.Collections.Generic;

namespace MeadowMapleTemperature
{
    public class TemperatureRequestHandler : RequestHandlerBase
    {
        public TemperatureRequestHandler() { }

        [HttpGet("/gettemperaturelogs")]
        public IActionResult GetTemperatureLogs()
        {
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

            return new JsonResult(data);
        }
    }
}