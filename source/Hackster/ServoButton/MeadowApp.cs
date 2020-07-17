using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Servos;
using Meadow.Hardware;

namespace ServoButton
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Servo servo;
        PushButton button;

        public MeadowApp()
        {
            var servoConfig = new ServoConfig(
                minimumAngle: 0,
                maximumAngle: 180,
                minimumPulseDuration: 700,
                maximumPulseDuration: 3000,
                frequency: 50);
            servo = new Servo(Device.CreatePwmPort(Device.Pins.D03), servoConfig);
            servo.RotateTo(0);

            button = new PushButton(Device, Device.Pins.D04);
            button.Clicked += ButtonClicked;

            // Keeps the app running
            Thread.Sleep(Timeout.Infinite);
        }

        void ButtonClicked(object sender, EventArgs e)
        {
            servo.RotateTo(75);
            Thread.Sleep(1000);
            servo.RotateTo(0);
        }
    }
}
