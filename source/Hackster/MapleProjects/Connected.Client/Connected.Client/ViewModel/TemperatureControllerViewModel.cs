using System.Threading.Tasks;
using Xamarin.Forms;

namespace Connected.Client.ViewModel
{
    public class TemperatureControllerViewModel : BaseViewModel
    {
        public TemperatureControllerViewModel() : base()
        {
            SendCommand = new Command(async () => await GetTemperatureLogs());
        }

        public async Task GetTemperatureLogs() 
        {
        
        }
    }
}
