using System.Threading;
using Meadow;

namespace LightSensor
{
    class Program
    {
        static IApp app;
        public static void Main(string[] args)
        {
            app = new MeadowApp();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
