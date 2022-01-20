﻿using Meadow.Foundation.Servos;
using Meadow.Units;
using System;
using System.Threading;
using System.Threading.Tasks;
using AU = Meadow.Units.Angle.UnitType;

namespace MeadowBleServo.Controllers
{
    public class ServoController
    {
        Servo servo;
        Task animationTask = null;
        CancellationTokenSource cancellationTokenSource = null;

        private static readonly Lazy<ServoController> instance =
            new Lazy<ServoController>(() => new ServoController());
        public static ServoController Instance => instance.Value;

        protected int _rotationAngle;

        private ServoController() 
        {
            Initialize();
        }

        public void Initialize()
        {
            servo = new Servo(MeadowApp.Device, MeadowApp.Device.Pins.D10, NamedServoConfigs.SG90);
            servo.RotateTo(NamedServoConfigs.SG90.MinimumAngle);
        }

        public void RotateTo(int angle)
        {
            servo.RotateTo(new Angle(angle));
        }

        public void StopSweep()
        {
            cancellationTokenSource?.Cancel();
        }

        public void StartSweep()
        {
            animationTask = new Task(async () =>
            {
                cancellationTokenSource = new CancellationTokenSource();
                await StartSweep(cancellationTokenSource.Token);
            });
            animationTask.Start();
        }
        protected async Task StartSweep(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested) { break; }

                while (_rotationAngle < 180)
                {
                    if (cancellationToken.IsCancellationRequested) { break; }

                    _rotationAngle++;
                    servo.RotateTo(new Angle(_rotationAngle, AU.Degrees));
                    await Task.Delay(50);
                }

                while (_rotationAngle > 0)
                {
                    if (cancellationToken.IsCancellationRequested) { break; }

                    _rotationAngle--;
                    servo.RotateTo(new Angle(_rotationAngle, AU.Degrees));
                    await Task.Delay(50);
                }
            }
        }
    }
}