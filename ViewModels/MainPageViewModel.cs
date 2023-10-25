using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace WinUIConnectivityIssue.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public MainPageViewModel()
        {
            MainThreadCommand = new Command(() => MainThreadResult = $"{Connectivity.Current.NetworkAccess}");
            BackgroundThreadCommand = new Command(() => {
                string state = string.Empty;

                Task.Run(() =>
                {
                    state = Connectivity.Current.NetworkAccess.ToString();   
                }).ContinueWith((task) => 
                {
                    BackgroundThreadResult = $"{state}";
                });
                }
            );
        }

        #region Properties

        private string mainThreadResult;
        public string MainThreadResult
        {
            get
            {
                return mainThreadResult;
            }
            set
            {
                mainThreadResult = value;
                OnPropertyChanged();
            }
        }

        private string backgroundThreadResult;
        public string BackgroundThreadResult
        {
            get
            {
                return backgroundThreadResult;
            }
            set
            {
                backgroundThreadResult = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands

        public ICommand MainThreadCommand { get; }

        public ICommand BackgroundThreadCommand { get; }

        #endregion

        #region INotifyPropertyChanged

        public event Action OnChange;
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void NotifyStateChanged() => OnChange?.Invoke();

        #endregion
    }
}
