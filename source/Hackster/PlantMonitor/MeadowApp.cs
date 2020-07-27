using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Moisture;
using Meadow.Hardware;

namespace PlantMonitor
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        LedBarGraph ledBarGraph;
        Capacitive capacitive;

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

            capacitive = new Capacitive(
                analogPort: Device.CreateAnalogInputPort(Device.Pins.A00), 
                minimumVoltageCalibration: 2.84f, 
                maximumVoltageCalibration: 1.37f);

            Run();
        }

        public async Task Run()
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

                Console.WriteLine($"Raw: {capacitive.Moisture} | Moisture {moisture * 100}%");
                Thread.Sleep(1000);
            }
        }
    }
}
