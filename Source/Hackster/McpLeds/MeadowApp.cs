using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Leds;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace McpLeds
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        List<Led> leds;
        Mcp23008 mcp;

        public override Task Initialize() 
        {
            var onboardLed = new RgbPwmLed(
                device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            mcp = new Mcp23008(Device.CreateI2cBus());

            leds = new List<Led>();
            leds.Add(new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP0)));
            leds.Add(new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP1)));
            leds.Add(new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP2)));
            leds.Add(new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP3)));
            leds.Add(new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP4)));
            leds.Add(new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP5)));
            leds.Add(new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP6)));
            leds.Add(new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP7)));

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        void CycleLeds()
        {
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

        public override Task Run()
        {
            CycleLeds();

            return base.Run();
        }
    }
}