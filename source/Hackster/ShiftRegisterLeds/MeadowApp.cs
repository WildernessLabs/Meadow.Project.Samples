using Meadow;
using Meadow.Devices;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Leds;
using System.Threading;

namespace ShiftRegisterLeds
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        x74595 shiftRegister;

        public MeadowApp()
        {
            Initialize();

            TestX74595();
        }

        void Initialize() 
        {
            var led = new RgbLed(Device, Device.Pins.OnboardLedRed, Device.Pins.OnboardLedGreen, Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            shiftRegister = new x74595(
                device: Device,
                spiBus: Device.CreateSpiBus(),
                pinChipSelect: Device.Pins.D03,
                pins: 8);

            led.SetColor(RgbLed.Colors.Green);
        }

        void TestX74595()
        {
            shiftRegister.Clear(true);

            while (true)
            {
                shiftRegister.Clear();
                for(int i = 0; i < shiftRegister.Pins.AllPins.Count; i++)
                {
                    shiftRegister.WriteToPin(shiftRegister.Pins.AllPins[i], true);
                    Thread.Sleep(500);
                    shiftRegister.WriteToPin(shiftRegister.Pins.AllPins[i], false);
                }
            }
        }
    }
}