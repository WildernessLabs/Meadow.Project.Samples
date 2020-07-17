using Meadow.Foundation.Motors;

namespace LedCar
{
    public class CarController
    {
        float SPEED = 0.75f;

        HBridgeMotor motorLeft;
        HBridgeMotor motorRight;

        public CarController(HBridgeMotor motorLeft, HBridgeMotor motorRight)
        {
            this.motorLeft = motorLeft;
            this.motorRight = motorRight;
        }

        public void Stop()
        {
            motorLeft.Speed = 0f;
            motorRight.Speed = 0f;
        }

        public void TurnLeft()
        {
            motorLeft.Speed = SPEED;
            motorRight.Speed = -SPEED;
        }

        public void TurnRight()
        {
            motorLeft.Speed = -SPEED;
            motorRight.Speed = SPEED;
        }

        public void MoveForward()
        {
            motorLeft.Speed = -SPEED;
            motorRight.Speed = -SPEED;
        }

        public void MoveBackward()
        {
            motorLeft.Speed = SPEED;
            motorRight.Speed = SPEED;
        }
    }
}
