using BleRover.Client.ViewModel;
using Xamarin.Forms;

namespace BleRover.Client
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }
}