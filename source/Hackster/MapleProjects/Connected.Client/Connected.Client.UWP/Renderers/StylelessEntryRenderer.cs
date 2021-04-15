using Connected.Client.UWP.Renderers;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Entry), typeof(StylelessEntryRenderer))]
namespace Connected.Client.UWP.Renderers
{
    public class StylelessEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderThickness = new Windows.UI.Xaml.Thickness(0);
            }
        }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (Control == null)
                    return;

                if (string.IsNullOrEmpty(Control.Text))
                    return;

                Control.Text += " ";
                Control.Text = Control.Text.Trim();
            });

            return base.GetDesiredSize(widthConstraint, heightConstraint);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }
    }
}