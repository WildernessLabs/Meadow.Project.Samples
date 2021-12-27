using SQLite;
using System;
using MU = Meadow.Units;

namespace MeadowMapleTemperature.Database
{
    [Table("TemperatureReadings")]
    public class TemperatureTable
    {
        [PrimaryKey, AutoIncrement]
        public int? ID { get; set; }

        public DateTime DateTime { get; set; }

        public double? TemperatureValue
        {
            get => Temperature?.Celsius;
            set => Temperature = new MU.Temperature(value.Value, MU.Temperature.UnitType.Celsius);
        }

        [Ignore]
        public MU.Temperature? Temperature { get; set; }
    }
}