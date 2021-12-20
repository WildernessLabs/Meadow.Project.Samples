using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Peripherals.Sensors.Hid;
using System;

namespace LedJoystick
{
    // public class MeadowApp : App<F7Micro, MeadowApp> <- If you have a Meadow F7 v1.*
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        PwmLed Up, Down, Left, Right;
        AnalogJoystick joystick;

        public MeadowApp()
        {
            var led = new RgbLed(Device, Device.Pins.OnboardLedRed, Device.Pins.OnboardLedGreen, Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            Up = new PwmLed(Device.CreatePwmPort(Device.Pins.D07, 100, 0.0f), TypicalForwardVoltage.Red);
            Down = new PwmLed(Device.CreatePwmPort(Device.Pins.D04, 100, 0.0f), TypicalForwardVoltage.Red);
            Left = new PwmLed(Device.CreatePwmPort(Device.Pins.D02, 100, 0.0f), TypicalForwardVoltage.Red);
            Right = new PwmLed(Device.CreatePwmPort(Device.Pins.D03, 100, 0.0f), TypicalForwardVoltage.Red);

            joystick = new AnalogJoystick(
                Device.CreateAnalogInputPort(Device.Pins.A01),
                Device.CreateAnalogInputPort(Device.Pins.A00),
                null, true);

            joystick.SetCenterPosition();
            joystick.Updated += Joystick_Updated;
            joystick.StartUpdating(TimeSpan.FromMilliseconds(100));
        }

        private void Joystick_Updated(object sender, IChangeResult<JoystickPosition> e)
        {
            if (e.New.Horizontal < 0.2f)
            {
                Left.SetBrightness(0f);
                Right.SetBrightness(0f);
            }
            if (e.New.Vertical < 0.2f)
            {
                Up.SetBrightness(0f);
                Down.SetBrightness(0f);
            }

            if (e.New.Horizontal > 0)
                Left.SetBrightness(e.New.Horizontal.Value);
            else
                Right.SetBrightness(Math.Abs(e.New.Horizontal.Value));

            if (e.New.Vertical > 0)
                Down.SetBrightness(Math.Abs(e.New.Vertical.Value));
            else
                Up.SetBrightness(Math.Abs(e.New.Vertical.Value));

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