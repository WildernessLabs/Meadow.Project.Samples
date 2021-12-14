using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.TftSpi;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace MorseCodeTrainer
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        readonly Color green = Color.FromHex("#16C60C");

        Dictionary<string, string> morseCode;

        RgbPwmLed onboardLed;
        PushButton btnTyper;
        PushButton btnFunction;
        GraphicsLibrary graphics;

        Timer timer;
        Stopwatch stopWatch;
        string character;
        string characterQuestion;
        string text;

        public MeadowApp()
        {
            Initialize();
        }

        void Initialize()
        {
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
            graphics.DrawText(24, 15, "Morse Code Coach");
            graphics.DrawHorizontalLine(24, 41, 196, Color.White);
            graphics.Show();

            btnTyper = new PushButton(device: Device, Device.Pins.D02);
            btnTyper.PressStarted += ButtonPressStarted;
            btnTyper.PressEnded += ButtonPressEnded;

            btnFunction = new PushButton(device: Device, Device.Pins.D03);
            btnFunction.Clicked += BtnFunctionClicked;
            btnFunction.LongClickedThreshold = TimeSpan.FromSeconds(2);
            btnFunction.LongClicked += BtnFunctionLongClicked;

            stopWatch = new Stopwatch();

            timer = new Timer(2000);
            timer.Elapsed += TimerElapsed;

            LoadMorseCode();

            ShowLetterQuestion();

            onboardLed.SetColor(Color.Green);
        }

        private void BtnFunctionLongClicked(object sender, EventArgs e)
        {
            character = string.Empty;
            UpdateInput();
        }

        private void BtnFunctionClicked(object sender, EventArgs e)
        {
            //text = text.Substring(0, text.Length - 1);
            //Console.WriteLine(text);
        }

        void LoadMorseCode() 
        {
            morseCode = new Dictionary<string, string>();            
            morseCode.Add("O-"   , "A");
            morseCode.Add("-OOO" , "B");
            morseCode.Add("-O-O" , "C");
            morseCode.Add("-OO"  , "D");
            morseCode.Add("O"    , "E");
            morseCode.Add( "OO-O", "F");
            morseCode.Add("--O"  , "G");
            morseCode.Add("OOOO" , "H");
            morseCode.Add("OO"   , "I");
            morseCode.Add("O---" , "J");
            morseCode.Add("-O-"  , "K");
            morseCode.Add("O-OO" , "L");
            morseCode.Add("--"   , "M");
            morseCode.Add("-O"   , "N");
            morseCode.Add("---"  , "O");
            morseCode.Add("O--O" , "P");
            morseCode.Add("--O-" , "Q");
            morseCode.Add("O-O"  , "R");
            morseCode.Add("OOO"  , "S");
            morseCode.Add("-"    , "T");
            morseCode.Add("OO-"  , "U");
            morseCode.Add("OOO-" , "V");
            morseCode.Add("O--"  , "W");
            morseCode.Add("-OO-" , "X");
            morseCode.Add("-O--" , "Y");
            morseCode.Add("--OO" , "Z");
            morseCode.Add("-----", "0");
            morseCode.Add("O----", "1");
            morseCode.Add("OO---", "2");
            morseCode.Add("OOO--", "3");
            morseCode.Add("OOOO-", "4");
            morseCode.Add("OOOOO", "5");
            morseCode.Add("-OOOO", "6");
            morseCode.Add("--OOO", "7");
            morseCode.Add("---OO", "8");
            morseCode.Add("----O", "9");
        }

        async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!morseCode.ContainsKey(character))
            {
                return;
            }

            timer.Stop();

            if (morseCode[character] == characterQuestion)
            {
                graphics.DrawText(120, 60, characterQuestion, green, GraphicsLibrary.ScaleFactor.X3, GraphicsLibrary.TextAlignment.Center);
                graphics.DrawText(120, 130, character, green, GraphicsLibrary.ScaleFactor.X3, GraphicsLibrary.TextAlignment.Center);
                graphics.DrawText(120, 200, "Correct!", green, GraphicsLibrary.ScaleFactor.X1, GraphicsLibrary.TextAlignment.Center);
                graphics.Show();

                await Task.Delay(2000);

                ShowLetterQuestion();
            }
            else
            {
                graphics.DrawText(120, 60, characterQuestion, Color.Red, GraphicsLibrary.ScaleFactor.X3, GraphicsLibrary.TextAlignment.Center);
                graphics.DrawText(120, 130, character, Color.Red, GraphicsLibrary.ScaleFactor.X3, GraphicsLibrary.TextAlignment.Center);
                graphics.DrawText(120, 200, "Incorrect!", Color.Red, GraphicsLibrary.ScaleFactor.X1, GraphicsLibrary.TextAlignment.Center);
            }            
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

            UpdateInput();

            timer.Start();
        }

        void UpdateInput() 
        {
            graphics.DrawRectangle(2, 130, 236, 60, Color.Black, true);
            graphics.DrawText(120, 130, character, Color.White, GraphicsLibrary.ScaleFactor.X3, GraphicsLibrary.TextAlignment.Center);
            graphics.Show();
        }

        void ShowLetterQuestion() 
        { 
            Random rand = new Random();

            character = String.Empty;

            characterQuestion = morseCode.ElementAt(rand.Next(0, morseCode.Count)).Value;
            graphics.DrawRectangle(2, 60, 236, 60, Color.Black, true);
            graphics.DrawText(120, 60, characterQuestion, Color.White, GraphicsLibrary.ScaleFactor.X3, GraphicsLibrary.TextAlignment.Center);
            graphics.DrawRectangle(2, 130, 236, 60, Color.Black, true);
            graphics.DrawRectangle(2, 200, 236, 20, Color.Black, true);            
            graphics.Show();
        }
    }
}