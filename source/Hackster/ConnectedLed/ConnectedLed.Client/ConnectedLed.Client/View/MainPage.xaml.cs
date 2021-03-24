using ConnectedLed.Client.ViewModel;
using Xamarin.Forms;

namespace ConnectedLed.Client
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