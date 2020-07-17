using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Motion;

namespace RotationDetector
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Led up;
        Led down; 
        Led left;
        Led right;
        Mpu6050 mpu6050;

        public MeadowApp()
        {
            up = new Led(Device.CreateDigitalOutputPort(Device.Pins.D15));
            down = new Led(Device.CreateDigitalOutputPort(Device.Pins.D12));
            left = new Led(Device.CreateDigitalOutputPort(Device.Pins.D14));
            right = new Led(Device.CreateDigitalOutputPort(Device.Pins.D13));
            mpu6050 = new Mpu6050(Device.CreateI2cBus());

            Start();
        }

        void Start()
        {
            mpu6050.Wake();

            while (true)
            {
                //mpu6050.Refresh();
                Thread.Sleep(100);

                if (mpu6050.AccelerationY > 1000 && mpu6050.AccelerationY < 16000)
                    up.IsOn = true;
                else
                    up.IsOn = false;

                if (mpu6050.AccelerationY > 49000 && mpu6050.AccelerationY < 64535)
                    down.IsOn = true;
                else
                    down.IsOn = false;

                if (mpu6050.AccelerationX > 1000 && mpu6050.AccelerationX < 16000)
                    right.IsOn = true;
                else
                    right.IsOn = false;

                if (mpu6050.AccelerationX > 49000 && mpu6050.AccelerationX < 64535)
                    left.IsOn = true;
                else
                    left.IsOn = false;
            }
        }
    }
}