using MobileBle.Utils;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileBle.ViewModel
{
    public class ServoControllerViewModel : BaseViewModel
    {
        ICharacteristic toggleCycling;
        ICharacteristic setAngle;

        int _angleDegrees;
        public int AngleDegrees
        {
            get => _angleDegrees;
            set { _angleDegrees = value; OnPropertyChanged(nameof(AngleDegrees)); }
        }

        bool _isRotateTo;
        public bool IsRotateTo
        {
            get => _isRotateTo;
            set { _isRotateTo = value; OnPropertyChanged(nameof(IsRotateTo)); }
        }

        bool _isCyclingStart;
        public bool IsCyclingStart
        {
            get => _isCyclingStart;
            set { _isCyclingStart = value; OnPropertyChanged(nameof(IsCyclingStart)); }
        }

        bool _isCyclingStop;
        public bool IsCyclingStop
        {
            get => _isCyclingStop;
            set { _isCyclingStop = value; OnPropertyChanged(nameof(IsCyclingStop)); }
        }

        public ICommand CmdSetAngle { get; set; }

        public ICommand CmdToggleCycling { get; set; }

        public ServoControllerViewModel()
        {
            IsRotateTo = true;

            adapter.DeviceConnected += AdapterDeviceConnected;
            adapter.DeviceDisconnected += AdapterDeviceDisconnected;

            CmdSetAngle = new Command(async () => await ServoSetAngle());

            CmdToggleCycling = new Command(async (b) => await ToggleCycling((bool)b));
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

            toggleCycling = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.IS_CYCLING));
            setAngle = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.ANGLE));
        }

        async Task ServoSetAngle()
        {
            IsRotateTo = true;
            IsCyclingStart = false;
            IsCyclingStop = false;

            byte[] array = new byte[1];
            array[0] = (byte)AngleDegrees;

            await setAngle.WriteAsync(array);
        }

        async Task ToggleCycling(bool isCycling)
        {
            IsRotateTo = false;
            IsCyclingStart = isCycling;
            IsCyclingStop = !isCycling;

            byte[] array = new byte[1];
            array[0] = isCycling ? (byte)1 : (byte)0;

            await toggleCycling.WriteAsync(array);
        }
    }
}