using Meadow;
using Meadow.Devices;
using Meadow.Foundation.ICs.IOExpanders;
using System.Threading;

namespace ShiftRegisterLeds
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        x74595 shiftRegister;

        public MeadowApp()
        {
            shiftRegister = new x74595(
                device: Device,
                spiBus: Device.CreateSpiBus(), 
                pinChipSelect: Device.Pins.D03,
                pins: 8);

            shiftRegister.Clear(true);

            TestX74595();
        }

        void TestX74595()
        {
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