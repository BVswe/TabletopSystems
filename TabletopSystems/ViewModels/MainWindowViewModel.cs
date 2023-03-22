

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using TabletopSystems.Helper_Methods;

namespace TabletopSystems.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    private UserConnection _connection;
    private string _message;
    private ObservableCollection<string> _messageLog;
    private INavigationService _navi;
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
    public ObservableCollection<TabletopSystem> Systems { get; set; }
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
            else{
                return _connection.sqlString;
            }
        }
        set
        {
            _connection.sqlString = "Data Source=" + value + "; Initial Catalog=TabletopSystems; Integrated Security=true; Encrypt=false ;Connection Timeout=3";
            OnPropertyChanged();
        }
    }
    public RelayCommand SendCommand { get; set; }
    public RelayCommand BackCommand { get; set; }
    public MainWindowViewModel(UserConnection conn, INavigationService nav)
    {
        _connection = conn;
        _navi = Navi;
        _message = string.Empty;
        _messageLog = new ObservableCollection<string>();
        Systems = new ObservableCollection<TabletopSystem>();
        SendCommand = new RelayCommand(o => {
            if (String.IsNullOrEmpty(Message)){
                return;
            }
            MessageLog.Add(Message);
            Message = ""; 
        }, o => true );
        BackCommand = new RelayCommand(o => { Navi.NavigateTo<SystemSelectionViewModel>(); }, o => true );
    }
}
