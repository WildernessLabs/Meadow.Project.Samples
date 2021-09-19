using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Gateways.Bluetooth;

namespace PwmRgbBLE
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        RgbPwmLed onboardLed;

        Definition bleTreeDefinition;
        CharacteristicBool isOnCharacteristic;
        CharacteristicInt32 colorCharacteristic;

        readonly string IsOnUUID = "24517ccc888e4ffc9da521884353b08d";
        readonly string ColorUUID = "5a0bb01669ab4a49a2f2de5b292458f3";

        public MeadowApp()
        {
            Initialize();

            PulseColor(Color.Orange);
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);

            InitializeBLE();

            Console.WriteLine("Hardware initialized.");
        }

        void InitializeBLE()
        { 
            // initializing the coprocessor for BLE
            Device.InitCoprocessor();

            // initialize the bluetooth defnition tree
            Console.WriteLine("Starting the BLE server.");
            bleTreeDefinition = GetDefinition();
            Device.BluetoothAdapter.StartBluetoothServer(bleTreeDefinition);

            isOnCharacteristic.ValueSet += IsOnCharacteristic_ValueSet; ;
            colorCharacteristic.ValueSet += ColorCharacteristic_ValueSet;
        }

        private void IsOnCharacteristic_ValueSet(ICharacteristic c, object data)
        {

            Console.WriteLine($"{ c.Name }: {data}");

            if ((Int32)data > 0)
            {
                onboardLed.IsOn = false;
                isOnCharacteristic.SetValue(true);
            }
            else
            {
                onboardLed.IsOn = true;
                isOnCharacteristic.SetValue(false);
            }
        }

        private void ColorCharacteristic_ValueSet(ICharacteristic c, object data)
        {
            int color = (Int32)data;

            Console.WriteLine($"Color value: {color.ToString("X4")}");

            byte r = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte b = (byte)(color);

            var newColor = new Color(r / 255.0, g / 255.0, b / 255.0);
            PulseColor(newColor);

            colorCharacteristic.SetValue(color);
        }

        void PulseColor(Color color)
        {
            onboardLed.Stop();
            onboardLed.StartPulse(color);
        }

        protected Definition GetDefinition()
        {
            string name = "On_Off";
            string uuid = Guid.NewGuid().ToString();

            isOnCharacteristic = new CharacteristicBool(
                    name: name,
                    uuid: IsOnUUID,
                    permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                    properties: CharacteristicProperty.Read | CharacteristicProperty.Write);

            colorCharacteristic = new CharacteristicInt32(
                    name: "CurrentColor",
                    uuid: ColorUUID,
                    permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                    properties: CharacteristicProperty.Read | CharacteristicProperty.Write);

            colorCharacteristic.SetValue(0x0000FF00); //blue

            var service = new Service(
                 "ServiceA",
                 253,
                 isOnCharacteristic,
                 colorCharacteristic
            );

            return new Definition("MeadowRGB", service);
        }

        void ShowColor(Color color, int duration = 1000)
        {
            Console.WriteLine($"Color: {color}");
            onboardLed.SetColor(color);
            Thread.Sleep(duration);
            onboardLed.Stop();
        }
    }

    /*
    Color GetRandomColor()
    {
        int index = rand.Next() % 8;

        switch (index)
        {
            case 0:
                colorNameCharacteristic.SetValue("AliceBlue");
                return Color.AliceBlue;
            case 1:
                colorNameCharacteristic.SetValue("Green");
                return Color.Green;
            case 2:
                colorNameCharacteristic.SetValue("Yellow");
                return Color.Yellow;
            case 3:
                colorNameCharacteristic.SetValue("Orange");
                return Color.Orange;
            case 4:
                colorNameCharacteristic.SetValue("Red");
                return Color.Red;
            case 5:
                colorNameCharacteristic.SetValue("Purple");
                return Color.Purple;
            case 6:
                colorNameCharacteristic.SetValue("White");
                return Color.White;
            case 7:
            default:
                colorNameCharacteristic.SetValue("Pink");
                return Color.Pink;
        }
    }*/
    
}