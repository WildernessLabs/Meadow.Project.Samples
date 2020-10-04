using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Motion;

namespace MotionDetector
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {        
        RgbPwmLed onboardLed;
        ParallaxPir motionSensor;

        public MeadowApp()
        {
            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);
            onboardLed.SetColor(Color.Red);

            motionSensor = new ParallaxPir(Device, Device.Pins.D08, InterruptMode.EdgeFalling, ResistorMode.Disabled, 5, 0);
            motionSensor.OnMotionStart += MotionSensorMotionStart;
            motionSensor.OnMotionEnd += MotionSensorMotionEnd;

            onboardLed.SetColor(Color.Green);
        }

        private void MotionSensorMotionEnd(object sender)
        {
            Console.WriteLine("End");
            onboardLed.SetColor(Color.Cyan);
        }

        private void MotionSensorMotionStart(object sender)
        {
            Console.WriteLine("Start");
            onboardLed.SetColor(Color.Magenta);
            Thread.Sleep(1000);
        }
    }
}
