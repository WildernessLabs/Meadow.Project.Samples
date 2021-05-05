using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BleRover.Client.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {



        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
