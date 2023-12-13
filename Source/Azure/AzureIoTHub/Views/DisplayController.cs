using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.Buffers;
using Meadow.Units;
using SimpleJpegDecoder;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MeadowAzureIoTHub.Views
{
    public class DisplayController
    {
        private static readonly Lazy<DisplayController> instance =
            new Lazy<DisplayController>(() => new DisplayController());
        public static DisplayController Instance => instance.Value;

        static Color backgroundColor = Color.FromHex("#23ABE3");
        static Color foregroundColor = Color.Black;

        CancellationTokenSource token;

        protected BufferRgb888 imgConnecting, imgConnected, imgRefreshing, imgRefreshed;
        protected MicroGraphics graphics;

        private DisplayController() { }

        public void Initialize(IGraphicsDisplay display)
        {
            imgConnected = LoadJpeg("img_wifi_connected.jpg");
            imgConnecting = LoadJpeg("img_wifi_connecting.jpg");
            imgRefreshing = LoadJpeg("img_refreshing.jpg");
            imgRefreshed = LoadJpeg("img_refreshed.jpg");

            graphics = new MicroGraphics(display)
            {
                CurrentFont = new Font8x12(),
                Stroke = 3,
                Rotation = RotationType._90Degrees
            };

            graphics.Clear(true);
        }

        BufferRgb888 LoadJpeg(string fileName)
        {
            var jpgData = LoadResource(fileName);
            var decoder = new JpegDecoder();
            decoder.DecodeJpeg(jpgData);
            return new BufferRgb888(decoder.Width, decoder.Height, decoder.GetImageData());
        }

        protected void DrawBackground()
        {
            var logo = LoadJpeg("img_meadow.jpg");

            graphics.Clear(backgroundColor);

            graphics.DrawBuffer(
                x: graphics.Width / 2 - logo.Width / 2,
                y: 63,
                buffer: logo);

            graphics.DrawText(graphics.Width / 2, 160, "Azure IoT Hub", foregroundColor, ScaleFactor.X2, HorizontalAlignment.Center);
        }

        protected byte[] LoadResource(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"MeadowAzureIoTHub.{filename}";

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
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

                graphics.DrawBuffer(204, 6, alternateImg ? imgConnecting : imgConnected);
                graphics.Show();

                await Task.Delay(500);
            }
        }

        public void ShowConnected()
        {
            token.Cancel();
            graphics.DrawBuffer(204, 6, imgConnected);

            graphics.DrawBuffer(6, 6, imgRefreshed);

            graphics.DrawRectangle(0, 32, 240, 208, backgroundColor, true);

            graphics.DrawRoundedRectangle(19, 62, 200, 70, 15, foregroundColor);
            graphics.DrawText(120, 70, "Temperature", foregroundColor, ScaleFactor.X2, HorizontalAlignment.Center);

            graphics.DrawRoundedRectangle(19, 151, 200, 70, 15, foregroundColor);
            graphics.DrawText(120, 159, "Humidity", foregroundColor, ScaleFactor.X2, HorizontalAlignment.Center);

            graphics.Show();
        }

        public async Task StartSyncCompletedAnimation((Temperature? Temperature, RelativeHumidity? Humidity) reading)
        {
            graphics.DrawBuffer(6, 6, imgRefreshing);
            graphics.Show();
            await Task.Delay(TimeSpan.FromSeconds(1));

            graphics.DrawRectangle(23, 100, 192, 24, backgroundColor, true);
            graphics.DrawText(120, 100, $"{reading.Temperature.Value.Celsius:N1}°C", foregroundColor, ScaleFactor.X2, HorizontalAlignment.Center);

            graphics.DrawRectangle(23, 189, 192, 24, backgroundColor, true);
            graphics.DrawText(120, 189, $"{reading.Humidity.Value.Percent:N2}%", foregroundColor, ScaleFactor.X2, HorizontalAlignment.Center);

            graphics.DrawBuffer(6, 6, imgRefreshed);
            graphics.Show();
        }
    }
}