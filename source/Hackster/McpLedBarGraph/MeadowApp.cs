using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using System;
using System.Threading;

namespace McpLedBarGraph
{
    // public class MeadowApp : App<F7Micro, MeadowApp> <- If you have a Meadow F7 v1.*
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        Mcp23x08 mcp;
        LedBarGraph ledBarGraph;

        public MeadowApp()
        {
            Initialize();

            ledBarGraph.Percentage = 1;
            CycleLeds();
        }

        void Initialize() 
        {
            var onboardLed = new RgbPwmLed(
                device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            mcp = new Mcp23x08(Device.CreateI2cBus(), true, true, true);

            IDigitalOutputPort[] ports =
            {
                Device.CreateDigitalOutputPort(Device.Pins.D01),
                Device.CreateDigitalOutputPort(Device.Pins.D00),
                mcp.CreateDigitalOutputPort(mcp.Pins.GP7, false, OutputType.PushPull),
                mcp.CreateDigitalOutputPort(mcp.Pins.GP6, false, OutputType.PushPull),
                mcp.CreateDigitalOutputPort(mcp.Pins.GP5, false, OutputType.PushPull),
                mcp.CreateDigitalOutputPort(mcp.Pins.GP4, false, OutputType.PushPull),
                mcp.CreateDigitalOutputPort(mcp.Pins.GP3, false, OutputType.PushPull),
                mcp.CreateDigitalOutputPort(mcp.Pins.GP2, false, OutputType.PushPull),
                mcp.CreateDigitalOutputPort(mcp.Pins.GP1, false, OutputType.PushPull),
                mcp.CreateDigitalOutputPort(mcp.Pins.GP0, false, OutputType.PushPull),
            };

            ledBarGraph = new LedBarGraph(ports);

            onboardLed.SetColor(Color.Green);
        }

        void CycleLeds()
        {
            Console.WriteLine("Cycle leds...");

            float percentage = 0;

            while (true)
            {
                Console.WriteLine("Turning them on using SetLed...");
                for (int i = 0; i < ledBarGraph.Count; i++)
                {
                    ledBarGraph.SetLed(i, true);
                    Thread.Sleep(300);
                }

                Thread.Sleep(1000);

                Console.WriteLine("Turning them off using SetLed...");
                for (int i = ledBarGraph.Count - 1; i >= 0; i--)
                {
                    ledBarGraph.SetLed(i, false);
                    Thread.Sleep(300);
                }

                Thread.Sleep(1000);

                Console.WriteLine("Turning them on using Percentage...");
                while (percentage <= 1)
                {
                    percentage += 0.10f;
                    ledBarGraph.Percentage = Math.Min(1.0f, percentage);
                    Thread.Sleep(100);
                }

                Thread.Sleep(1000);

                Console.WriteLine("Turning them off using Percentage...");
                while (percentage >= 0)
                {
                    percentage -= 0.10f;
                    ledBarGraph.Percentage = Math.Max(0.0f, percentage); ;
                    Thread.Sleep(100);
                }

                Thread.Sleep(1000);

                Console.WriteLine("Blinking for 3 seconds...");
                ledBarGraph.StartBlink();
                Thread.Sleep(3000);
                ledBarGraph.Stop();

                Thread.Sleep(1000);
            }
        }
    }
}