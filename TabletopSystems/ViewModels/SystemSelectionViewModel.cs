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
    private TabletopSystem _selectedSystem;
    private ObservableCollection<TabletopSystem> _systems;
    private TabletopSystemRepository _tabletopSystemRepository;
    private MainWindowViewModel _mainWindowViewModel;
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
    public ICommand ReloadCommand { get; }

    public SystemSelectionViewModel(UserConnection conn, MainWindowViewModel mainWinViewModel)
    {
        _selectedSystem = new TabletopSystem();
        _userConnection = conn;
        _tabletopSystemRepository = new TabletopSystemRepository(_userConnection);
        _systems = _tabletopSystemRepository.GetSystems();
        _mainWindowViewModel = mainWinViewModel;
        SystemSelectedCommand = new RelayCommand(o => { ExecuteSystemSelectedCommand(); }, o => true);
        DeleteSystemCommand = new RelayCommand(o => ExecuteDeleteSystemCommand());
        AddSystemCommand = new RelayCommand(o => ExecuteAddSystemCommand());
        ReloadCommand = new RelayCommand(o => ExecuteReloadCommand());
        Trace.WriteLine("System Selection View Model constructed!");
    }

    public void ExecuteReloadCommand()
    {
        _userConnection.tryConnection();
        Systems = _tabletopSystemRepository.GetSystems();
    }

    public void ExecuteAddSystemCommand()
    {
        _mainWindowViewModel.Navi.NavigateTo<AddSystemViewModel>();
    }

    public void ExecuteSystemSelectedCommand()
    {
        _mainWindowViewModel.TbltopSys = SelectedSystem;
        _mainWindowViewModel.Navi.NavigateTo<SystemMainPageViewModel>();
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
