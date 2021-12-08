using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using System;
using System.Diagnostics;
using System.Threading;

namespace MorseCodeTrainer
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        RgbPwmLed onboardLed;
        PushButton button;
        Stopwatch stopWatch;
        System.Timers.Timer timer;
        string character;

        public MeadowApp()
        {
            Initialize();
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            button = new PushButton(device: Device, Device.Pins.D02);
            button.PressStarted += ButtonPressStarted;
            button.PressEnded += ButtonPressEnded;

            stopWatch = new Stopwatch();

            timer = new System.Timers.Timer(3000);
            timer.Elapsed += TimerElapsed;

            onboardLed.SetColor(Color.Green);
        }

        private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine(character);
            character = string.Empty;
            timer.Stop();
        }

        void ButtonPressStarted(object sender, EventArgs e)
        {
            stopWatch.Reset();
            stopWatch.Start();
            timer.Stop();
        }

        void ButtonPressEnded(object sender, EventArgs e)
        {
            stopWatch.Stop();

            if (stopWatch.ElapsedMilliseconds < 200)
            {
                character += "dot ";
            }
            else
            {
                character += "dash ";
            }

            timer.Start();
        }
    }
}