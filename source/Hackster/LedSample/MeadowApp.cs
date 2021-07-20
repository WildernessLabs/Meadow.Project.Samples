using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Leds;
using System;
using System.Threading;

namespace LedSample
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Led led;
        PwmLed pwmLed;
        PiezoSpeaker speaker;

        public MeadowApp()
        {
            Console.WriteLine("Initializing...");

            led = new Led(Device.CreateDigitalOutputPort(Device.Pins.D01));
            //pwmLed = new PwmLed(Device.CreatePwmPort(Device.Pins.D02),
            //   TypicalForwardVoltage.Red);

            speaker = new PiezoSpeaker(Device.CreatePwmPort(Device.Pins.D11));
            speaker.PlayTone(261.63f, 100);


            TestPiezo();
            //TestLed();
            //TestPwmLed();
        }

        void TestPiezo() 
        {
            new Thread(() => 
            {
                led.IsOn = true;
                speaker.PlayTone(261.63f, 400);
                led.IsOn = false;
            }).Start();            
        }

        void TestLed()
        {
            Console.WriteLine("TestLeds...");

            while (true)
            {
                Console.WriteLine("Turning on and off each led for 1 second");
                for (int i = 0; i < 2; i++)
                {
                    led.IsOn = true;
                    Thread.Sleep(1000);
                    led.IsOn = false;
                    Thread.Sleep(1000);
                }

                Console.WriteLine("Blinking the LED for a bit.");
                led.StartBlink();
                Thread.Sleep(3000);
                led.Stop();
            }
        }

        void TestPwmLed()
        {
            Console.WriteLine("TestPwmLed...");

            while (true)
            {
                Console.WriteLine("Turning on and off each led for one second");
                pwmLed.IsOn = true;
                Thread.Sleep(500);
                pwmLed.IsOn = false;

                Console.WriteLine("Blinking the LED for three seconds...");
                pwmLed.StartBlink();
                Thread.Sleep(3000);
                pwmLed.Stop();

                Console.WriteLine("Pulsing the LED for three seconds...");
                pwmLed.StartPulse();
                Thread.Sleep(3000);
                pwmLed.Stop();

                Console.WriteLine("Increasing and decreasing brightness...");
                for (int j = 0; j <= 3; j++)
                {
                    for (int i = 0; i <= 10; i++)
                    {
                        pwmLed.SetBrightness(i * 0.10f);
                        Thread.Sleep(100);
                    }

                    for (int i = 10; i >= 0; i--)
                    {
                        pwmLed.SetBrightness(i * 0.10f);
                        Thread.Sleep(100);
                    }
                }
            }
        }
    }
}