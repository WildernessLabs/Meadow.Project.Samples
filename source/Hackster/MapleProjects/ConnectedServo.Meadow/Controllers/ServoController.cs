using Meadow.Foundation.Servos;
using Meadow.Hardware;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectedServo.Meadow.Controllers
{
    public class ServoController
    {
        Servo servo;
        Task animationTask = null;
        CancellationTokenSource cancellationTokenSource = null;

        protected int _rotationAngle;

        protected bool initialized = false;

        public static ServoController Current { get; private set; }

        private ServoController() { }

        static ServoController()
        {
            Current = new ServoController();
        }

        public void Initialize(IIODevice device, IPin PwmPin)
        {
            if (initialized) { return; }

            Console.WriteLine("Initialize hardware...");
            servo = new Servo(device, PwmPin, NamedServoConfigs.SG90);
            servo.RotateTo(0);

            initialized = true;

            Console.WriteLine("Initialization complete.");
        }

        public void RotateTo(int angle) 
        {
            servo.RotateTo(angle);
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
                    servo.RotateTo(_rotationAngle);
                    await Task.Delay(15);
                }

                while (_rotationAngle > 0)
                {
                    if (cancellationToken.IsCancellationRequested) { break; }

                    _rotationAngle--;
                    servo.RotateTo(_rotationAngle);
                    await Task.Delay(15);
                }
            }
        }
    }
}