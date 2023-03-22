﻿using System.Diagnostics;
using TabletopSystems.Helper_Methods;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Windows;

namespace TabletopSystems.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    #region Fields and Properties
    private UserConnection _connection;
    
    private INavigationService _navi;
    //tie back button to a bool
    public INavigationService Navi
    {
        get { return _navi; }
        set
        {
            _navi = value;
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
                return _connection.sqliteString;
            }
        }
        set
        {
            _connection.sqlString = "Data Source=" + value + "; Initial Catalog=TabletopSystems; Integrated Security=true; Encrypt=false ;Connection Timeout=3";
            OnPropertyChanged();
        }
    }
    public RelayCommand NavigateSystemMainPageCommand { get; set; }
    public RelayCommand BackCommand { get; set; }
    #endregion
    public MainWindowViewModel(UserConnection conn, INavigationService navi, IServiceScopeFactory serviceScope)
    {
        _connection = conn;
        _navi = navi;
        BackCommand = new RelayCommand(o => { ExecuteBackCommand(); }, o => true);
        BackCommand.Execute(null);
        Trace.WriteLine("MainWindowView was constructed!");
    }
    /// <summary>
    /// Go back to System Selection view
    /// </summary>
    public void ExecuteBackCommand()
    {
        Navi.NavigateTo<SystemSelectionViewModel>();
    }
}
