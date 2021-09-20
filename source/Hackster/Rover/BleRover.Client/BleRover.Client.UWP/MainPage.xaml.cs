namespace BleRover.Client.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new BleRover.Client.App());
        }
    }
}
