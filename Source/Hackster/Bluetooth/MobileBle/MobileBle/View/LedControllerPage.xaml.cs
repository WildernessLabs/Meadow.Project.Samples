using MobileBle.ViewModel;
using Xamarin.Forms;

namespace MobileBle.View
{
    public partial class LedControllerPage : ContentPage
    {
        public LedControllerPage()
        {
            InitializeComponent();
            BindingContext = new LedControllerViewModel();

            ColorWheel.SelectedColorChanged += ColorChanged;
        }

        void ColorChanged(object sender, ColorPicker.BaseClasses.ColorPickerEventArgs.ColorChangedEventArgs e)
        {
            (BindingContext as LedControllerViewModel).SelectedColor = e.NewColor;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as BaseViewModel).CmdSearchForDevices.Execute(null);
        }
    }
}