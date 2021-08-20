using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Connected.Client.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        void BtnLedClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LedControllerPage());
        }

        void BtnServoClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ServoControllerPage());
        }

        void BtnTemperatureClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TemperatureControllerPage());
        }
    }
}