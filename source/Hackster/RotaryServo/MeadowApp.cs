using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Rotary;
using Meadow.Foundation.Servos;

namespace RotaryServo
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        int angle = 0;
        Servo servo;
        RgbPwmLed onboardLed;
        RotaryEncoder rotaryEncoder;

        public MeadowApp()
        {
            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);
            onboardLed.SetColor(Color.Red);

            servo = new Servo(Device.CreatePwmPort(Device.Pins.D08), NamedServoConfigs.SG90);
            servo.RotateTo(0);

            rotaryEncoder = new RotaryEncoder(Device, Device.Pins.D02, Device.Pins.D03);
            rotaryEncoder.Rotated += (s, e) =>
            {
                if (e.Direction == Meadow.Peripherals.Sensors.Rotary.RotationDirection.Clockwise)
                    angle++;
                else
                    angle--;

                if (angle > 180) angle = 180;
                else if (angle < 0) angle = 0;

                servo.RotateTo(angle);
            };

            onboardLed.SetColor(Color.Green);
        }
    }
}