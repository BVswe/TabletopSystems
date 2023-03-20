

namespace TabletopSystems.ViewModels;

public class ConnectToSqlViewModel : ObservableObject
{
    private UserConnection _connection;
    private string _isConnectedText;
    public string IsConnectedText
    {
        get { return _isConnectedText; }
        set
        {
            _isConnectedText = value;
            OnPropertyChanged();
        }
    }
    public string connection
    {
        get
        {
            return _connection.sqlString;
        }
        set
        {
            _connection.sqlString = "Data Source=" + value + "; Initial Catalog=TabletopSystems; Integrated Security=true; Encrypt=false ;Connection Timeout=1";
            OnPropertyChanged();
        }
    }
    public RelayCommand TryConnectionCommand { get; set; }
    public ConnectToSqlViewModel(UserConnection conn)
    {
        _connection = conn;
        TryConnectionCommand = new RelayCommand(o => {
            _connection.tryConnection();
            if (_connection.connectedToSqlServer)
            {
                IsConnectedText = "Connected!";
            }
            else
            {
                IsConnectedText = "Failed to connect.";
            }
            
        }, o => true);
    }
}
