using Meadow.Foundation;
using Meadow.Foundation.Leds;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectedLed.Meadow
{
    public class LedController
    {
        RgbPwmLed rgbPwmLed;
        Task animationTask = null;
        CancellationTokenSource cancellationTokenSource = null;

        bool initialized = false;

        public static LedController Current { get; private set; }

        private LedController() { }

        static LedController()
        {
            Current = new LedController();
        }

        void Stop() 
        {
            rgbPwmLed.Stop();
            cancellationTokenSource?.Cancel();
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
            Stop();
            rgbPwmLed.SetColor(color);            
        }

        public void TurnOn()
        {
            Stop();
            rgbPwmLed.SetColor(GetRandomColor());
            rgbPwmLed.IsOn = true;
        }

        public void TurnOff()
        {
            Stop();
            rgbPwmLed.IsOn = false;
        }

        public void StartBlink()
        {
            Stop();
            rgbPwmLed.StartBlink(GetRandomColor());
        }

        public void StartPulse()
        {
            Stop();
            rgbPwmLed.StartPulse(GetRandomColor());
        }        

        public void StartRunningColors()
        {
            rgbPwmLed.Stop();

            animationTask = new Task(async () =>
            {
                cancellationTokenSource = new CancellationTokenSource();
                await StartRunningColors(cancellationTokenSource.Token);
            });
            animationTask.Start();
        }

        protected async Task StartRunningColors(CancellationToken cancellationToken) 
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                rgbPwmLed.SetColor(GetRandomColor());
                await Task.Delay(500);                
            }
        }

        protected Color GetRandomColor()
        {
            var random = new Random();
            return Color.FromHsba(random.NextDouble(), 1, 1);
        }
    }
}