using System;
using System.Collections.Generic;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Leds;

namespace McpLeds
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        List<Led> leds;
        Mcp23x08 mcp;

        public MeadowApp()
        {
            Console.Write("Initialize hardware...");

            mcp = new Mcp23x08(Device.CreateI2cBus(), true, true, true);

            leds = new List<Led>();
            leds.Add(new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP0)));
            leds.Add(new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP1)));
            leds.Add(new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP2)));
            leds.Add(new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP3)));
            leds.Add(new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP4)));
            leds.Add(new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP5)));
            leds.Add(new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP6)));
            leds.Add(new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP7)));

            Console.WriteLine("done.");
            CycleLeds();
        }

        void CycleLeds()
        {
            Console.WriteLine("Cycle leds...");

            while (true)
            {
                foreach(var led in leds)
                {
                    led.IsOn = true;
                    Thread.Sleep(500);
                }

                Thread.Sleep(1000);

                foreach (var led in leds)
                {
                    led.IsOn = false;
                    Thread.Sleep(500);
                }
            }
        }
    }
}
