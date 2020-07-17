using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays.Lcd;
using Meadow.Foundation.RTCs;

namespace ChristmasCountdown
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {        
        Ds1307 rtc;
        DateTime currentDate;
        CharacterDisplay display;

        public MeadowApp()
        {
            rtc = new Ds1307(Device.CreateI2cBus());
            // Uncomment only when setting the time
            // rtc.SetTime(new DateTime(2019, 11, 23, 22, 55, 20));

            display = new CharacterDisplay
            (
                device: Device,
                pinRS: Device.Pins.D15,
                pinE: Device.Pins.D14,
                pinD4: Device.Pins.D13,
                pinD5: Device.Pins.D12,
                pinD6: Device.Pins.D11,
                pinD7: Device.Pins.D10
            );

            StartCountdown();
        }

        void StartCountdown() 
        {
            currentDate = rtc.GetTime();

            display.WriteLine("Current Date:", 0);
            display.WriteLine($"{currentDate.Month}/{currentDate.Day}/{currentDate.Year}", 1);
            display.WriteLine("Christmas Countdown:", 2);

            while (true)
            {                
                UpdateCountdown();
                Thread.Sleep(60000);
            }
        }

        void UpdateCountdown()
        {
            var date = rtc.GetTime();
            var christmasDate = new DateTime(date.Year, 12, 25);

            if (currentDate.Day != date.Day)
            {
                currentDate = date;
                display.WriteLine(currentDate.Month + "/" + currentDate.Day + "/" + currentDate.Year, 1);
            }
            
            var countdown = christmasDate.Subtract(date);
            display.WriteLine(countdown.Days + "d" + countdown.Hours + "h" + countdown.Minutes + "m to go!", 3);
        }
    }
}