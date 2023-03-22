

using System;

namespace TabletopSystems.Helper_Methods;

public class NavigationService : ObservableObject, INavigationService
{
    private ObservableObject _currentView;
    private readonly Func<Type, ObservableObject> _viewModelFactory;

    public ObservableObject CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public NavigationService(Func<Type, ObservableObject> viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }

    public void NavigateTo<TViewModelBase>() where TViewModelBase : ObservableObject
    {
        ObservableObject viewModel = _viewModelFactory.Invoke(typeof(TViewModelBase));
        CurrentView = viewModel;
    }
}
