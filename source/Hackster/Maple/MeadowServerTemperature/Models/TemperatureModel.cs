using System;
using MU = Meadow.Units;
using SQLite;

namespace MeadowServerTemperature.Models
{
    [Table("TemperatureReadings")]
    public class TemperatureModel
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