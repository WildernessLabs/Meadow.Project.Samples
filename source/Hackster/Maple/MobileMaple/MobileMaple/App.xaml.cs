using MobileMaple.View;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileMaple
{
    public partial class App : Application
    {
        public static new App Current;

        public App()
        {
            InitializeComponent();

            Current = this;

            MainPage = new NavigationPage(new MainPage()) 
            {
                BarTextColor = Color.White,
                BarBackgroundColor = (Color)Current.Resources["ButtonActive"]
            };
        }

        public async Task DisplayAlert(string title, string msg, string cancel)
        {
            if (MainPage != null)
            {
                await MainPage.DisplayAlert(title, msg, cancel);
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}