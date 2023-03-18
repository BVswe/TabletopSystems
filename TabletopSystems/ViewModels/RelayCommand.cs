using System;
using System.Windows.Input;

namespace TabletopSystems.ViewModels;

public class RelayCommand : ICommand
{
    private readonly Action<object> _executeAction;
    private readonly Predicate<object> _canExecuteAction;

    #region Constructors
    public RelayCommand(Action<object> executeAction)
    {
        _executeAction = executeAction;
        _canExecuteAction = null;
    }

    //Constrcutors
    public RelayCommand(Action<object> executeAction, Predicate<object> canExecuteAction)
    {
        _executeAction = executeAction;
        _canExecuteAction = canExecuteAction;
    }

    #endregion

    //Events
    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecuteAction == null ? true : _canExecuteAction(parameter);
    }

    public void Execute(object? parameter)
    {
        _executeAction(parameter);
    }
}
