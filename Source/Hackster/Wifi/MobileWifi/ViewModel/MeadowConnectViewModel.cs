using MobileWifi.Utils;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System.Text;
using System.Windows.Input;

namespace MobileWifi.ViewModel
{
    public class MeadowConnectViewModel : BaseViewModel
    {
        ICharacteristic CharacteristicSsid;
        ICharacteristic CharacteristicPassword;
        ICharacteristic CharacteristicConnect;

        bool _showPassword;
        public bool ShowPassword
        {
            get => _showPassword;
            set { _showPassword = value; OnPropertyChanged(nameof(ShowPassword)); }
        }


        string _ssid;
        public string Ssid
        {
            get => _ssid;
            set { _ssid = value; OnPropertyChanged(nameof(Ssid)); }
        }

        string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        public ICommand ConnectCommand { get; set; }

        public ICommand TogglePasswordVisibility { get; set; }

        public MeadowConnectViewModel()
        {
            adapter.DeviceConnected += AdapterDeviceConnected;
            adapter.DeviceDisconnected += AdapterDeviceDisconnected;

            ConnectCommand = new Command(async () => 
            {
                var ssid = Encoding.ASCII.GetBytes(Ssid);
                await CharacteristicSsid.WriteAsync(ssid);

                await Task.Delay(500);

                var password = Encoding.ASCII.GetBytes(Password);
                await CharacteristicPassword.WriteAsync(password);

                await Task.Delay(500);

                byte[] connect = new byte[1] { 1 };
                await CharacteristicConnect.WriteAsync(connect);
            });

            TogglePasswordVisibility = new Command(() => ShowPassword = !ShowPassword);
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

            CharacteristicSsid = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.SSID));
            CharacteristicPassword = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.PASSWORD));
            CharacteristicConnect = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.CONNECT));
        }
    }
}