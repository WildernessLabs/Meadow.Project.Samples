using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Motors;
using Meadow.Foundation.Sensors.Rotary;
using System;
using System.Threading.Tasks;

namespace MotorRotaryController
{
    // public class MeadowApp : App<F7Micro, MeadowApp> <- If you have a Meadow F7 v1.*
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
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
                await Task.Delay(1000);

                for (float i = -1; i <= 1; i = i + 0.1f)
                {
                    Console.WriteLine($"{i}");
                    motor.Power = i;
                    await Task.Delay(1000);
                }

                await Task.Delay(2000);

                for (float i = 1; i >= -1; i = i - 0.1f)
                {
                    Console.WriteLine($"{i}");
                    motor.Power = i;
                    await Task.Delay(1000);
                }

                await Task.Delay(2000);
            }
        }

        void RotaryRotated(object sender, Meadow.Peripherals.Sensors.Rotary.RotaryChangeResult e)
        {
            if (e.New == Meadow.Peripherals.Sensors.Rotary.RotationDirection.Clockwise)
            {
                motor.Power = 0.75f;
                //number = number + SPEED > 1 ? 1.0 : number + SPEED;
                //motor.Power = motor.Power + SPEED > 1.0f ? 0.75f : motor.Power + SPEED;
            }
            else
            {
                motor.Power = -0.75f;
                //number = number - SPEED < -1 ? -1.0 : number - SPEED;
                //motor.Power = motor.Power - SPEED < -1.0f ? -0.75f : motor.Power - SPEED;
            }

            //Console.WriteLine($"{number}");
            Console.WriteLine($"{motor.Power}");
        }
    }
}