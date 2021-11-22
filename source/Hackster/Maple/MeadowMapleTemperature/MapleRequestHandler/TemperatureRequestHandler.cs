using Meadow.Foundation.Web.Maple.Server;
using Meadow.Foundation.Web.Maple.Server.Routing;
using MeadowMapleTemperature.Entities;
using MeadowMapleTemperature.Models;
using System.Collections.Generic;

namespace MeadowMapleTemperature.MapleRequestHandlers
{
    public class TemperatureRequestHandler : RequestHandlerBase
    {
        public TemperatureRequestHandler() { }

        [HttpGet]
        public void GetTemperatureLogs()
        {
            var logs = SQLiteDatabaseManager.Instance.GetTemperatureReadings();

            var data = new List<TemperatureLogEntity>();
            foreach (var log in logs)
            {
                data.Add(new TemperatureLogEntity()
                {
                    Temperature = log.TemperatureValue?.ToString("00"),
                    DateTime = log.DateTime.ToString("yyyy-mm-dd hh:mm:ss tt")
                });
            }

            Context.Response.ContentType = ContentTypes.Application_Json;
            Context.Response.StatusCode = 200;
            Send(data).Wait();
        }
    }
}