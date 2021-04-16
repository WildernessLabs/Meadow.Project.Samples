using Connected.Client;
using Meadow.Foundation.Maple.Client;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Connected.Client.ViewModel
{
    public class LedControllerViewModel : INotifyPropertyChanged
    {
        LedClient ledClient;

        bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(nameof(IsBusy)); }
        }

        bool _isOn;
        public bool IsOn 
        {
            get => _isOn;
            set { _isOn = value; OnPropertyChanged(nameof(IsOn)); }
        }

        bool _isOff;
        public bool IsOff
        {
            get => _isOff;
            set { _isOff = value; OnPropertyChanged(nameof(IsOff)); }
        }

        bool _isPulsing;
        public bool IsPulsing
        {
            get => _isPulsing;
            set { _isPulsing = value; OnPropertyChanged(nameof(IsPulsing)); }
        }

        bool _isBlinking;
        public bool IsBlinking
        {
            get => _isBlinking;
            set { _isBlinking = value; OnPropertyChanged(nameof(IsBlinking)); }
        }

        bool _isRunningColors;
        public bool IsRunningColors
        {
            get => _isRunningColors;
            set { _isRunningColors = value; OnPropertyChanged(nameof(IsRunningColors)); }
        }

        bool _isServerListEmpty;
        public bool IsServerListEmpty
        {
            get => _isServerListEmpty;
            set { _isServerListEmpty = value; OnPropertyChanged(nameof(IsServerListEmpty)); }
        }

        ServerModel _selectedServer;
        public ServerModel SelectedServer
        {
            get => _selectedServer;
            set { _selectedServer = value; OnPropertyChanged(nameof(SelectedServer)); }
        }

        public ObservableCollection<ServerModel> HostList { get; set; }

        public Command SendCommand { private set; get; }

        public Command SearchServersCommand { private set; get; }

        public LedControllerViewModel()
        {
            HostList = new ObservableCollection<ServerModel>();

            ledClient = new LedClient();
            ledClient.Servers.CollectionChanged += ServersCollectionChanged;

            SendCommand = new Command(async (obj) => await SendLedCommand(obj as string));

            SearchServersCommand = new Command(async () => await GetServers());

            IsOn = true;
        }

        public async Task GetServers()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                IsServerListEmpty = false;

                await ledClient.StartScanningForAdvertisingServers();

                if (HostList.Count == 0)
                {
                    IsServerListEmpty = true;
                }
                else
                {
                    IsServerListEmpty = false;
                    SelectedServer = HostList[0];
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            { 
                IsBusy = false;
            }
        }

        void ServersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (ServerModel server in e.NewItems)
                    {
                        HostList.Add(new ServerModel() { Name = $"{server.Name} ({server.IpAddress})", IpAddress = server.IpAddress });
                        Console.WriteLine($"'{server.Name}' @ ip:[{server.IpAddress}]");
                    }
                    break;
            }
        }

        async Task SendLedCommand(string command)
        {
            if (IsBusy)
                return;
            IsBusy = true;

            bool response = await ledClient.SendLedCommand(SelectedServer, command);

            if (response)
            {
                IsOn = IsOff = IsBlinking = IsPulsing = IsRunningColors = false;

                switch (command) 
                {
                    case "TurnOn": IsOn = true; break;
                    case "TurnOff": IsOff = true; break;
                    case "StartBlink": IsBlinking = true; break;
                    case "StartPulse": IsPulsing = true; break;
                    case "StartRunningColors": IsRunningColors = true; break;
                }
            }
            else
            {
                await App.Current.DisplayAlert("Error", "Request failed.", "Close");
            }

            IsBusy = false;
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