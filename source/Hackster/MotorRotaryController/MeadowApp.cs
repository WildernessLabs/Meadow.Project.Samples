using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Motors;
using Meadow.Foundation.Sensors.Rotary;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MotorRotaryController
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        float SPEED = 0.02f;
        double number = 0;

        RgbPwmLed led;
        HBridgeMotor motor;
        RotaryEncoder rotary;

        public MeadowApp()
        {
            Initialize();
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            led = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            led.SetColor(Color.Red);

            motor = new HBridgeMotor
            (
                device: Device,
                a1Pin: Device.Pins.D05,
                a2Pin: Device.Pins.D06,
                enablePin: Device.Pins.D09
            );
            motor.Power = 0f;

            rotary = new RotaryEncoder(Device, Device.Pins.D02, Device.Pins.D03);
            rotary.Rotated += RotaryRotated;

            led.SetColor(Color.Green);

            //TestMotor();
        }

        async Task TestMotor() 
        {
            while (true)
            {
                motor.Power = 0.75f;
                await Task.Delay(1000);
                motor.Power = 0.0f;
                await Task.Delay(1000);
                motor.Power = -0.75f;
                await Task.Delay(1000);
                motor.Power = 0.0f;
                await Task.Delay(1000);
            }
        }

        private void RotaryRotated(object sender, Meadow.Peripherals.Sensors.Rotary.RotaryChangeResult e)
        {
            if (e.New == Meadow.Peripherals.Sensors.Rotary.RotationDirection.Clockwise)
            {
                motor.Power += SPEED;
            }
            else
            {
                motor.Power -= SPEED;
            }

            Console.WriteLine($"{motor.Power}");
        }
    }
}
