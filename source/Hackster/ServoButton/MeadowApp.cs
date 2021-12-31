using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Servos;
using Meadow.Units;
using System;
using System.Threading;
using AU = Meadow.Units.Angle.UnitType;

namespace ServoButton
{
    // public class MeadowApp : App<F7Micro, MeadowApp> <- If you have a Meadow F7 v1.*
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        Servo servo;
        PushButton button;

        public MeadowApp()
        {
            Initialize();
        }

        void Initialize() 
        {
            var onboardLed = new RgbPwmLed(
                device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            servo = new Servo(Device.CreatePwmPort(Device.Pins.D03), NamedServoConfigs.SG90);
            servo.RotateTo(NamedServoConfigs.SG90.MaximumAngle);
            Thread.Sleep(1000);
            servo.RotateTo(NamedServoConfigs.SG90.MinimumAngle);

            button = new PushButton(Device, Device.Pins.D04);
            button.Clicked += ButtonClicked;

            onboardLed.SetColor(Color.Green);
        }

        void ButtonClicked(object sender, EventArgs e)
        {
            servo.RotateTo(new Angle(75, AU.Degrees));
            Thread.Sleep(1000);
            servo.RotateTo(new Angle(0, AU.Degrees));
        }
    }
}