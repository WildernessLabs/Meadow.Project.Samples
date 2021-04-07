using Meadow.Foundation;
using Meadow.Foundation.Leds;
using System;

namespace ConnectedLed.Meadow
{
    public class LedController
    {
        RgbPwmLed rgbPwmLed;

        bool initialized = false;

        public static LedController Current { get; private set; }

        private LedController() { }

        static LedController()
        {
            Current = new LedController();
        }

        public void Initialize()
        {
            if (initialized) { return; }

            Console.WriteLine("Initialize hardware...");
            rgbPwmLed = new RgbPwmLed(
                device: MeadowApp.Device,
                redPwmPin: MeadowApp.Device.Pins.OnboardLedRed,
                greenPwmPin: MeadowApp.Device.Pins.OnboardLedGreen,
                bluePwmPin: MeadowApp.Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f);
            rgbPwmLed.SetColor(Color.Red);

            initialized = true;

            Console.WriteLine("Initialization complete.");
        }

        public void SetColor(Color color) 
        {
            rgbPwmLed.Stop();
            rgbPwmLed.SetColor(color);            
        }

        public void TurnOn()
        {
            rgbPwmLed.Stop();
            rgbPwmLed.SetColor(GetRandomColor());
            rgbPwmLed.IsOn = true;
        }

        public void TurnOff()
        {
            rgbPwmLed.Stop();
            rgbPwmLed.IsOn = false;
        }

        public void StartBlink()
        {
            rgbPwmLed.Stop();
            rgbPwmLed.StartBlink(GetRandomColor());
        }

        public void StartPulse()
        {
            rgbPwmLed.Stop();
            rgbPwmLed.StartPulse(GetRandomColor());
        }

        public void StartRunningColors()
        {
            //var arrayColors = new ArrayList();
            //for (int i = 0; i < 360; i = i + 5)
            //{
            //    var hue = ((double)i / 360F);
            //    arrayColors.Add(Color.FromHsba(((double)i / 360F), 1, 1));
            //}

            //int[] intervals = new int[arrayColors.Count];
            //for (int i = 0; i < intervals.Length; i++)
            //{
            //    intervals[i] = 100;
            //}

            //_rgbPwmLed.Stop();
            //_rgbPwmLed.StartRunningColors(arrayColors, intervals);
        }

        public void NetworkConnected()
        {
            rgbPwmLed.Stop();
            rgbPwmLed.SetColor(Color.Green);
        }

        protected Color GetRandomColor()
        {
            var random = new Random();
            return Color.FromHsba(random.NextDouble(), 1, 1);
        }
    }
}