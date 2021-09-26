using Android.Content;
using MobileMaple.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(StylelessEntry))]
namespace MobileMaple.Droid.Renderers
{
    class StylelessEntry : EntryRenderer
    {
        public StylelessEntry(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Background = null;

                var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
                layoutParams.SetMargins(0, 0, 0, 0);
                LayoutParameters = layoutParams;
                Control.LayoutParameters = layoutParams;
                Control.SetPadding(0, 0, 0, 0);
                Control.SetBackgroundColor(Color.Transparent.ToAndroid());
                SetPadding(0, 0, 0, 0);
            }
        }
    }
}