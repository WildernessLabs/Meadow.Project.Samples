using Meadow;
using System.Threading;

namespace ServoButton
{
    class Program
    {
        static IApp app;
        public static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "--exitOnDebug") return;
            app = new MeadowApp();
            Thread.Sleep(Timeout.Infinite);
        }
    }
}