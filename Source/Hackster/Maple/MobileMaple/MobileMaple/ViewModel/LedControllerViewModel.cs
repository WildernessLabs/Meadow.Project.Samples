using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileMaple.ViewModel
{
    public class LedControllerViewModel : BaseViewModel
    {
        bool _isOn;
        public bool IsOn 
        {
            get => _isOn;
            set { _isOn = value; OnPropertyChanged(nameof(IsOn)); }
        }

        bool _isOff;
        public bool IsOff
        {
            get => _isOff;
            set { _isOff = value; OnPropertyChanged(nameof(IsOff)); }
        }

        bool _isPulsing;
        public bool IsPulsing
        {
            get => _isPulsing;
            set { _isPulsing = value; OnPropertyChanged(nameof(IsPulsing)); }
        }

        bool _isBlinking;
        public bool IsBlinking
        {
            get => _isBlinking;
            set { _isBlinking = value; OnPropertyChanged(nameof(IsBlinking)); }
        }

        bool _isRunningColors;
        public bool IsRunningColors
        {
            get => _isRunningColors;
            set { _isRunningColors = value; OnPropertyChanged(nameof(IsRunningColors)); }
        }

        public LedControllerViewModel() : base()
        {
            SendCommand = new Command(async (obj) => await SendLedCommand(obj as string));

            IsOn = true;
        }

        async Task SendLedCommand(string command)
        {
            if (IsBusy || SelectedServer == null)
                return;
            IsBusy = true;

            try
            {
                bool response = await client.PostAsync(SelectedServer.IpAddress, ServerPort, command, string.Empty);

                if (response)
                {
                    IsOn = IsOff = IsBlinking = IsPulsing = IsRunningColors = false;

                    switch (command) 
                    {
                        case "TurnOn": IsOn = true; break;
                        case "TurnOff": IsOff = true; break;
                        case "StartBlink": IsBlinking = true; break;
                        case "StartPulse": IsPulsing = true; break;
                        case "StartRunningColors": IsRunningColors = true; break;
                    }
                }
                else
                {
                    await App.Current.DisplayAlert("Error", "Request failed.", "Close");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}