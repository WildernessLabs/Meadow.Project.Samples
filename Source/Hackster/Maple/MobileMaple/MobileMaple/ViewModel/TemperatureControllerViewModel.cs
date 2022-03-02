using MobileMaple.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileMaple.ViewModel
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
            var response = await client.GetAsync(SelectedServer != null? SelectedServer.IpAddress : IpAddress, ServerPort, "gettemperaturelogs", null, null);

            var values = JsonConvert.DeserializeObject<List<TemperatureModel>>(response);

            if (values != null)
            {
                foreach (var value in values)
                {
                    TemperatureLog.Add(new TemperatureModel()
                    {
                        Temperature = value.Temperature,
                        DateTime = value.DateTime
                    });
                }
            }
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
}