using Android.Content;
using Connected.Client.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(StylelessEntryPicker))]
namespace Connected.Client.Droid.Renderers
{
    class StylelessEntryPicker : EntryRenderer
    {
        public StylelessEntryPicker(Context context) : base(context) { }

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