

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows;
using TabletopSystems.Helper_Methods;


namespace TabletopSystems.ViewModels
{
    public class SystemMainPageViewModel : ObservableObject
    {
        private UserConnection _connection;
        private ObservableCollection<string> _messageLog;
        private ObservableCollection<ObservableObject> _viewModels;
        private string _message;
        private readonly MainWindowViewModel _mainWinViewModel;
        public MainWindowViewModel MainWinViewModel { get { return _mainWinViewModel; } }

        public string Online
        {
            get
            {
                if (_connection.connectedToSqlServer) return "Online";
                return "Offline";
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

        public ObservableCollection<ObservableObject> ViewModels
        {
            get { return _viewModels; }
        }

        public RelayCommand SendCommand { get; set; }
        public SystemMainPageViewModel(UserConnection conn, MainWindowViewModel mainWinVM, AddSystemViewModel addViewModel, SearchViewModel searchVM, CharactersViewModel charVM, AddItemMainViewModel AddTypeVM)
        {
            _mainWinViewModel = mainWinVM;
            _message = string.Empty;
            _messageLog = new ObservableCollection<string>();
            _connection = conn;
            _viewModels = new ObservableCollection<ObservableObject>
            {
                searchVM,
                charVM,
                AddTypeVM
            };
            SendCommand = new RelayCommand(o => {
                if (String.IsNullOrEmpty(Message))
                {
                    return;
                }
                MessageLog.Add(Message);
                Message = "";
            }, o => true);
            //Trace.WriteLine("SystemMainPageView was constructed!");
        }
    }
}
