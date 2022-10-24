using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LedDice
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        PwmLed[] leds;
        PushButton button;

        public override Task Initialize() 
        {
            var onboardLed = new RgbPwmLed(
                device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            leds = new PwmLed[7];
            leds[0] = new PwmLed(Device, Device.Pins.D06, TypicalForwardVoltage.Red);  // 
            leds[1] = new PwmLed(Device, Device.Pins.D07, TypicalForwardVoltage.Red);  // [6]       [5]
            leds[2] = new PwmLed(Device, Device.Pins.D08, TypicalForwardVoltage.Red);  // 
            leds[3] = new PwmLed(Device, Device.Pins.D09, TypicalForwardVoltage.Red);  // [4]  [3]  [2]
            leds[4] = new PwmLed(Device, Device.Pins.D10, TypicalForwardVoltage.Red);  // 
            leds[5] = new PwmLed(Device, Device.Pins.D11, TypicalForwardVoltage.Red);  // [1]       [0]
            leds[6] = new PwmLed(Device, Device.Pins.D12, TypicalForwardVoltage.Red);  // 

            button = new PushButton(Device, Device.Pins.D05);
            button.Clicked += ButtonClicked;

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        void ButtonClicked(object sender, EventArgs e)
        {
            Random random = new Random();

            ShuffleAnimation();
            ShowNumber(random.Next(1,7));
        }

        void ShuffleAnimation() 
        {
            foreach (var led in leds)
            {
                led.StartBlink();
            }
            Thread.Sleep(1000);

            foreach (var led in leds)
            {
                led.Stop();
            }
            Thread.Sleep(100);
        }

        void ShowNumber(int number)
        {
            leds[0].IsOn = (number == 6 || number == 5 || number == 4);
            leds[1].IsOn = (number == 6 || number == 5 || number == 4 || number == 3 || number == 2);
            leds[2].IsOn = (number == 6);
            leds[3].IsOn = (number == 4 || number == 5 || number == 3 || number == 1);
            leds[4].IsOn = (number == 6);
            leds[5].IsOn = (number == 6 || number == 5 || number == 4 || number == 3 || number == 2);
            leds[6].IsOn = (number == 6 || number == 5 || number == 4);
        }

        public override Task Run()
        {
            ShuffleAnimation();

            return base.Run();
        }
    }
}