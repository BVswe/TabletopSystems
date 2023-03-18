

using TabletopSystems.Helper_Methods;

namespace TabletopSystems.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    private INavigationService _navigation;
    private string _connection;
    public string connection
    {
        get => _connection;
        set
        {
            _connection = value;
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

    public MainWindowViewModel(INavigationService navi)
    {
        Navigation = navi;
        GoToSystemSelectionCommand = new RelayCommand(o => { Navigation.NavigateTo<SystemSelectionViewModel>(); }, o => true);
    }
}
