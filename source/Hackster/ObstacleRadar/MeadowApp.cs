using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Distance;
using Meadow.Foundation.Servos;
using Meadow.Hardware;
using System;
using System.Threading;

namespace ObstacleRadar
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        GraphicsLibrary graphics;
        St7789 display;
        Vl53l0x sensor;
        Servo servo;

        float[] radarData = new float[181];

        public MeadowApp()
        {
            Initialize();

            Draw();
        }

        void Initialize()
        {
            var led = new RgbLed(
                Device, 
                Device.Pins.OnboardLedRed, 
                Device.Pins.OnboardLedGreen, 
                Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            var config = new SpiClockConfiguration(24000, SpiClockConfiguration.Mode.Mode3);
            var spiBus = Device.CreateSpiBus(
                Device.Pins.SCK, 
                Device.Pins.MOSI, 
                Device.Pins.MISO, config);

            display = new St7789(device: Device, spiBus: spiBus,
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240);

            graphics = new GraphicsLibrary(display);
            graphics.CurrentFont = new Font12x20();
            graphics.Rotation = GraphicsLibrary.RotationType._270Degrees;

            var i2cBus = Device.CreateI2cBus(I2cBusSpeed.FastPlus);
            sensor = new Vl53l0x(Device, i2cBus);
            sensor.StartUpdating(200);

            servo = new Servo(Device.CreatePwmPort(Device.Pins.D05), NamedServoConfigs.SG90);
            servo.RotateTo(0);

            led.SetColor(RgbLed.Colors.Green);
        }

        void Draw()
        {
            int angle = 160;
            int increment = 4;
            int x, y = 0;

            while (true)
            {
                graphics.Clear();

                DrawRadar();

                graphics.DrawLine(120, 170, 105, (float)(angle * Math.PI / 180), Color.Yellow);

                if (angle >= 180) { increment = -4; }
                if (angle <= 0) { increment = 4; }

                angle += increment;
                servo.RotateTo(angle);

                graphics.DrawText(0, 0, $"{180 - angle}°", Color.Yellow);

                if (sensor?.Conditions?.Distance != null && sensor?.Conditions?.Distance.Value >= 0)
                {
                    graphics.DrawText(170, 0, $"{sensor.Conditions.Distance.Value}mm", Color.Yellow);
                    radarData[angle] = sensor.Conditions.Distance.Value / 2;
                }
                else
                {
                    radarData[angle] = 0;
                }

                for (int i = 0; i < 180; i++)
                {
                    x = 120 + (int)(radarData[i] * MathF.Cos(i * MathF.PI / 180f));
                    y = 170 - (int)(radarData[i] * MathF.Sin(i * MathF.PI / 180f));
                    graphics.DrawCircle(x, y, 2, Color.Yellow, true);
                }

                graphics.Show();
                Thread.Sleep(100);
            }
        }

        void DrawRadar()
        {
            int xCenter = 120;
            int yCenter = 170;

            var radarColor = Color.LawnGreen;

            for (int i = 1; i < 5; i++)
            {
                graphics.DrawCircleQuadrant(xCenter, yCenter, 25 * i, 0, radarColor);
                graphics.DrawCircleQuadrant(xCenter, yCenter, 25 * i, 1, radarColor);
            }

            for (int i = 0; i < 7; i++)
            {
                graphics.DrawLine(xCenter, yCenter, 105, (float)(i * Math.PI / 6), radarColor);
            }
        }
    }
}
