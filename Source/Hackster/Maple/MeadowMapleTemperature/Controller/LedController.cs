using Meadow.Foundation;
using Meadow.Foundation.Leds;
using System;

namespace MeadowMapleTemperature.Controller
{
    public class LedController
    {
        RgbPwmLed led;

        private static readonly Lazy<LedController> instance =
            new Lazy<LedController>(() => new LedController());
        public static LedController Instance => instance.Value;

        private LedController()
        {
            Initialize();
        }

        private void Initialize()
        {
            led = new RgbPwmLed(
                MeadowApp.Device,
                MeadowApp.Device.Pins.OnboardLedRed,
                MeadowApp.Device.Pins.OnboardLedGreen,
                MeadowApp.Device.Pins.OnboardLedBlue
            );
        }

        public void SetColor(Color color)
        {
            led.Stop();
            led.SetColor(color);
        }

        public void StartBlink(Color color)
        {
            led.Stop();
            led.StartBlink(color);
        }
    }
}
