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
            Initialize();

            mpu.StartUpdating(TimeSpan.FromMilliseconds(100));
        }

        void Initialize() 
        {
            var led = new RgbLed(Device, Device.Pins.OnboardLedRed, Device.Pins.OnboardLedGreen, Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            up = new Led(Device.CreateDigitalOutputPort(Device.Pins.D13));
            down = new Led(Device.CreateDigitalOutputPort(Device.Pins.D10));
            left = new Led(Device.CreateDigitalOutputPort(Device.Pins.D11));
            right = new Led(Device.CreateDigitalOutputPort(Device.Pins.D12));

            mpu = new Mpu6050(Device.CreateI2cBus());
            mpu.Updated += MpuUpdated;            

            led.SetColor(RgbLed.Colors.Green);
        }

        void MpuUpdated(object sender, IChangeResult<(Acceleration3D? Acceleration3D, AngularVelocity3D? AngularVelocity3D, Temperature? Temperature)> e)
        {
            up.IsOn = e.New.Acceleration3D?.Y.CentimetersPerSecondSquared < -50;
            down.IsOn = e.New.Acceleration3D?.Y.CentimetersPerSecondSquared > 100;
            left.IsOn = e.New.Acceleration3D?.X.CentimetersPerSecondSquared > 50;
            right.IsOn = e.New.Acceleration3D?.X.CentimetersPerSecondSquared < -100;
        }
    }
}