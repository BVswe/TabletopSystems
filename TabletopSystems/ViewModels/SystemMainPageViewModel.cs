

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using TabletopSystems.Helper_Methods;


namespace TabletopSystems.ViewModels
{
    public class SystemMainPageViewModel : ObservableObject
    {
        private TabletopSystem _currentSystem;
        private UserConnection _connection;
        private ObservableCollection<string> _messageLog;
        private ObservableCollection<ObservableObject> _viewModels;
        private string _message;
        private INavigationService _navi;
        public TabletopSystem CurrentSystem
        {
            get { return _currentSystem; }
            set
            {
                _currentSystem = value;
                OnPropertyChanged();
            }
        }
        //tie back button to a bool
        public INavigationService Navi
        {
            get { return _navi; }
            set
            {
                _navi = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> MessageLog
        {
            get { return _messageLog; }
            set { _messageLog = value; }
        }
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }
        public string connection
        {
            get
            {
                if (_connection.connectedToSqlServer)
                {
                    return _connection.sqlString;
                }
                else
                {
                    return _connection.sqliteString;
                }
            }
            set
            {
                _connection.sqlString = "Data Source=" + value + "; Initial Catalog=TabletopSystems; Integrated Security=true; Encrypt=false ;Connection Timeout=3";
                OnPropertyChanged();
            }
        }
        public RelayCommand SendCommand { get; set; }
        public SystemMainPageViewModel(UserConnection conn)
        {
            _message = string.Empty;
            _messageLog = new ObservableCollection<string>();
            _connection = conn;
            SendCommand = new RelayCommand(o => {
                if (String.IsNullOrEmpty(Message))
                {
                    return;
                }
                MessageLog.Add(Message);
                Message = "";
            }, o => true);
            Trace.WriteLine("SystemMainPageView was constructed!");
        }
    }
}
