using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.TextDisplayMenu;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MeadowMenu
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {        
        Menu menu;
        St7789 st7789;
        RgbPwmLed onboardLed;       
        GraphicsLibrary graphics;        
        PushButton next, previous, select;

        public MeadowApp()
        {
            Initialize();
        }

        void Initialize()
        {
            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);

            onboardLed.SetColor(Color.Red);

            var config = new SpiClockConfiguration(
                speedKHz: 12000, 
                mode: SpiClockConfiguration.Mode.Mode3);
            var spiBus = Device.CreateSpiBus(
                clock: Device.Pins.SCK, 
                mosi: Device.Pins.MOSI, 
                miso: Device.Pins.MISO, 
                config: config);
            st7789 = new St7789
            (
                device: Device,
                spiBus: spiBus,
                chipSelectPin: null,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240
            );

            graphics = new GraphicsLibrary(st7789)
            {
                CurrentFont = new Font12x20(),
            };
            graphics.Clear();

            var menuData = LoadFromJson("menu.json");
            //var menuData = LoadFromCode();
            menu = new Menu(graphics, menuData, false);

            next = new PushButton(Device, Device.Pins.D03, ResistorMode.InternalPullUp);
            next.Clicked += (s, e) => { menu.Next(); };

            select = new PushButton(Device, Device.Pins.D04, ResistorMode.InternalPullUp);
            select.Clicked += (s, e) => { menu.Select(); };

            previous = new PushButton(Device, Device.Pins.D02, ResistorMode.InternalPullUp);
            previous.Clicked += (s, e) => { menu.Previous(); };

            onboardLed.SetColor(Color.Green);

            menu.Enable();
        }

        byte[] LoadFromJson(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"MeadowMenu.{filename}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        MenuItem[] LoadFromCode() 
        {
            var subMenuItems2 = new List<MenuItem>();
            subMenuItems2.Add(new MenuItem(
                text: "Sub Item C"));
            subMenuItems2.Add(new MenuItem(
                text: "Sub Item D"));
            subMenuItems2.Add(new MenuItem(
                text: "Sub Item E"));

            var subMenuItems1 = new List<MenuItem>();
            subMenuItems1.Add(new MenuItem(
                text: "Sub Item A"));
            subMenuItems1.Add(new MenuItem(
                text: "Sub Item B"));
            subMenuItems1.Add(new MenuItem(
                text: "Submenu 2", 
                subItems: subMenuItems2.ToArray()));

            var menuItems = new List<MenuItem>();
            menuItems.Add(new MenuItem(
                text: "Value 1: {value}",
                id: "displayValue1",
                value: 77));
            menuItems.Add(new MenuItem(
                text: "Edit Value 1",
                id: "myValue1",
                type: "Value",
                value: 77));
            menuItems.Add(new MenuItem(
                text: "Value 2: {value}",
                id: "displayValue1",
                value: 25));
            menuItems.Add(new MenuItem(
                text: "Edit Value 2",
                id: "myValue2",
                type: "Value",
                value: 25));
            menuItems.Add(new MenuItem(
                text: "submenu 1",
                subItems: subMenuItems1.ToArray()));

            return menuItems.ToArray();
        }
    }
}