using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays.Lcd;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using System;
using System.Threading;

namespace MeadowClock
{
    // public class MeadowApp : App<F7Micro, MeadowApp> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        PushButton minute;
        PushButton hour;
        CharacterDisplay display;

        public MeadowApp()
        {
            Initialize();

            CharacterDisplayClock();
        }

        void Initialize() 
        {
            var led = new RgbLed(
                    Device,
                    Device.Pins.OnboardLedRed,
                    Device.Pins.OnboardLedGreen,
                    Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            display = new CharacterDisplay
            (
                device: Device,
                pinV0: Device.Pins.D11,
                pinRS: Device.Pins.D10,
                pinE: Device.Pins.D09,
                pinD4: Device.Pins.D08,
                pinD5: Device.Pins.D07,
                pinD6: Device.Pins.D06,
                pinD7: Device.Pins.D05
            );

            hour = new PushButton(Device, Device.Pins.D14);
            hour.Clicked += HourClicked;
            minute = new PushButton(Device, Device.Pins.D13);
            minute.Clicked += MinuteClicked;

            Device.SetClock(new DateTime(2022, 03, 05, 19, 45, 00));

            led.SetColor(RgbLed.Colors.Green);
        }

        void HourClicked(object sender, EventArgs e)
        {
            Device.SetClock(DateTime.Now.AddHours(1));
        }

        void MinuteClicked(object sender, EventArgs e)
        {
            Device.SetClock(DateTime.Now.AddMinutes(1));
        }

        void CharacterDisplayClock()
        {
            display.ClearLines();
            display.WriteLine($"Meadow F7 Micro  ", 0);
            display.WriteLine($"Onboard RTC      ", 1);
            while (true)
            {
                DateTime clock = DateTime.Now;
                display.WriteLine($"{clock:MM}/{clock:dd}/{clock:yyyy}", 2);
                display.WriteLine($"{clock:hh}:{clock:mm}:{clock:ss} {clock:tt}", 3);
                Thread.Sleep(1000);
            }
        }
    }
}