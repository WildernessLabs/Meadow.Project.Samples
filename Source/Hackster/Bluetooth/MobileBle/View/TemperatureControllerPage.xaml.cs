using MobileBle.ViewModel;

namespace MobileBle.View
{
    public partial class TemperatureControllerPage : ContentPage
    {
        public TemperatureControllerPage()
        {
            InitializeComponent();
            BindingContext = new TemperatureControllerViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as BaseViewModel).CmdSearchForDevices.Execute(null);
        }
    }
}