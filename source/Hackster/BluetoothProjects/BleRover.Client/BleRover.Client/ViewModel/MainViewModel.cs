using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BleRover.Client.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        IBluetoothLE ble;
        IAdapter adapter;
        BluetoothState state;
        IDevice device;
        ICharacteristic up, down, left, right;

        bool isConnected;
        public bool IsConnected 
        {
            get => isConnected;
            set { isConnected = value; OnPropertyChanged(nameof(IsConnected)); }
        }

        public MainViewModel() 
        {
            ble = CrossBluetoothLE.Current;

            adapter = CrossBluetoothLE.Current.Adapter;
            adapter.ScanMode = ScanMode.LowLatency;
            adapter.DeviceConnected += async (s,e) => 
            {
                IsConnected = true;

                device = e.Device;

                var services = await device.GetServicesAsync();

                up = await services[2].GetCharacteristicAsync(new Guid("017e99d6-8a61-11eb-8dcd-0242ac1300aa"));
                down = await services[2].GetCharacteristicAsync(new Guid("017e99d6-8a61-11eb-8dcd-0242ac1300bb"));
                left = await services[2].GetCharacteristicAsync(new Guid("017e99d6-8a61-11eb-8dcd-0242ac1300cc"));
                right = await services[2].GetCharacteristicAsync(new Guid("017e99d6-8a61-11eb-8dcd-0242ac1300dd"));
            };
            adapter.DeviceDisconnected += (s, e) =>
            {
                IsConnected = false;
            };

            state = ble.State;
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

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
