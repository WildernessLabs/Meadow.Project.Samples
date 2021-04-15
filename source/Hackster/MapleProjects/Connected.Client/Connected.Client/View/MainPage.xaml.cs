using Connected.Client.ViewModel;
using Xamarin.Forms;

namespace Connected.Client
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await (BindingContext as MainViewModel).GetServers();
        }
    }
}