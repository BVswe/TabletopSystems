using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;
using System.Windows.Input;
using TabletopSystems.Database_Access;

namespace TabletopSystems.ViewModels;

public class SystemSelectionViewModel : ObservableObject
{
    #region Properties/Fields

    private readonly UserConnection _userConnection;
    private readonly TabletopSystem _tabletopSystem;
    private SqlTabletopSystemRepository _tabletopSystemRepository;
    public string systemName {

        get { return _tabletopSystem.SystemName; }

        set {
            _tabletopSystem.SystemName = value;
            OnPropertyChanged();
        } 
    }
    public UserConnection userConnection
    {
        get { return _userConnection; }
    }

    public ICommand SystemSelectedCommand { get; }
    public ICommand AddSystemCommand { get; }
    public ICommand DeleteSystemCommand { get; }

    #endregion
    public SystemSelectionViewModel(UserConnection conn, TabletopSystem t)
    {
        _tabletopSystem = t;
        _userConnection = conn;
        _tabletopSystemRepository = new SqlTabletopSystemRepository(_userConnection);

        SystemSelectedCommand = new RelayCommand(p => ExecuteSystemSelectedCommand());
    }

    public void ExecuteSystemSelectedCommand()
    {
        Trace.WriteLine(_tabletopSystem.SystemID);
        _tabletopSystem.SystemID = _tabletopSystemRepository.GetIDBySystemName(_tabletopSystem.SystemName);
    }
}
