using Acr.UserDialogs;
using System;
using Xamarin.Forms;

namespace MobileBleRover.Extension
{
    public static class IUserDialogsExtensions
    {
        public static IDisposable ErrorToast(this IUserDialogs dialogs, string title, string message, TimeSpan duration)
        {
            return dialogs.Toast(new ToastConfig(message) { BackgroundColor = Color.Red, Duration = duration });
        }
    }
}