using Connected.Client.UWP.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Picker), typeof(StylelessPickerRenderer))]
namespace Connected.Client.UWP.Renderers
{
    public class StylelessPickerRenderer : PickerRenderer
    {
        public static void Init() { }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderThickness = new Windows.UI.Xaml.Thickness(0);
                Control.Margin = new Windows.UI.Xaml.Thickness(0);
                Control.Padding = new Windows.UI.Xaml.Thickness(0);
            }
        }
    }
}