

namespace TabletopSystems.Helper_Methods;

public interface INavigationService
{
    ObservableObject CurrentView { get; }
    void NavigateTo<T>() where T : ObservableObject;
}
