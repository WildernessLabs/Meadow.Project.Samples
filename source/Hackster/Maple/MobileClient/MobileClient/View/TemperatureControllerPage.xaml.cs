using MobileClient.ViewModel;
using Xamarin.Forms;

namespace MobileClient.View
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

            await (BindingContext as TemperatureControllerViewModel).LoadData();
        }
    }
}