using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TabletopSystems;

public abstract class ObservableObject : INotifyPropertyChanged
{
    //Notifies binding clients that a property has changed and they need to reevaluate
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Base OnPropertyChanged method
    /// </summary>
    //CallerMemberName allows us to use OnPropertyChanged() in a getter/setter instead of OnPropertyChanged("Property")
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName=null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
