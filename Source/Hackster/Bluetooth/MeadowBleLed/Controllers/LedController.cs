using Meadow.Foundation;
using Meadow.Foundation.Leds;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeadowBleLed.Controller
{
    public class LedController
    {
        RgbPwmLed rgbPwmLed;
        Task animationTask = null;
        CancellationTokenSource cancellationTokenSource = null;

        protected bool initialized = false;

        public static LedController Current { get; private set; }

        private LedController() { }

        static LedController()
        {
            Current = new LedController();
        }

        public void Initialize()
        {
            if (initialized) { return; }

            rgbPwmLed = new RgbPwmLed(
                device: MeadowApp.Device,
                redPwmPin: MeadowApp.Device.Pins.D12,
                greenPwmPin: MeadowApp.Device.Pins.D11,
                bluePwmPin: MeadowApp.Device.Pins.D10);
            rgbPwmLed.SetColor(Color.Red);

            initialized = true;
        }

        void Stop()
        {
            rgbPwmLed.Stop();
            cancellationTokenSource?.Cancel();
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
                await Task.Delay(1000);
            }
        }

        protected Color GetRandomColor()
        {
            var random = new Random();
            return Color.FromHsba(random.NextDouble(), 1, 1);
        }
    }
}