using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;

namespace RgbLedSample
{
    // public class MeadowApp : App<F7Micro, MeadowApp> <- If you have a Meadow F7 v1.*
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        RgbLed rgbLed;
        RgbPwmLed rgbPwmLed;

        public MeadowApp()
        {
            Initialize();

            TestRgbLed();
            TestRgbPwmLed();
        }

        void Initialize() 
        {
            var onboardLed = new RgbPwmLed(
                device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            rgbLed = new RgbLed(
                Device.CreateDigitalOutputPort(Device.Pins.D02),
                Device.CreateDigitalOutputPort(Device.Pins.D03),
                Device.CreateDigitalOutputPort(Device.Pins.D04)
            );
            rgbPwmLed = new RgbPwmLed(
                Device.CreatePwmPort(Device.Pins.D02),
                Device.CreatePwmPort(Device.Pins.D03),
                Device.CreatePwmPort(Device.Pins.D04)
            );

            onboardLed.SetColor(Color.Green);
        }

        void TestRgbLed()
        {
            Console.WriteLine("TestRgbLed...");

            while (true)
            {
                Console.WriteLine("Going through each color on each RGB LED...");
                for (int i = 0; i < (int)RgbLed.Colors.count; i++)
                {
                    rgbLed.SetColor((RgbLed.Colors)i);
                    Thread.Sleep(500);
                }
            }
        }

        void TestRgbPwmLed()
        {
            Console.WriteLine("TestRgbPwmLed...");

            while (true)
            {
                for (int i = 0; i < 360; i++)
                {
                    var hue = ((double)i / 360F);
                    rgbPwmLed.SetColor(Color.FromHsba(((double)i / 360F), 1, 1));
                    Console.WriteLine($"Hue {hue}");
                    Thread.Sleep(25);
                }
            }
        }
    }
}