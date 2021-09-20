using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Gateways.Bluetooth;

namespace MeadowBleLed
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

            PulseColor(Color.Green);
        }

        void Initialize()
        {
            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            PulseColor(Color.Red);

            Device.InitCoprocessor();

            bleTreeDefinition = GetDefinition();
            Device.BluetoothAdapter.StartBluetoothServer(bleTreeDefinition);

            isOnCharacteristic.ValueSet += IsOnCharacteristicValueSet;
            colorCharacteristic.ValueSet += ColorCharacteristicValueSet;
        }

        void IsOnCharacteristicValueSet(ICharacteristic c, object data)
        {
            if ((int)data > 0)
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

        void ColorCharacteristicValueSet(ICharacteristic c, object data)
        {
            int color = (int)data;

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

        Definition GetDefinition()
        {
            isOnCharacteristic = new CharacteristicBool(
                name: "On_Off",
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
    }
}