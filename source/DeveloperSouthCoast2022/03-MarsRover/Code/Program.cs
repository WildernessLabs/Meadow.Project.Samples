using System.Threading;
using Meadow;

namespace MarsRover
{
    class Program
    {
        static IApp marsRover;
        public static void Main(string[] args)
        {
            // instantiate and run new meadow app
            marsRover = new Rover();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
