using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using TabletopSystems.Database_Access;
using TabletopSystems.Helper_Methods;

namespace TabletopSystems.ViewModels;

public class SystemSelectionViewModel : ObservableObject
{
    private readonly UserConnection _userConnection;
    private INavigationService _navi;
    private TabletopSystem _selectedSystem;
    private ObservableCollection<TabletopSystem> _systems;
    private SqlTabletopSystemRepository _tabletopSystemRepository;
    private IServiceScopeFactory _serviceScope;
    public INavigationService Navi
    {
        get { return _navi; }
        set
        {
            _navi = value;
            OnPropertyChanged();
        }
    }
    public TabletopSystem SelectedSystem
    {
        get { return _selectedSystem; }
        set
        {
            _selectedSystem = value;
            OnPropertyChanged();
        }
    }
    public ObservableCollection<TabletopSystem> Systems
    {
        get { return _systems; }
        set
        {
            _systems = value;
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

    public SystemSelectionViewModel(UserConnection conn, INavigationService navi, IServiceScopeFactory serviceScope)
    {
        _navi = navi;
        _selectedSystem = new TabletopSystem();
        _userConnection = conn;
        _serviceScope = serviceScope;
        _tabletopSystemRepository = new SqlTabletopSystemRepository(_userConnection);
        _systems = _tabletopSystemRepository.GetSystems();
        SystemSelectedCommand = new RelayCommand(o => { ExecuteSystemSelectedCommand(); }, o => true);
        DeleteSystemCommand = new RelayCommand(p => ExecuteDeleteSystemCommand());
        AddSystemCommand = new RelayCommand(p => ExecuteAddSystemCommand());
    }

    public void ExecuteAddSystemCommand()
    {
        //Switch pages here later
    }

    public void ExecuteSystemSelectedCommand()
    {
        Navi.NavigateTo<SystemMainPageViewModel>();
    }

    public void ExecuteDeleteSystemCommand()
    {
        MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete " + SelectedSystem.SystemName + "?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
        if (messageBoxResult == MessageBoxResult.Yes)
        {
            _tabletopSystemRepository.Delete(SelectedSystem);
            _systems.Remove(SelectedSystem);
        }
        else
        {
            return;
        }
    }
}
