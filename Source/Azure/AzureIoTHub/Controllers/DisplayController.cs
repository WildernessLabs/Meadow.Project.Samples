using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Units;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeadowAzureIoTHub.Controllers
{
    public class DisplayController
    {
        static Color backgroundColor = Color.FromHex("#23ABE3");
        static Color foregroundColor = Color.Black;

        CancellationTokenSource token;

        Image imgConnecting = Image.LoadFromResource("MeadowAzureIoTHub.Resources.img_wifi_connected.bmp");
        Image imgConnected = Image.LoadFromResource("MeadowAzureIoTHub.Resources.img_wifi_connecting.bmp");
        Image imgRefreshing = Image.LoadFromResource("MeadowAzureIoTHub.Resources.img_refreshing.bmp");
        Image imgRefreshed = Image.LoadFromResource("MeadowAzureIoTHub.Resources.img_refreshed.bmp");

        MicroGraphics graphics;

        DisplayScreen displayScreen;

        AbsoluteLayout SplashLayout;

        AbsoluteLayout DataLayout;

        public DisplayController(IGraphicsDisplay display)
        {
            graphics = new MicroGraphics(display)
            {
                CurrentFont = new Font8x12(),
                Stroke = 3,
                Rotation = RotationType._90Degrees
            };

            displayScreen = new DisplayScreen(display);
            {
                backgroundColor = backgroundColor;
            }

            graphics.Clear(true);
        }

        protected void DrawBackground()
        {
            var logo = Image.LoadFromResource("MeadowAzureIoTHub.Resources.img_meadow.bmp");

            graphics.Clear(backgroundColor);

            graphics.DrawImage(
                x: graphics.Width / 2 - logo.Width / 2,
                y: 63,
                image: logo);

            graphics.DrawText(graphics.Width / 2, 160, "Azure IoT Hub", foregroundColor, ScaleFactor.X2, HorizontalAlignment.Center);
        }

        public void ShowSplashScreen()
        {
            DrawBackground();

            graphics.Show();
        }

        public async Task ShowConnectingAnimation()
        {
            token = new CancellationTokenSource();

            bool alternateImg = false;
            while (!token.IsCancellationRequested)
            {
                alternateImg = !alternateImg;

                graphics.DrawImage(204, 6, alternateImg ? imgConnecting : imgConnected);
                graphics.Show();

                await Task.Delay(500);
            }
        }

        public void ShowConnected()
        {
            token.Cancel();
            graphics.DrawImage(204, 6, imgConnected);

            graphics.DrawImage(6, 6, imgRefreshed);

            graphics.DrawRectangle(0, 32, 240, 208, backgroundColor, true);

            graphics.DrawRoundedRectangle(19, 47, 200, 80, 15, foregroundColor);
            graphics.DrawText(120, 56, "Temperature", foregroundColor, ScaleFactor.X2, HorizontalAlignment.Center);

            graphics.DrawRoundedRectangle(19, 141, 200, 80, 15, foregroundColor);
            graphics.DrawText(120, 149, "Humidity", foregroundColor, ScaleFactor.X2, HorizontalAlignment.Center);

            graphics.Show();
        }

        public async Task StartSyncCompletedAnimation((Temperature? Temperature, RelativeHumidity? Humidity) reading)
        {
            graphics.DrawImage(6, 6, imgRefreshing);
            graphics.Show();
            await Task.Delay(TimeSpan.FromSeconds(1));

            graphics.CurrentFont = new Font12x20();

            graphics.DrawRectangle(24, 85, 190, 40, backgroundColor, true);
            graphics.DrawText(120, 85, $"{reading.Temperature.Value.Celsius:N1}°C", foregroundColor, ScaleFactor.X2, HorizontalAlignment.Center);

            graphics.DrawRectangle(24, 178, 190, 40, backgroundColor, true);
            graphics.DrawText(120, 178, $"{reading.Humidity.Value.Percent:N2}%", foregroundColor, ScaleFactor.X2, HorizontalAlignment.Center);

            graphics.DrawImage(6, 6, imgRefreshed);
            graphics.Show();
        }
    }
}