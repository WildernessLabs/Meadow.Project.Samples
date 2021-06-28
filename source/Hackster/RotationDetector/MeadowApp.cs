using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Motion;
using Meadow.Units;
using System;

namespace RotationDetector
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Led up;
        Led down; 
        Led left;
        Led right;
        Mpu6050 mpu;

        public MeadowApp()
        {
            var led = new RgbLed(Device, Device.Pins.OnboardLedRed, Device.Pins.OnboardLedGreen, Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            up = new Led(Device.CreateDigitalOutputPort(Device.Pins.D15));
            down = new Led(Device.CreateDigitalOutputPort(Device.Pins.D12));
            left = new Led(Device.CreateDigitalOutputPort(Device.Pins.D14));
            right = new Led(Device.CreateDigitalOutputPort(Device.Pins.D13));

            mpu = new Mpu6050(Device.CreateI2cBus());
            mpu.Updated += Mpu_Updated; // += RotationDetected;
            mpu.StartUpdating(TimeSpan.FromMilliseconds(100));            

            led.SetColor(RgbLed.Colors.Green);
        }

        private void Mpu_Updated(object sender, IChangeResult<(Acceleration3D? Acceleration3D, AngularVelocity3D? AngularVelocity3D, Temperature? Temperature)> e)
        {
            throw new System.NotImplementedException();
        }

        private void RotationDetected(object sender, IChangeResult<(Acceleration3D? Acceleration, AngularAcceleration3D? AngularAcceleration)> e)
        {
            up.IsOn = (0.20 < e.New.Acceleration?.Y.MetersPerSecondSquared && e.New.Acceleration?.Y.MetersPerSecondSquared < 0.80);
            down.IsOn = (-0.80 < e.New.Acceleration?.Y.MetersPerSecondSquared && e.New.Acceleration?.Y.MetersPerSecondSquared < -0.20);
            left.IsOn = (0.20 < e.New.Acceleration?.X.MetersPerSecondSquared && e.New.Acceleration?.X.MetersPerSecondSquared < 0.80);
            right.IsOn = (-0.80 < e.New.Acceleration?.X.MetersPerSecondSquared && e.New.Acceleration?.X.MetersPerSecondSquared < -0.20);
        }
    }
}