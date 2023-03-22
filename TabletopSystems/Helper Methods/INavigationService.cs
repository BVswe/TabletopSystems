

namespace TabletopSystems.Helper_Methods;

public interface INavigationService
{
    ObservableObject CurrentView { get; set; }
    void NavigateTo<T>() where T : ObservableObject;
}
