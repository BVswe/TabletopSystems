
using TabletopSystems.Database_Access;

namespace TabletopSystems.ViewModels;

public class SystemMainPageViewModel : ObservableObject
{
    private readonly UserConnection _userConnection;
    private readonly TabletopSystem _tabletopSystem;
    public SystemMainPageViewModel(UserConnection conn, TabletopSystem t)
    {
        _userConnection = conn;
        _tabletopSystem = t;
    }
}
