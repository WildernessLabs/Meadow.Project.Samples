using MobileBle.Utils;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileBle.ViewModel
{
    public class LedControllerViewModel : BaseViewModel
    {
        ICharacteristic toggleLed;
        ICharacteristic setColor;

        bool isLedOn;
        public bool IsLedOn
        {
            get => isLedOn;
            set { isLedOn = value; OnPropertyChanged(nameof(IsLedOn)); }
        }

        Color selectedColor;
        public Color SelectedColor
        {
            get => selectedColor;
            set { selectedColor = value; OnPropertyChanged(nameof(SelectedColor)); }
        }

        public ICommand CmdToggle { get; set; }

        public ICommand CmdSetColor { get; set; }

        public LedControllerViewModel()
        {
            IsLedOn = true;

            adapter.DeviceConnected += AdapterDeviceConnected;
            adapter.DeviceDisconnected += AdapterDeviceDisconnected;

            CmdToggle = new Command(async () => await LedToggle());

            CmdSetColor = new Command(async () => await LedSetColor());
        }

        void AdapterDeviceDisconnected(object sender, DeviceEventArgs e)
        {
            IsConnected = false;
        }

        async void AdapterDeviceConnected(object sender, DeviceEventArgs e)
        {
            IsConnected = true;

            IDevice device = e.Device;

            var services = await device.GetServicesAsync();

            foreach (var serviceItem in services)
            {
                if (UuidToUshort(serviceItem.Id.ToString()) == DEVICE_ID)
                {
                    service = serviceItem;
                }
            }

            toggleLed = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.IS_ON));
            setColor = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.COLOR));
        }

        async Task LedToggle()
        {
            IsLedOn = !IsLedOn;

            byte[] array = new byte[1];
            array[0] = IsLedOn ? (byte)1 : (byte)0;

            await toggleLed.WriteAsync(array);
        }

        async Task LedSetColor()
        {
            byte[] array;

            var color = int.Parse(SelectedColor.ToHex().Substring(1, 8), System.Globalization.NumberStyles.HexNumber);

            array = BitConverter.GetBytes(color);

            await setColor.WriteAsync(array);
        }
    }
}