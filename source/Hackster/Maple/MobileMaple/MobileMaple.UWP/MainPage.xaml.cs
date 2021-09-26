namespace MobileMaple.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new MobileMaple.App());
        }
    }
}