using MobileBle.ViewModel;
using Xamarin.Forms;

namespace MobileBle.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
            ColorWheel1.SelectedColorChanged += ColorWheel1_SelectedColorChanged;
        }

        private void ColorWheel1_SelectedColorChanged(object sender, ColorPicker.BaseClasses.ColorPickerEventArgs.ColorChangedEventArgs e)
        {
            (BindingContext as MainViewModel).SelectedColor = e.NewColor;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as MainViewModel).DiscoverDevices();
        }
    }
}
