using Meadow.Foundation.Web.Maple.Server;
using Meadow.Foundation.Web.Maple.Server.Routing;
using MeadowMapleTemperature.Database;
using MeadowMapleTemperature.Entities;
using System.Collections.Generic;
using System.Text.Json;

namespace MeadowMapleTemperature.MapleRequestHandlers
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
                    Temperature = log.TemperatureValue?.ToString("00"),
                    DateTime = log.DateTime.ToString("yyyy-mm-dd hh:mm:ss tt")
                });
            }

            return new JsonResult(JsonSerializer.Serialize(data));
        }
    }
}