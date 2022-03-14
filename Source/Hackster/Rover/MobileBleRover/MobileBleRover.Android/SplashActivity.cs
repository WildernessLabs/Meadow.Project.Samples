using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace MobileBleRover.Droid
{
    [Activity(Label = "Rover", Icon = "@mipmap/icon_launcher", RoundIcon = "@mipmap/icon_round_launcher", Theme = "@style/Theme.Splash",
        MainLauncher = true, NoHistory = true, ConfigurationChanges = ConfigChanges.ScreenSize, ScreenOrientation = ScreenOrientation.Landscape)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            if (!IsTaskRoot
                && Intent.HasCategory(Intent.CategoryLauncher)
                && !string.IsNullOrEmpty(Intent.Action)
                && Intent.Action == Intent.ActionMain)
            {
                Finish();
                return;
            }

            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }
    }
}