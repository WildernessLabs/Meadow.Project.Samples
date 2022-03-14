namespace MobileBleRover.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new MobileBleRover.App());
        }
    }
}
