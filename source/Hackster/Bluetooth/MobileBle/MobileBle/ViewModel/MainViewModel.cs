using MobileBle.Utils;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileBle.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        ushort DEVICE_ID = 253;

        IAdapter adapter;
        IService service;
        ICharacteristic toggleLed;
        ICharacteristic setColor;

        public ObservableCollection<IDevice> DeviceList { get; set; }

        IDevice deviceSelected;
        public IDevice DeviceSelected 
        {
            get => deviceSelected;
            set { deviceSelected = value; OnPropertyChanged(nameof(DeviceSelected)); }
        }

        bool isDeviceListEmpty;
        public bool IsDeviceListEmpty
        {
            get => isDeviceListEmpty;
            set { isDeviceListEmpty = value; OnPropertyChanged(nameof(IsDeviceListEmpty)); }
        }

        bool isLedOn;
        public bool IsLedOn
        {
            get => isLedOn;
            set { isLedOn = value; OnPropertyChanged(nameof(IsLedOn)); }
        }

        bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            set { isConnected = value; OnPropertyChanged(nameof(IsConnected)); }
        }

        Color selectedColor;
        public Color SelectedColor
        {
            get => selectedColor;
            set { selectedColor = value; OnPropertyChanged(nameof(SelectedColor)); }
        }

        public ICommand CmdToggle { get; set; }

        public ICommand CmdSetColor { get; set; }

        public ICommand CmdToggleConnection { get; set; }

        public ICommand CmdSearchForDevices { get; set; }

        public MainViewModel()
        {
            DeviceList = new ObservableCollection<IDevice>();

            IBluetoothLE ble = CrossBluetoothLE.Current;

            adapter = CrossBluetoothLE.Current.Adapter;
            adapter.ScanMode = ScanMode.LowLatency;
            adapter.DeviceConnected += async (s, e) =>
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
            };
            adapter.DeviceDisconnected += (s, e) =>
            {
                IsConnected = false;
            };

            CmdToggle = new Command(async () =>
            {
                IsLedOn = !IsLedOn;

                byte[] array = new byte[1];
                array[0] = isLedOn ? (byte)1 : (byte)0;

                await toggleLed.WriteAsync(array);
            });

            CmdSetColor = new Command(async () =>
            {
                byte[] array;

                string colorHex = SelectedColor.ToHex().Substring(1, 8);

                var color = int.Parse(colorHex, System.Globalization.NumberStyles.HexNumber);

                array = BitConverter.GetBytes(color);

                await setColor.WriteAsync(array);
            });

            CmdToggleConnection = new Command(async () => await ToggleConnection());

            CmdSearchForDevices = new Command(async () => await DiscoverDevices());
        }

        int UuidToUshort(string uuid)
        {
            int result;

            string id = uuid.Substring(4, 4);

            result = int.Parse(id, System.Globalization.NumberStyles.HexNumber);

            return result;
        }

        async Task ToggleConnection()
        {
            try
            {
                if (IsConnected)
                {
                    await adapter.DisconnectDeviceAsync(DeviceSelected);
                    IsConnected = false;
                }
                else
                {
                    await adapter.ConnectToDeviceAsync(DeviceSelected);
                    IsConnected = true;
                }
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

        public async Task DiscoverDevices()
        {
            try
            {
                adapter.DeviceDiscovered += (s, a) =>
                {
                    if (DeviceList.FirstOrDefault(x => x.Name == a.Device.Name) == null && 
                        !string.IsNullOrEmpty(a.Device.Name))
                    {
                        DeviceList.Add(a.Device);
                    }

                    if (a.Device.Name == "MeadowRGB")
                    {
                        IsDeviceListEmpty = false;
                        DeviceSelected = a.Device;
                    }
                };
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

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}