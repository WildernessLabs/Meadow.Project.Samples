using Meadow.Foundation;
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

        protected BufferRgb888 imgConnecting, imgConnected, imgRefreshing;
        protected MicroGraphics graphics;

        private DisplayController() { }

        public void Initialize(IGraphicsDisplay display)
        {
            imgConnected = LoadJpeg("img_wifi_connected.jpg");
            imgConnecting = LoadJpeg("img_wifi_connecting.jpg");
            imgRefreshing = LoadJpeg("img_refresh.jpg");

            graphics = new MicroGraphics(display)
            {
                CurrentFont = new Font12x16(),
                Stroke = 3,
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

            graphics.DrawText(graphics.Width / 2, 160, "Azure IoT Hub", Color.Black, alignmentH: HorizontalAlignment.Center);
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
            graphics.DrawBuffer(
                x: 204,
                y: 6,
                buffer: imgConnected);

            graphics.DrawBuffer(
                x: 6,
                y: 6,
                buffer: imgRefreshing);

            graphics.DrawRectangle(0, 32, 240, 208, backgroundColor, true);

            graphics.DrawCircle(120, 75, 50, foregroundColor);
            graphics.DrawText(120, 59, "Temp", foregroundColor, alignmentH: HorizontalAlignment.Center);

            graphics.DrawCircle(178, 177, 50, foregroundColor);
            graphics.DrawText(178, 161, "Hum", foregroundColor, alignmentH: HorizontalAlignment.Center);

            graphics.Show();
        }

        public async Task StartSyncCompletedAnimation((Temperature? Temperature, RelativeHumidity? Humidity) reading)
        {
            graphics.DrawBuffer(6, 6, imgRefreshing);
            graphics.Show();
            await Task.Delay(TimeSpan.FromSeconds(1));

            graphics.DrawRectangle(75, 78, 90, 16, backgroundColor, true);
            graphics.DrawText(120, 78, $"{reading.Temperature.Value.Celsius:N1}°C", foregroundColor, alignmentH: HorizontalAlignment.Center);

            graphics.DrawRectangle(133, 180, 90, 16, backgroundColor, true);
            graphics.DrawText(178, 180, $"{reading.Humidity.Value.Percent:N2}%", foregroundColor, alignmentH: HorizontalAlignment.Center);

            graphics.DrawRectangle(6, 6, 26, 26, backgroundColor, true);
            graphics.Show();
        }
    }
}