using System.Threading;
using Meadow;

namespace TempHumidityMonitorWithTftDisplay
{
    class Program
    {
        static IApp app;
        public static void Main(string[] args)
        {
            // instantiate and run new meadow app
            app = new TempHumidityMonitorWithTftDisplay();

            Thread.Sleep(-1);
        }
    }
}
