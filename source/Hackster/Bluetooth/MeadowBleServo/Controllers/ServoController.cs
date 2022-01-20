using Meadow.Foundation.Servos;
using Meadow.Units;
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

        protected int _rotationAngle;

        protected bool initialized = false;

        public static ServoController Current { get; private set; }

        private ServoController() { }

        static ServoController()
        {
            Current = new ServoController();
        }

        public void Initialize()
        {
            if (initialized) { return; }

            servo = new Servo(MeadowApp.Device, MeadowApp.Device.Pins.D10, NamedServoConfigs.SG90);
            servo.RotateTo(NamedServoConfigs.SG90.MinimumAngle);

            initialized = true;
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
