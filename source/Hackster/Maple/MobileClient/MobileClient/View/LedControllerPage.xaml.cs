using MobileClient.ViewModel;
using Xamarin.Forms;

namespace MobileClient.View
{
    public partial class LedControllerPage : ContentPage
    {
        public LedControllerPage()
        {
            InitializeComponent();
            BindingContext = new LedControllerViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await (BindingContext as LedControllerViewModel).GetServers();
        }
    }
}