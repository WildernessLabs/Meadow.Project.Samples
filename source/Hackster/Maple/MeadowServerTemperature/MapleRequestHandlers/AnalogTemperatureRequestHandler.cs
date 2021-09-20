using Meadow.Foundation.Web.Maple.Server;
using Meadow.Foundation.Web.Maple.Server.Routing;
using MeadowServerTemperature.Entities;
using MeadowServerTemperature.Models;
using System.Collections.Generic;

namespace MeadowServerTemperature.MapleRequestHandlers
{
    public class AnalogTemperatureRequestHandler : RequestHandlerBase
    {
        public AnalogTemperatureRequestHandler() { }

        [HttpGet]
        public void GetTemperatureLogs() 
        {
            var logs = SQLiteDatabaseManager.Instance.GetTemperatureReadings();

            var data = new List<TemperatureLogEntity>();
            foreach (var log in logs)
            {
                data.Add(new TemperatureLogEntity()
                {
                    Temperature = log.TemperatureValue,
                    DateTime = log.DateTime
                });
            }

            Context.Response.ContentType = ContentTypes.Application_Json;
            Context.Response.StatusCode = 200;
            Send(data).Wait();
        }
    }
}