using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Rotary;
using Meadow.Foundation.Servos;

namespace RotaryServo
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        int angle = 0;
        Servo servo;
        RotaryEncoder rotaryEncoder;

        public MeadowApp()
        {
            var led = new RgbLed(Device, Device.Pins.OnboardLedRed, Device.Pins.OnboardLedGreen, Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

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

            led.SetColor(RgbLed.Colors.Green);
        }
    }
}