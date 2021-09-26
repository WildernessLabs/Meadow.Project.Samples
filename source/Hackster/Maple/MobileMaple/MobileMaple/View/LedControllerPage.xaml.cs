using MobileMaple.ViewModel;
using Xamarin.Forms;

namespace MobileMaple.View
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