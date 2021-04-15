using System.Threading.Tasks;
using Xamarin.Forms;

namespace Connected.Client
{
    public partial class App : Application
    {
        public static new App Current;

        public App()
        {
            InitializeComponent();

            Current = this;

            MainPage = new MainPage();
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