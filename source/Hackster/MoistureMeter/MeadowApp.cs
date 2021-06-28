using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Moisture;
using Meadow.Hardware;
using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow.Units;
using VU = Meadow.Units.Voltage.UnitType;

namespace MoistureMeter
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        readonly Voltage MINIMUM_VOLTAGE_CALIBRATION = new Voltage(2.84, VU.Volts);
        readonly Voltage MAXIMUM_VOLTAGE_CALIBRATION = new Voltage(1.37, VU.Volts);

        Capacitive capacitive;
        LedBarGraph ledBarGraph;        

        public MeadowApp()
        {
            var led = new RgbLed(Device, Device.Pins.OnboardLedRed, Device.Pins.OnboardLedGreen, Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            IDigitalOutputPort[] ports =
            {
                Device.CreateDigitalOutputPort(Device.Pins.D05),
                Device.CreateDigitalOutputPort(Device.Pins.D06),
                Device.CreateDigitalOutputPort(Device.Pins.D07),
                Device.CreateDigitalOutputPort(Device.Pins.D08),
                Device.CreateDigitalOutputPort(Device.Pins.D09),
                Device.CreateDigitalOutputPort(Device.Pins.D10),
                Device.CreateDigitalOutputPort(Device.Pins.D11),
                Device.CreateDigitalOutputPort(Device.Pins.D12),
                Device.CreateDigitalOutputPort(Device.Pins.D13),
                Device.CreateDigitalOutputPort(Device.Pins.D14)
            };

            ledBarGraph = new LedBarGraph(ports);
            capacitive = new Capacitive
            (
                Device.CreateAnalogInputPort(Device.Pins.A00),
                MINIMUM_VOLTAGE_CALIBRATION,
                MAXIMUM_VOLTAGE_CALIBRATION
            );

            led.SetColor(RgbLed.Colors.Green);

            StartReading();
        }

        async Task StartReading()
        {
            while (true)
            {
                var reading = await capacitive.Read();
                double moisture = reading;

                if (moisture > 1)
                    moisture = 1f;
                else
                if (moisture < 0)
                    moisture = 0f;

                ledBarGraph.Percentage = (float) moisture;
                Console.WriteLine($"Moisture {moisture * 100}%");
                Thread.Sleep(1000);
            }
        }
    }
}
