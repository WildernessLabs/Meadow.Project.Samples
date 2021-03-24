using ConnectedLed.Client.UWP.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Button), typeof(StylelessButtonRenderer))]
namespace ConnectedLed.Client.UWP.Renderers
{
    public class StylelessButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Resources["ButtonBackgroundPointerOver"] = Control.BackgroundColor;
                Resources["ButtonBackgroundPressed"] = Control.BackgroundColor;
            }
        }
    }
}