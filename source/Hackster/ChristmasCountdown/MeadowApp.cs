using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays.Lcd;
using Meadow.Foundation.Leds;
using Meadow.Foundation;

namespace ChristmasCountdown
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {        
        CharacterDisplay display;

        public MeadowApp()
        {
            Device.WiFiAdapter.WiFiConnected += WiFiConnected;
        }

        void WiFiConnected(object sender, EventArgs e)
        {
            Device.SetClock(DateTime.Now.AddHours(-8));

            Initialize();

            StartCountdown();
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

        void StartCountdown() 
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
            display.WriteLine($"{DateTime.Now.ToString("MMMM dd, yyyy")}", 0);
            display.WriteLine($"{DateTime.Now.ToString("hh:mm:ss tt")}", 1);

            var christmasDate = new DateTime(DateTime.Now.Year, 12, 25);
            var countdown = christmasDate.Subtract(DateTime.Now);
            display.WriteLine($"  {countdown.Days}d {countdown.Hours}h {countdown.Minutes}m {countdown.Seconds}s!", 3);            
        }
    }
}