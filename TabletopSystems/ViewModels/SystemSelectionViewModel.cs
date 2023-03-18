using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;
using System.Windows.Input;
using TabletopSystems.Database_Access;

namespace TabletopSystems;

public class SystemSelectionViewModel : ViewModelBase
{
    #region Properties/Fields

    private readonly UserConnection _userConnection;
    private readonly TabletopSystem _tabletopSystem;
    public string systemName {

        get { return _tabletopSystem.SystemName; }

        set {
            _tabletopSystem.SystemName = value;
            OnPropertyChanged();
        } 
    }

    public ICommand SystemSelectedCommand { get; }
    public ICommand AddSystemCommand { get; }
    public ICommand DeleteSystemCommand { get; }

    #endregion

    public SystemSelectionViewModel(UserConnection conn, TabletopSystem t)
    {
        _tabletopSystem = t;
        _userConnection = conn;
        SystemSelectedCommand = new RelayCommand(p => ExecuteSystemSelectedCommand());
    }

    public void ExecuteSystemSelectedCommand()
    {
        SqlTabletopSystemRepository db = new SqlTabletopSystemRepository(_userConnection);
        _tabletopSystem.SystemID = db.GetIDBySystemName(_tabletopSystem.SystemName);
        Trace.WriteLine(_tabletopSystem.SystemID);

    }
}
