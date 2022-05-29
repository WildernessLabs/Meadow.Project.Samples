using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Units;
using System;

namespace LedJoystick
{
    // public class MeadowApp : App<F7Micro, MeadowApp> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2, MeadowApp>
    {
        PwmLed Up, Down, Left, Right;
        AnalogJoystick joystick;

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

            Up = new PwmLed(Device.CreatePwmPort(Device.Pins.D07, 100, 0.0f), TypicalForwardVoltage.Red);
            Down = new PwmLed(Device.CreatePwmPort(Device.Pins.D04, 100, 0.0f), TypicalForwardVoltage.Red);
            Left = new PwmLed(Device.CreatePwmPort(Device.Pins.D02, 100, 0.0f), TypicalForwardVoltage.Red);
            Right = new PwmLed(Device.CreatePwmPort(Device.Pins.D03, 100, 0.0f), TypicalForwardVoltage.Red);

            joystick = new AnalogJoystick(
                Device.CreateAnalogInputPort(Device.Pins.A01, 1, TimeSpan.FromMilliseconds(10), new Voltage(3.3)),
                Device.CreateAnalogInputPort(Device.Pins.A00, 1, TimeSpan.FromMilliseconds(10), new Voltage(3.3)),
                null);

            joystick.SetCenterPosition();
            joystick.Updated += JoystickUpdated;
            joystick.StartUpdating(TimeSpan.FromMilliseconds(100));

            onboardLed.SetColor(Color.Green);
        }

        private void JoystickUpdated(object sender, IChangeResult<Meadow.Peripherals.Sensors.Hid.AnalogJoystickPosition> e)
        {
            if (e.New.Horizontal < 0.2f)
            {
                Left.Brightness = 0f;
                Right.Brightness = 0f;
            }
            if (e.New.Vertical < 0.2f)
            {
                Up.Brightness = 0f;
                Down.Brightness = 0f;
            }

            if (e.New.Horizontal > 0)
                Left.Brightness = e.New.Horizontal.Value;
            else
                Right.Brightness = Math.Abs(e.New.Horizontal.Value);

            if (e.New.Vertical > 0)
                Down.Brightness = Math.Abs(e.New.Vertical.Value);
            else
                Up.Brightness = Math.Abs(e.New.Vertical.Value);

            Console.WriteLine($"({e.New.Horizontal.Value}, {e.New.Vertical.Value})");
        }

        //async Task TestAnalogJoystick()
        //{
        //    Console.WriteLine("TestAnalogJoystick()...");

        //    Up.StartBlink();
        //    Down.StartBlink();
        //    Left.StartBlink();
        //    Right.StartBlink();
        //    Thread.Sleep(1000);
        //    Up.Stop();
        //    Down.Stop();
        //    Left.Stop();
        //    Right.Stop();

        //    while (true)
        //    {
        //        var position = await joystick.GetPosition();
        //        switch (position)
        //        {
        //            case DigitalJoystickPosition.Up:
        //                Down.IsOn = Left.IsOn = Right.IsOn = false;
        //                Up.SetBrightness(0.1f);
        //                break;
        //            case DigitalJoystickPosition.Down:
        //                Up.IsOn = Left.IsOn = Right.IsOn = false;
        //                Down.SetBrightness(0.1f);
        //                break;
        //            case DigitalJoystickPosition.Left:
        //                Up.IsOn = Down.IsOn = Right.IsOn = false;
        //                Left.SetBrightness(0.1f);
        //                break;
        //            case DigitalJoystickPosition.Right:
        //                Up.IsOn = Down.IsOn = Left.IsOn = false;
        //                Right.SetBrightness(0.1f);
        //                break;
        //            case DigitalJoystickPosition.Center:
        //                Up.IsOn = Down.IsOn = Left.IsOn = Right.IsOn = false;
        //                break;
        //            case DigitalJoystickPosition.UpLeft:
        //                Down.IsOn = Right.IsOn = false;
        //                Up.SetBrightness(0.1f);
        //                Left.SetBrightness(0.1f);
        //                break;
        //            case DigitalJoystickPosition.UpRight:
        //                Down.IsOn = Left.IsOn = false;
        //                Up.SetBrightness(0.1f);
        //                Right.SetBrightness(0.1f);
        //                break;
        //            case DigitalJoystickPosition.DownLeft:
        //                Up.IsOn = Right.IsOn = false;
        //                Down.SetBrightness(0.1f);
        //                Left.SetBrightness(0.1f);
        //                break;
        //            case DigitalJoystickPosition.DownRight:
        //                Up.IsOn = Left.IsOn = false;
        //                Down.SetBrightness(0.1f);
        //                Right.SetBrightness(0.1f);
        //                break;
        //        }
        //        Console.WriteLine($"{position.ToString()}");

        //        Thread.Sleep(50);
        //    }
        //}
    }
}