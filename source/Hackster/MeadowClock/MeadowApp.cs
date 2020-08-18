using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Displays.Lcd;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using System;
using System.Threading;

namespace MeadowClock
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        PushButton minute;
        PushButton hour;
        CharacterDisplay display;

        //Max7219 display;
        //GraphicsLibrary graphics;

        public MeadowApp()
        {
            Console.Write("Initializing...");

            var led = new RgbLed(Device, Device.Pins.OnboardLedRed, Device.Pins.OnboardLedGreen, Device.Pins.OnboardLedBlue);
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

            //display = new Max7219(Device, Device.CreateSpiBus(), Device.Pins.D01, 4, Max7219.Max7219Type.Display);
            //graphics = new GraphicsLibrary(display);
            //graphics.CurrentFont = new Font4x8();
            //graphics.Rotation = GraphicsLibrary.RotationType._180Degrees;

            hour = new PushButton(Device, Device.Pins.D15);
            hour.Clicked += HourClicked;            
            minute = new PushButton(Device, Device.Pins.D12);
            minute.Clicked += MinuteClicked;

            Device.SetClock(new DateTime(2020, 03, 31, 00, 45, 00));

            led.SetColor(RgbLed.Colors.Green);
            Console.WriteLine("done");

            CharacterDisplayClock();
            //RunClock();
        }

        void HourClicked(object sender, EventArgs e)
        {
            Console.WriteLine("hour up");
            Device.SetClock(DateTime.Now.AddHours(1));
        }

        void MinuteClicked(object sender, EventArgs e)
        {
            Console.WriteLine("minute up");
            Device.SetClock(DateTime.Now.AddMinutes(1));
        }

        void CharacterDisplayClock()
        {
            display.ClearLines();
            display.WriteLine($"Meadow RTC is now", 0);
            display.WriteLine($"available in b3.9", 1);
            while (true)
            {
                DateTime clock = DateTime.Now;
                display.WriteLine($"{clock:MM}/{clock:dd}/{clock:yyyy}", 2);
                display.WriteLine($"{clock:hh}:{clock:mm}:{clock:ss} {clock:tt}", 3);
                Thread.Sleep(1000);
            }
        }

        //void RunClock()
        //{
        //    while (true)
        //    {
        //        DateTime clock = DateTime.Now;
        //        graphics.Clear();
        //        graphics.DrawText(0, 1, $"{clock:hh}");
        //        graphics.DrawText(0, 9, $"{clock:mm}");
        //        graphics.DrawText(0, 17, $"{clock:ss}");
        //        graphics.DrawText(0, 25, $"{clock:tt}");
        //        graphics.Show();
        //        Thread.Sleep(1000);
        //    }
        //}
    }
}
