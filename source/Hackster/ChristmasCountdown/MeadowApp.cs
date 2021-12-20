using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays.Lcd;
using Meadow.Foundation.Leds;
using Meadow.Foundation;

namespace ChristmasCountdown
{
    // public class MeadowApp : App<F7Micro, MeadowApp> <- If you have a Meadow F7 v1.*
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {        
        CharacterDisplay display;

        public MeadowApp()
        {
            Initialize();

            ShowSplashScreen();

            Device.WiFiAdapter.WiFiConnected += WiFiConnected;
        }

        void Initialize() 
        {
            var led = new RgbPwmLed(Device, 
                Device.Pins.OnboardLedRed, 
                Device.Pins.OnboardLedGreen, 
                Device.Pins.OnboardLedBlue);
            led.SetColor(Color.Red);

            display = new CharacterDisplay
            (
                device: Device,
                pinRS:  Device.Pins.D10,
                pinE:   Device.Pins.D09,
                pinD4:  Device.Pins.D08,
                pinD5:  Device.Pins.D07,
                pinD6:  Device.Pins.D06,
                pinD7:  Device.Pins.D05,
                rows: 4, columns: 20
            );

            led.SetColor(Color.Green);
        }

        void ShowSplashScreen() 
        {
            display.WriteLine("====================", 0);
            display.WriteLine("Christmas Countdown!", 1);
            display.WriteLine("=   Joining WIFI   =", 2);
            display.WriteLine("====================", 3);
        }

        void WiFiConnected(object sender, EventArgs e)
        {
            display.WriteLine($"{DateTime.Now.ToString("MMMM dd, yyyy")}", 0);
            display.WriteLine("Christmas Countdown:", 2);

            while (true)
            {
                UpdateCountdown();
                Thread.Sleep(1000);
            }
        }

        void UpdateCountdown()
        {
            // Adjust Time Zone (Pacific Time)
            var today = DateTime.Now.AddHours(-8);

            display.WriteLine($"{today.ToString("MMMM dd, yyyy")}", 0);
            display.WriteLine($"{today.ToString("hh:mm:ss tt")}", 1);
            
            var christmasDate = new DateTime(today.Year, 12, 25);
            var countdown = christmasDate.Subtract(today);
            display.WriteLine($"  {countdown.Days.ToString("D3")}d {countdown.Hours.ToString("D2")}h {countdown.Minutes.ToString("D2")}m {countdown.Seconds.ToString("D2")}s", 3);            
        }
    }
}