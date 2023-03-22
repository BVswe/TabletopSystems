

using System.Collections.ObjectModel;

namespace TabletopSystems.ViewModels
{
    public class ViewToPlayWithViewModel : ObservableObject
    {
        private string _message;
        private ObservableCollection<string> _messageLog;
        public ObservableCollection<string> MessageLog
        {
            get { return _messageLog; }
            set { _messageLog = value;}
        }
        public ObservableCollection<TabletopSystem> Systems {get; set;}
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand SendCommand { get; set; }
        public ViewToPlayWithViewModel() 
        {
            _message= string.Empty;
            _messageLog = new ObservableCollection<string>();
            Systems = new ObservableCollection<TabletopSystem>();
            SendCommand = new RelayCommand(o => { MessageLog.Add(Message); });
        }
    }
}
