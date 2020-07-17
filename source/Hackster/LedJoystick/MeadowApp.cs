using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Peripherals.Sensors.Hid;
using System;

namespace LedJoystick
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        PwmLed Up, Down, Left, Right;
        AnalogJoystick joystick;

        public MeadowApp()
        {
            Console.WriteLine("Initializing...");

            Up = new PwmLed(Device.CreatePwmPort(Device.Pins.D07, 100, 0.0f), TypicalForwardVoltage.Red);
            Down = new PwmLed(Device.CreatePwmPort(Device.Pins.D04, 100, 0.0f), TypicalForwardVoltage.Red);
            Left = new PwmLed(Device.CreatePwmPort(Device.Pins.D02, 100, 0.0f), TypicalForwardVoltage.Red);
            Right = new PwmLed(Device.CreatePwmPort(Device.Pins.D03, 100, 0.0f), TypicalForwardVoltage.Red);

            joystick = new AnalogJoystick(
                Device.CreateAnalogInputPort(Device.Pins.A01),
                Device.CreateAnalogInputPort(Device.Pins.A00),
                null, true);

            joystick.SetCenterPosition();
            joystick.Updated += JoystickUpdated;
            joystick.StartUpdating();

            //TestAnalogJoystick();
        }

        private void JoystickUpdated(object sender, JoystickPositionChangeResult e)
        {
            if (e.New.HorizontalValue < 0.2f)
            {
                Left.SetBrightness(0f);
                Right.SetBrightness(0f);
            }
            if (e.New.VerticalValue < 0.2f)
            {
                Up.SetBrightness(0f);
                Down.SetBrightness(0f);
            }

            if (e.New.HorizontalValue > 0)
                Left.SetBrightness(Math.Abs(e.New.HorizontalValue));
            else
                Right.SetBrightness(Math.Abs(e.New.HorizontalValue));

            if (e.New.VerticalValue > 0)
                Down.SetBrightness(Math.Abs(e.New.VerticalValue));
            else
                Up.SetBrightness(Math.Abs(e.New.VerticalValue));

            Console.WriteLine($"({e.New.HorizontalValue}, {e.New.VerticalValue})");
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