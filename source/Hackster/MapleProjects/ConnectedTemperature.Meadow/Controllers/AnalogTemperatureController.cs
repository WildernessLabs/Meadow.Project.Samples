using ConnectedTemperature.Meadow.DTOs;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Hardware;
using System;
using System.Collections.Generic;

namespace ConnectedTemperature.Meadow.Controllers
{
    public class AnalogTemperatureController
    {
        AnalogTemperature analogTemperature;

        List<TemperatureEntity> temperatureList;

        protected bool initialized = false;

        public static AnalogTemperatureController Current { get; private set; }

        private AnalogTemperatureController() { }

        static AnalogTemperatureController()
        {
            Current = new AnalogTemperatureController();
        }

        public void Initialize(IAnalogInputController device, IPin analogPin)
        {
            if (initialized) { return; }

            temperatureList = new List<TemperatureEntity>();

            analogTemperature = new AnalogTemperature(device, analogPin, AnalogTemperature.KnownSensorType.LM35);
            analogTemperature.TemperatureUpdated += TemperatureUpdated;
            analogTemperature.StartUpdating(TimeSpan.FromSeconds(5));

            initialized = true;
        }

        void TemperatureUpdated(object sender, global::Meadow.IChangeResult<global::Meadow.Units.Temperature> e)
        {
            temperatureList.Add(new TemperatureEntity()
            {
                temperature = e.New.Celsius.ToString("0.0"),
                dateTime = DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss tt")
            });
        }

        public List<TemperatureEntity> GetTemperatureLog()
        {
            return temperatureList;
        }
    }
}