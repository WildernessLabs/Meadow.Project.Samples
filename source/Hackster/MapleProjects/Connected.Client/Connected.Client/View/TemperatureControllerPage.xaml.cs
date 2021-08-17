using Connected.Client.ViewModel;
using Xamarin.Forms;

namespace Connected.Client.View
{
    public partial class TemperatureControllerPage : ContentPage
    {
        public TemperatureControllerPage()
        {
            InitializeComponent();
            BindingContext = new TemperatureControllerViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await (BindingContext as TemperatureControllerViewModel).GetServers();
        }
    }
}