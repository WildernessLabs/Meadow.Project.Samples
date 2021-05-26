using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.TftSpi;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Moisture;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Hardware;
using System;
using Meadow.Units;
using VU = Meadow.Units.Voltage.UnitType;

namespace PlantMonitor
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        readonly Voltage MINIMUM_VOLTAGE_CALIBRATION = new Voltage(2.81, VU.Volts);
        readonly Voltage MAXIMUM_VOLTAGE_CALIBRATION = new Voltage(1.50, VU.Volts);        

        double moisture;
        Temperature temperature;

        RgbPwmLed onboardLed;
        PushButton button;
        Capacitive capacitive;        
        AnalogTemperature analogTemperature;
        DisplayController displayController;
       
        public MeadowApp()
        {
            Initialize();            
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);
            onboardLed.SetColor(Color.Red);

            button = new PushButton(Device, Device.Pins.D04, ResistorMode.InternalPullUp);
            button.Clicked += ButtonClicked;

            var config = new SpiClockConfiguration
            (
                speedKHz: 6000,
                mode: SpiClockConfiguration.Mode.Mode3
            );
            var display = new St7789
            (
                device: Device,
                spiBus: Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config),
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240
            );
            displayController = new DisplayController(display);
            
            capacitive = new Capacitive(
                device: Device,
                analogPin: Device.Pins.A01,
                minimumVoltageCalibration: MINIMUM_VOLTAGE_CALIBRATION,
                maximumVoltageCalibration: MAXIMUM_VOLTAGE_CALIBRATION);

            var capacitiveObserver = Capacitive.CreateObserver(
                handler: result =>
                {
                    onboardLed.SetColor(Color.Purple);

                    displayController.UpdateMoistureImage(result.New);
                    displayController.UpdateMoisturePercentage(result.New, result.Old.Value);

                    onboardLed.SetColor(Color.Green);
                },
                filter: null
            );
            capacitive.Subscribe(capacitiveObserver);

            capacitive.StartUpdating(
                sampleCount: 10, 
                sampleIntervalDuration: 40, 
                standbyDuration: (int)TimeSpan.FromHours(1).TotalMilliseconds);

            analogTemperature = new AnalogTemperature(Device, Device.Pins.A00, AnalogTemperature.KnownSensorType.LM35);
            var analogTemperatureObserver = AnalogTemperature.CreateObserver(
                handler =>
                {
                    onboardLed.SetColor(Color.Purple);

                    displayController.UpdateTemperatureValue(handler.New, handler.Old.Value);

                    onboardLed.SetColor(Color.Green);
                },
                filter: null
            );
            analogTemperature.Subscribe(analogTemperatureObserver);


            analogTemperature.StartUpdating(
                sampleCount: 10,
                sampleIntervalDuration: 40,
                standbyDuration: (int)TimeSpan.FromHours(1).TotalMilliseconds);

            onboardLed.SetColor(Color.Green);
        }

        async void ButtonClicked(object sender, EventArgs e)
        {
            onboardLed.SetColor(Color.Orange);

            var newMoisture = await capacitive.Read();
            var newTemperature = await analogTemperature.Read();

            displayController.UpdateMoisturePercentage(newMoisture.New, moisture);
            moisture = newMoisture.New;

            displayController.UpdateTemperatureValue(newTemperature.New, newTemperature.Old.Value);
            temperature = newTemperature.New;

            onboardLed.SetColor(Color.Green);
        }
    }
}