using System;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;
using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Foundation.Displays.Tft;
using System.Threading;

namespace TempHumidityMonitorWithTftDisplay
{
    public class TempHumidityMonitorWithTftDisplay : App<F7Micro, TempHumidityMonitorWithTftDisplay>
    {
        IDigitalOutputPort blueLed;
        GraphicsLibrary display;
        Sht31D sensor;

        public TempHumidityMonitorWithTftDisplay()
        {
            InitHardware();

            sensor.StartUpdating(5000);
        }

        public void InitHardware()
        {
            Console.WriteLine("Initialize hardware...");
            blueLed = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedBlue);

            sensor = new Sht31D(Device.CreateI2cBus());
            sensor.Updated += Sensor_Updated;

            var st7789 = new St7789(Device, Device.CreateSpiBus(),
                Device.Pins.D02, Device.Pins.D01, Device.Pins.D00,
                135, 240);

            display = new GraphicsLibrary(st7789);
            display.CurrentFont = new Font12x20();
            display.Rotation = GraphicsLibrary.RotationType._90Degrees; 
        }

        private void Sensor_Updated(object sender, Meadow.Peripherals.Sensors.Atmospheric.AtmosphericConditionChangeResult e)
        {
            Console.WriteLine("Data updated");

            blueLed.State = true;
            Thread.Sleep(20);
            blueLed.State = false;

            display.Clear();

            display.DrawText(0, 10, $"New Temp: {e.New.Temperature}", Color.RoyalBlue);
            display.DrawText(0, 40, $"Old Temp: {e.Old.Temperature}", Color.Gray);
            display.DrawText(0, 70, $"New Humidity: {e.New.Humidity}", Color.RoyalBlue);
            display.DrawText(0, 100, $"Old Humidity: {e.Old.Humidity}", Color.Gray);

            display.Show();
        }
    }
}