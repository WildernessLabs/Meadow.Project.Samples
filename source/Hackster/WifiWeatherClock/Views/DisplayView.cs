using Meadow.Foundation.Displays.Lcd;
using System;
using WifiWeatherClock.ViewModels;

namespace WifiWeatherClock.Views
{
    public class DisplayView
    {
        CharacterDisplay display;
        
        public DisplayView()
        {
            Initialize();
            
            display.WriteLine($" WiFI Weather Clock ", 1);
            display.WriteLine($"     Loading...     ", 2);
        }

        void Initialize()
        {
            display = new CharacterDisplay
            (
                device: MeadowApp.Device,
                pinV0: MeadowApp.Device.Pins.D11,
                pinRS: MeadowApp.Device.Pins.D10,
                pinE: MeadowApp.Device.Pins.D09,
                pinD4: MeadowApp.Device.Pins.D08,
                pinD5: MeadowApp.Device.Pins.D07,
                pinD6: MeadowApp.Device.Pins.D06,
                pinD7: MeadowApp.Device.Pins.D05,
                rows: 4, columns: 20
            );
        }

        public void UpdateDisplay(WeatherViewModel model)
        {
            display.WriteLine($"{DateTime.Now.ToString("MMMM dd, yyyy")}", 0);
            display.WriteLine($"{DateTime.Now.ToString("hh:mm:ss tt")}", 1);
            display.WriteLine($"In: {model.IndoorTemperature}C | Out: {model.OutdoorTemperature}C", 2);
            display.WriteLine($"{model.Weather}", 3);
        }

        public void WriteLine(string text, byte lineNumber) 
        {
            display.WriteLine($"{text}", lineNumber);
        }
    }
}
