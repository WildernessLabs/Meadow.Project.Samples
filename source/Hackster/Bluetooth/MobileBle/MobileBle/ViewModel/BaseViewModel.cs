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
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected ushort DEVICE_ID = 253;

        protected IAdapter adapter;
        protected IService service;

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

        public ICommand CmdToggleConnection { get; set; }

        public ICommand CmdSearchForDevices { get; set; }

        public BaseViewModel()
        {
            DeviceList = new ObservableCollection<IDevice>();

            adapter = CrossBluetoothLE.Current.Adapter;
            adapter.ScanMode = ScanMode.LowLatency;
            adapter.DeviceDiscovered += AdapterDeviceDiscovered;

            CmdToggleConnection = new Command(async () => await ToggleConnection());

            CmdSearchForDevices = new Command(async () => await DiscoverDevices());
        }

        void AdapterDeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            if (DeviceList.FirstOrDefault(x => x.Name == e.Device.Name) == null &&
                !string.IsNullOrEmpty(e.Device.Name))
            {
                DeviceList.Add(e.Device);
            }

            if (e.Device.Name == "MeadowRGB")
            {
                IsDeviceListEmpty = false;
                DeviceSelected = e.Device;
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

        protected int UuidToUshort(string uuid)
        {
            int result;

            string id = uuid.Substring(4, 4);

            result = int.Parse(id, System.Globalization.NumberStyles.HexNumber);

            return result;
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