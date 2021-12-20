using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Audio.Radio;
using Meadow.Foundation.Displays.Ssd130x;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using System;
using System.Collections.Generic;
using System.Threading;

namespace RadioPlayer
{
    // public class MeadowApp : App<F7Micro, MeadowApp> <- If you have a Meadow F7 v1.*
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        List<float> stations;
        int currentStation = 0;

        Tea5767 radio;
        MicroGraphics graphics;
        PushButton btnNext;
        PushButton btnPrevious;

        public MeadowApp()
        {
            Initialize();

            stations = new List<float>();
            stations.Add(94.5f);
            stations.Add(95.3f);
            stations.Add(96.9f);
            stations.Add(102.7f);
            stations.Add(103.5f);
            stations.Add(104.3f);
            stations.Add(105.7f);

            DisplayText("Radio Player");
            Thread.Sleep(1000);
            radio.SelectFrequency(stations[currentStation]);
            DisplayText($"<- FM {stations[currentStation]} ->");
        }

        void Initialize()
        {
            var onboardLed = new RgbPwmLed(
                device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            var i2CBus = Device.CreateI2cBus();

            radio = new Tea5767(i2CBus);

            var display = new Ssd1306(i2CBus, 60, Ssd1306.DisplayType.OLED128x32);
            graphics = new MicroGraphics(display);
            graphics.Rotation = RotationType._180Degrees;

            btnNext = new PushButton(Device, Device.Pins.D03);
            btnNext.Clicked += BtnNextClicked;

            btnPrevious = new PushButton(Device, Device.Pins.D04);
            btnPrevious.Clicked += BtnPreviousClicked;

            onboardLed.SetColor(Color.Green);
        }

        void BtnNextClicked(object sender, EventArgs e)
        {
            if (currentStation < stations.Count-1)
            {
                DisplayText("      >>>>      ", 0);
                currentStation++;
                radio.SelectFrequency(stations[currentStation]);
                DisplayText($"<- FM {stations[currentStation]} ->");
            }
        }

        void BtnPreviousClicked(object sender, EventArgs e)
        {
            if (currentStation > 0)
            {
                DisplayText("      <<<<      ", 0);
                currentStation--;
                radio.SelectFrequency(stations[currentStation]);
                DisplayText($"<- FM {stations[currentStation]} ->");
            }
        }

        void DisplayText(string text, int x = 12)
        {
            graphics.Clear();
            graphics.CurrentFont = new Font8x12();
            graphics.DrawRectangle(0, 0, 128, 32);
            graphics.DrawText(x, 12, text);
            graphics.Show();
        }
    }
}