using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Moisture;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Hardware;
using Meadow.Peripherals.Sensors.Atmospheric;
using System;

namespace PlantMonitor
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        const float MINIMUM_VOLTAGE_CALIBRATION = 2.81f;
        const float MAXIMUM_VOLTAGE_CALIBRATION = 1.50f;

        float moisture;
        AtmosphericConditions temperature;

        RgbPwmLed onboardLed;
        PushButton button;
        Capacitive capacitive;        
        AnalogTemperature analogTemperature;
        DisplayController displayController;
       
        public MeadowApp()
        {
            Initialize();

            

            analogTemperature.Subscribe(new FilterableChangeObserver<AtmosphericConditionChangeResult, AtmosphericConditions>(
                handler => 
                {
                    onboardLed.SetColor(Color.Purple);

                    displayController.UpdateTemperatureValue(handler.New.Temperature.Value, handler.Old.Temperature.Value);

                    onboardLed.SetColor(Color.Green);
                },
                filter => 
                {
                    return (Math.Abs(filter.Delta.Temperature.Value) > 1f);
                }
            ));
            analogTemperature.StartUpdating();            
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
            capacitive.Subscribe(new FilterableChangeObserver<FloatChangeResult, float>(
                handler =>
                {
                    onboardLed.SetColor(Color.Purple);

                    displayController.UpdateMoistureImage(handler);
                    displayController.UpdateMoisturePercentage(handler.New, handler.Old);

                    onboardLed.SetColor(Color.Green);
                },
                filter =>
                {
                    return (Math.Abs(filter.Delta) > 0.05);
                }
            ));
            capacitive.StartUpdating();

            analogTemperature = new AnalogTemperature(Device, Device.Pins.A00, AnalogTemperature.KnownSensorType.LM35);

            onboardLed.SetColor(Color.Green);
        }

        async void ButtonClicked(object sender, EventArgs e)
        {
            onboardLed.SetColor(Color.Orange);

            float newMoisture = await capacitive.Read();
            var newTemperature = await analogTemperature.Read();

            displayController.UpdateMoisturePercentage(newMoisture, moisture);
            moisture = newMoisture;

            displayController.UpdateTemperatureValue(newTemperature.Temperature.Value, temperature.Temperature.Value);
            temperature = newTemperature;

            onboardLed.SetColor(Color.Green);
        }
    }
}