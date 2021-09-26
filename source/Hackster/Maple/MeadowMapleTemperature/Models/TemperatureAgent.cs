using Meadow.Foundation.Sensors.Temperature;
using System;

namespace MeadowMapleTemperature.Models
{
    public class TemperatureAgent
    {
        AnalogTemperature analogTemperature;

        public event EventHandler<TemperatureModel> TemperatureUpdated = delegate { };

        public TemperatureAgent()
        {
            Initialize();
        }

        void Initialize()
        {
            analogTemperature = new AnalogTemperature(MeadowApp.Device, MeadowApp.Device.Pins.A00, AnalogTemperature.KnownSensorType.LM35);
            analogTemperature.StartUpdating(TimeSpan.FromSeconds(30));
            analogTemperature.TemperatureUpdated += AnalogTemperatureUpdated;
        }

        void AnalogTemperatureUpdated(object sender, Meadow.IChangeResult<Meadow.Units.Temperature> e)
        {
            var reading = new TemperatureModel()
            {
                Temperature = e.New,
                DateTime = DateTime.Now
            };

            TemperatureUpdated(this, reading);
        }
    }
}