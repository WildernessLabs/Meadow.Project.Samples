using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.TftSpi;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using System;
using System.Diagnostics;

namespace MorseCodeTrainer
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        RgbPwmLed onboardLed;

        PushButton button;
        Stopwatch stopWatch;
        GraphicsLibrary graphics;

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

            var config = new SpiClockConfiguration(12000, SpiClockConfiguration.Mode.Mode3);
            var display = new St7789
            (
                device: Device,
                spiBus: Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config),
                chipSelectPin: null,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240,
                displayColorMode: ColorType.Format16bppRgb565
            );
            graphics = new GraphicsLibrary(display)
            {
                Stroke = 1,
                CurrentFont = new Font12x20(),
                Rotation = RotationType._270Degrees
            };
            graphics.Clear();
            graphics.DrawRectangle(0, 0, 240, 240);
            graphics.DrawText(5, 5, "Morse Code Trainer");
            graphics.Show();

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
                character += "O";
            }
            else
            {
                character += "-";
            }

            timer.Start();
        }
    }

    public class MorseCodeConstants 
    {
        public const string A = "O-";
        public const string B = "-OOO";
        public const string C = "-O-O";
        public const string D = "-OO";
        public const string E = "O";
        public const string F = "OO-O";
        public const string G = "--O";
        public const string H = "OOOO";
        public const string I = "OO";
        public const string J = "O---";
        public const string K = "-O-";
        public const string L = "O-OO";
        public const string M = "--";
        public const string N = "-O";
        public const string O = "---";
        public const string P = "O--O";
        public const string Q = "--O-";
        public const string R = "O-O";
        public const string S = "OOO";
        public const string T = "-";
        public const string U = "OO-";
        public const string V = "OOO-";
        public const string W = "O--";
        public const string X = "-OO-";
        public const string Y = "-O--";
        public const string Z = "--OO";
        public const string _0 = "-----";
        public const string _1 = "O----";
        public const string _2 = "OO---";
        public const string _3 = "OOO--";
        public const string _4 = "OOOO-";
        public const string _5 = "OOOOO";
        public const string _6 = "-OOOO";
        public const string _7 = "--OOO";
        public const string _8 = "---OO";
        public const string _9 = "----O";
    }
}