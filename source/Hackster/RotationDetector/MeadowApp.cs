using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Motion;
using Meadow.Peripherals.Sensors.Motion;

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
            //mpu.AccelerationChangeThreshold = 0.05f;
            //mpu.Updated += MpuUpdated;
            //mpu.StartUpdating(100);            

            up.IsOn = true;

            led.SetColor(RgbLed.Colors.Green);
        }

        void MpuUpdated(object sender, AccelerationConditionChangeResult e)
        {
            up.IsOn = (0.20 < e.New.YAcceleration && e.New.YAcceleration < 0.80);
            down.IsOn = (-0.80 < e.New.YAcceleration && e.New.YAcceleration < -0.20);
            left.IsOn = (0.20 < e.New.XAcceleration && e.New.XAcceleration < 0.80);
            right.IsOn = (-0.80 < e.New.XAcceleration && e.New.XAcceleration < -0.20);
        }
    }
}