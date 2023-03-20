

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using TabletopSystems.Helper_Methods;

namespace TabletopSystems.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    private INavigationService _navigation;
    private UserConnection _connection;
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
    public INavigationService Navigation
    {
        get => _navigation;
        set
        {
            _navigation = value;
            OnPropertyChanged();
        }
    }
    public RelayCommand GoToSystemSelectionCommand { get; set; }
    public RelayCommand GoToSystemMainPageCommand { get; set; }
    public RelayCommand GoToConnectionPageCommand { get; set; }



    public MainWindowViewModel(UserConnection conn, INavigationService navi)
    {
        Navigation = navi;
        _connection = conn;
        GoToSystemSelectionCommand = new RelayCommand(o => { Navigation.NavigateTo<SystemSelectionViewModel>(); }, o => true);
        GoToSystemMainPageCommand = new RelayCommand(o => { Navigation.NavigateTo<SystemMainPageViewModel>(); }, o => true);
        GoToConnectionPageCommand = new RelayCommand(o => { Navigation.NavigateTo<ConnectToSqlViewModel>(); }, o => true);
    }
}
