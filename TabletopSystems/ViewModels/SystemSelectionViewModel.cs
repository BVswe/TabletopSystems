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
    private string _hostName;
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
    public string HostName
    {
        get { return _hostName; }
        set { _hostName = value; OnPropertyChanged(); }
    }

    public ICommand SystemSelectedCommand { get; }
    public ICommand AddSystemCommand { get; }
    public ICommand DeleteSystemCommand { get; }
    public ICommand ReloadCommand { get; }
    public ICommand ConnectCommand { get; }

    public SystemSelectionViewModel(UserConnection conn, MainWindowViewModel mainWinViewModel)
    {
        _selectedSystem = new TabletopSystem();
        _userConnection = conn;
        _hostName = "";
        _tabletopSystemRepository = new TabletopSystemRepository(_userConnection);
        _systems = _tabletopSystemRepository.GetSystems();
        _mainWindowViewModel = mainWinViewModel;
        SystemSelectedCommand = new RelayCommand(o => { ExecuteSystemSelectedCommand(); }, o => true);
        DeleteSystemCommand = new RelayCommand(o => ExecuteDeleteSystemCommand());
        AddSystemCommand = new RelayCommand(o => ExecuteAddSystemCommand());
        ReloadCommand = new RelayCommand(o => ExecuteReloadCommand());
        ConnectCommand = new RelayCommand(o => ExecuteConnectCommand());
        //Trace.WriteLine("System Selection View Model constructed!");
    }

    public void ExecuteReloadCommand()
    {
        Systems = _tabletopSystemRepository.GetSystems();
    }
    public void ExecuteConnectCommand()
    {
        _userConnection.tryConnection(_hostName);
        Systems = _tabletopSystemRepository.GetSystems();
        if (_userConnection.connectedToSqlServer)
        {
            MessageBox.Show($"Connected to {_hostName}!");
        }
        else { MessageBox.Show($"Connection Failed."); };
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
