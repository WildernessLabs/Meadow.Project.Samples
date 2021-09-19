namespace MobileClient.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new MobileClient.App());
        }
    }
}