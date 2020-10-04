using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays.Led;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using System;
using System.Diagnostics;
using System.Threading;

namespace StopwatchDisplay
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        bool isRunning;
        Stopwatch stopwatch;
        PushButton reset;
        PushButton startStop;
        FourDigitSevenSegment display;

        public MeadowApp()
        {
            Console.Write("Initialize...");

            var led = new RgbLed(Device, Device.Pins.OnboardLedRed, Device.Pins.OnboardLedGreen, Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            stopwatch = new Stopwatch();

            startStop = new PushButton(Device, Device.Pins.D12, Meadow.Hardware.ResistorMode.PullUp);
            startStop.Clicked += StartStopClicked;

            reset = new PushButton(Device, Device.Pins.D13, Meadow.Hardware.ResistorMode.PullUp);
            reset.Clicked += ResetClicked;

            display = new FourDigitSevenSegment
            (
                portDigit1: Device.CreateDigitalOutputPort(Device.Pins.D00),
                portDigit2: Device.CreateDigitalOutputPort(Device.Pins.D03),
                portDigit3: Device.CreateDigitalOutputPort(Device.Pins.D04),
                portDigit4: Device.CreateDigitalOutputPort(Device.Pins.D06),
                portA: Device.CreateDigitalOutputPort(Device.Pins.D01),
                portB: Device.CreateDigitalOutputPort(Device.Pins.D05),
                portC: Device.CreateDigitalOutputPort(Device.Pins.D08),
                portD: Device.CreateDigitalOutputPort(Device.Pins.D10),
                portE: Device.CreateDigitalOutputPort(Device.Pins.D11),
                portF: Device.CreateDigitalOutputPort(Device.Pins.D02),
                portG: Device.CreateDigitalOutputPort(Device.Pins.D07),
                portDecimal: Device.CreateDigitalOutputPort(Device.Pins.D09),
                isCommonCathode: true
            );
            display.SetDisplay("0000".ToCharArray());

            led.SetColor(RgbLed.Colors.Green);

            while (true)
            {
                string time = stopwatch.Elapsed.Minutes.ToString("D2") + stopwatch.Elapsed.Seconds.ToString("D2");
                display.SetDisplay(time.ToCharArray());
                Thread.Sleep(1000);
            }
        }

        void StartStopClicked(object sender, EventArgs e)
        {
            if (isRunning)
            {
                Console.WriteLine("stop");
                stopwatch.Stop();
            }
            else
            {
                Console.WriteLine("start");
                stopwatch.Start();
            }
            isRunning = !isRunning;
        }

        void ResetClicked(object sender, EventArgs e)
        {
            display.SetDisplay("0000".ToCharArray());
            stopwatch.Reset();
        }
    }
}