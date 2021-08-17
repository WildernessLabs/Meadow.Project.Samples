using Meadow.Devices;
using Meadow.Foundation.Servos;
using Meadow.Hardware;
using Meadow.Units;
using System.Threading;
using System.Threading.Tasks;
using AU = Meadow.Units.Angle.UnitType;

namespace ConnectedServo.Meadow.Controllers
{
    public class ServoController
    {
        Servo servo;
        Task animationTask = null;
        CancellationTokenSource cancellationTokenSource = null;

        protected Angle _rotationAngle;

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
            servo.RotateTo(new Angle(0, AU.Degrees));

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

                while (_rotationAngle < new Angle(180, AU.Degrees))
                {
                    if (cancellationToken.IsCancellationRequested) { break; }

                    _rotationAngle += new Angle(1, AU.Degrees);
                    servo.RotateTo(_rotationAngle);
                    await Task.Delay(50);
                }

                while (_rotationAngle > new Angle(0, AU.Degrees))
                {
                    if (cancellationToken.IsCancellationRequested) { break; }

                    _rotationAngle -= new Angle(1, AU.Degrees);
                    servo.RotateTo(_rotationAngle);
                    await Task.Delay(50);
                }
            }
        }
    }
}