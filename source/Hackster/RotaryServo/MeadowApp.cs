using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Rotary;
using Meadow.Foundation.Servos;
using Meadow.Units;
using AU = Meadow.Units.Angle.UnitType;

namespace RotaryServo
{
    // public class MeadowApp : App<F7Micro, MeadowApp> <- If you have a Meadow F7 v1.*
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        Angle angle = new Angle(0, AU.Degrees);
        Servo servo;
        RotaryEncoder rotaryEncoder;

        public MeadowApp()
        {
            var led = new RgbLed(Device, Device.Pins.OnboardLedRed, Device.Pins.OnboardLedGreen, Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            servo = new Servo(Device.CreatePwmPort(Device.Pins.D08), NamedServoConfigs.SG90);
            servo.RotateTo(new Angle(0, AU.Degrees));

            rotaryEncoder = new RotaryEncoder(Device, Device.Pins.D02, Device.Pins.D03);
            rotaryEncoder.Rotated += (s, e) =>
            {
                if (e.New == Meadow.Peripherals.Sensors.Rotary.RotationDirection.Clockwise)
                {
                    angle += new Angle(1, AU.Degrees);
                }
                else
                {
                    angle -= new Angle(1, AU.Degrees);
                }

                if (angle > new Angle(180, AU.Degrees)) angle = new Angle(180, AU.Degrees);
                else if (angle < new Angle(0, AU.Degrees)) angle = new Angle(0, AU.Degrees);

                servo.RotateTo(angle);
            };

            led.SetColor(RgbLed.Colors.Green);
        }
    }
}