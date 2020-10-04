using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Servos;
using System;
using System.Threading;

namespace ServoButton
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Servo servo;
        PushButton button;

        public MeadowApp()
        {
            var led = new RgbLed(Device, Device.Pins.OnboardLedRed, Device.Pins.OnboardLedGreen, Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            servo = new Servo(Device.CreatePwmPort(Device.Pins.D03), NamedServoConfigs.SG90);
            servo.RotateTo(0);
            Thread.Sleep(1000);
            servo.RotateTo(180);

            button = new PushButton(Device, Device.Pins.D04);
            button.Clicked += ButtonClicked;

            led.SetColor(RgbLed.Colors.Green);
        }

        void ButtonClicked(object sender, EventArgs e)
        {
            servo.RotateTo(75);
            Thread.Sleep(1000);
            servo.RotateTo(0);
        }
    }
}