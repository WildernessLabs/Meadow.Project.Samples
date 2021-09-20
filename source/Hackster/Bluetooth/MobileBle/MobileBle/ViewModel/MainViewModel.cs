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

        public IDevice DeviceSelected { get; set; }

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

        public ICommand CmdConnect { get; set; }

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

            CmdConnect = new Command(async () => await Connect());
        }

        int UuidToUshort(string uuid)
        {
            int result;

            string id = uuid.Substring(4, 4);

            result = int.Parse(id, System.Globalization.NumberStyles.HexNumber);

            return result;
        }

        public async Task DiscoverDevices()
        {
            try
            {
                adapter.DeviceDiscovered += (s, a) =>
                {
                    if (DeviceList.FirstOrDefault(x => x.Name == a.Device.Name) == null)
                    {
                        DeviceList.Add(a.Device);
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

        public async Task Connect()
        {
            try
            {
                await adapter.ConnectToDeviceAsync(DeviceSelected);
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
