using System;
using System.Windows.Input;

namespace TabletopSystems;

public class SelectSystemViewModel : ViewModelBase
{
    private string _systemName;
    public string SystemName {

        get { return _systemName; }

        set {
            _systemName = value;
            OnPropertyChanged();
        } 
    }

    public ICommand SystemSaveCommand { get; }
    public SelectSystemViewModel()
    {
        SystemSaveCommand = new RelayCommand(ExecuteSystemSaveCommand, p => CanExecuteSystemSaveCommand(""));
    }

    private void ExecuteSystemSaveCommand(object obj)
    {
        throw new NotImplementedException();
    }

    private bool CanExecuteSystemSaveCommand(string inputSystemName)
    {
        throw new NotImplementedException();
    }
}
