using Meadow;
using Meadow.Foundation.Servos;
using Meadow.Hardware;
using Meadow.Units;
using System.Threading;
using System.Threading.Tasks;
using AU = Meadow.Units.Angle.UnitType;

namespace MeadowMapleServo.Controllers
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

        public void Initialize(IMeadowDevice device, IPin PwmPin)
        {
            if (initialized) { return; }

            servo = new Servo(device, PwmPin, NamedServoConfigs.SG90);
            servo.RotateTo(NamedServoConfigs.SG90.MinimumAngle);

            initialized = true;
        }

        public void RotateTo(Angle angle)
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