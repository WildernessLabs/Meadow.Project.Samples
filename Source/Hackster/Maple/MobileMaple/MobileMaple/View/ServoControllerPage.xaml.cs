using MobileMaple.ViewModel;
using Xamarin.Forms;

namespace MobileMaple.View
{
    public partial class ServoControllerPage : ContentPage
    {
        public ServoControllerPage()
        {
            InitializeComponent();
            BindingContext = new ServoControllerViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await (BindingContext as ServoControllerViewModel).GetServers();
        }
    }
}