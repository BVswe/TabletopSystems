using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopSystems.Helper_Methods;

namespace TabletopSystems.ViewModels
{
    public class TestViewModel : ObservableObject
    {
        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand NavigateToSettingsViewCommand { get; set; }
        public TestViewModel(INavigationService navigation)
        {
            Navigation = navigation;
        }
    }
}
