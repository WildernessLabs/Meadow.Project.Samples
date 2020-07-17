using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Moisture;
using Meadow.Hardware;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MoistureMeter
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        const float MINIMUM_VOLTAGE_CALIBRATION = 2.84f;
        const float MAXIMUM_VOLTAGE_CALIBRATION = 1.37f;

        Capacitive capacitive;
        LedBarGraph ledBarGraph;        

        public MeadowApp()
        {
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

            StartReading();
        }

        async Task StartReading()
        {
            while (true)
            {
                float moisture = await capacitive.Read();

                if (moisture > 1)
                    moisture = 1f;
                else
                if (moisture < 0)
                    moisture = 0f;

                ledBarGraph.Percentage = moisture;
                Console.WriteLine($"Moisture {moisture * 100}%");
                Thread.Sleep(1000);
            }
        }
    }
}
