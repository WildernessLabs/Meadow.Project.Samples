using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Connected.Client.ViewModel
{
    public class TemperatureControllerViewModel : BaseViewModel
    {
        public ObservableCollection<TemperatureModel> TemperatureLog { get; set; }

        bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set { isRefreshing = value; OnPropertyChanged(nameof(IsRefreshing)); }
        }

        public ICommand CmdReloadTemperatureLog { get; private set; }

        public TemperatureControllerViewModel() : base()
        {
            TemperatureLog = new ObservableCollection<TemperatureModel>();

            SendCommand = new Command(async () => await GetTemperatureLogs());

            CmdReloadTemperatureLog = new Command(async () =>
            {
                TemperatureLog.Clear();

                await GetTemperatureLogs();

                IsRefreshing = false;
            });
        }

        public async Task GetTemperatureLogs()
        {
            await Task.Delay(500);

            TemperatureLog.Add(new TemperatureModel() { DateTime = "2021-08-01 10:23:10 PM", Temperature = 22.3f });
            TemperatureLog.Add(new TemperatureModel() { DateTime = "2021-08-05 10:23:05 PM", Temperature = 20.7f });
            TemperatureLog.Add(new TemperatureModel() { DateTime = "2021-08-07 10:23:23 PM", Temperature = 18.5f });
            TemperatureLog.Add(new TemperatureModel() { DateTime = "2021-08-12 10:23:45 PM", Temperature = 25.9f });
            TemperatureLog.Add(new TemperatureModel() { DateTime = "2021-08-15 10:23:35 PM", Temperature = 27.1f });
        }

        public async Task LoadData() 
        {
            await GetServers();

            if (SelectedServer != null)
            {
                await GetTemperatureLogs();
            }
        }
    }

    public class TemperatureModel
    {
        public string DateTime { get; set; }
        public float Temperature { get; set; }
    }
}
