using BleRover.Client.Utils;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BleRover.Client.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        protected ushort DEVICE_ID = 253;

        IAdapter adapter;
        IService service;
        ICharacteristic up, down, left, right;

        public ObservableCollection<IDevice> DeviceList { get; set; }

        IDevice deviceSelected;
        public IDevice DeviceSelected
        {
            get => deviceSelected;
            set { deviceSelected = value; OnPropertyChanged(nameof(DeviceSelected)); }
        }

        bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            set { isConnected = value; OnPropertyChanged(nameof(IsConnected)); }
        }

        bool isDeviceListEmpty;
        public bool IsDeviceListEmpty
        {
            get => isDeviceListEmpty;
            set { isDeviceListEmpty = value; OnPropertyChanged(nameof(IsDeviceListEmpty)); }
        }

        public ICommand CmdSearchForDevices { get; set; }

        public MainViewModel() 
        {
            IBluetoothLE ble = CrossBluetoothLE.Current;

            DeviceList = new ObservableCollection<IDevice>();

            adapter = CrossBluetoothLE.Current.Adapter;
            adapter.ScanMode = ScanMode.LowLatency;
            adapter.DeviceDiscovered += DeviceDiscovered;
            adapter.DeviceConnected += DeviceConnected;
            adapter.DeviceDisconnected += (s, e) =>
            {
                IsConnected = false;
            };

            CmdSearchForDevices = new Command(async () => await DiscoverDevices());
        }

        int UuidToUshort(string uuid)
        {
            int result;

            string id = uuid.Substring(4, 4);

            result = int.Parse(id, System.Globalization.NumberStyles.HexNumber);

            return result;
        }

        async void DeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            if (DeviceList.FirstOrDefault(x => x.Name == e.Device.Name) == null &&
                !string.IsNullOrEmpty(e.Device.Name))
            {
                DeviceList.Add(e.Device);
            }

            if (e.Device.Name == "MeadowRover")
            {
                IsDeviceListEmpty = false;
                DeviceSelected = e.Device;
                await adapter.ConnectToDeviceAsync(DeviceSelected);
            }
        }

        async Task DiscoverDevices()
        {
            try
            {
                await adapter.StartScanningForDevicesAsync();
            }
            catch (DeviceConnectionException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async void DeviceConnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            IsConnected = true;

            var services = await DeviceSelected.GetServicesAsync();
            foreach (var serviceItem in services)
            {
                if (UuidToUshort(serviceItem.Id.ToString()) == DEVICE_ID)
                {
                    service = serviceItem;
                }
            }

            up = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.UP));
            down = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.DOWN));
            left = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.LEFT));
            right = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.RIGHT));
        }

        public async Task Connect() 
        {
            try
            {
                Guid guid = new Guid("00000000-0000-0000-0000-d8a01d697eaa");
                await adapter.ConnectToKnownDeviceAsync(guid);
            }
            catch (DeviceConnectionException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task MoveForward(bool go)
        {
            byte[] array = new byte[1];
            array[0] = go ? (byte)1 : (byte)0;

            await up.WriteAsync(array);
        }

        public async Task MoveBackward(bool go)
        {
            byte[] array = new byte[1];
            array[0] = go ? (byte)1 : (byte)0;

            await down.WriteAsync(array);
        }

        public async Task TurnLeft(bool go)
        {
            byte[] array = new byte[1];
            array[0] = go ? (byte)1 : (byte)0;

            await left.WriteAsync(array);
        }

        public async Task TurnRight(bool go)
        {
            byte[] array = new byte[1];
            array[0] = go ? (byte)1 : (byte)0;

            await right.WriteAsync(array);
        }
    }
}