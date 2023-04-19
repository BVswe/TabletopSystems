using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using TabletopSystems.Helper_Methods;

namespace TabletopSystems.ViewModels
{
    public class AddItemMainViewModel : ObservableObject
    {
        //Header is to set the name of this tab in the view
        public string Header { get;}
        private ObservableCollection<ObservableObject> _viewModels;
        private UserConnection _userConnection;
        private INavigationService _addItemNavi;

        public ObservableCollection<ObservableObject> ViewModels
        {
            get { return _viewModels; }
        }
        public INavigationService AddItemNavi
        {
            get { return _addItemNavi; }
            set
            {
                _addItemNavi = value;
                OnPropertyChanged();
            }
        }
        public ICommand ChangeViewModel { get; set; }
        public AddItemMainViewModel(UserConnection conn, INavigationService navi)
        {
            Header = "Add To System";
            _addItemNavi = navi;
            _userConnection = conn;
            ChangeViewModel = new RelayCommand(o =>
            {
                if (o == null)
                {
                    return;
                }
                if ((o as string) != null)
                {
                    switch ((string)o){
                        case "Capability":
                            AddItemNavi.NavigateTo<AddCapabilityViewModel>();
                            break;
                        case "Gear":
                            AddItemNavi.NavigateTo<AddGearViewModel>();
                            break;
                        case "Class":
                            AddItemNavi.NavigateTo<AddClassViewModel>();
                            break;
                        case "Monster":
                            AddItemNavi.NavigateTo<AddMonsterViewModel>();
                            break;
                        case "Race":
                            AddItemNavi.NavigateTo<AddRaceViewModel>();
                            break;
                        case "Tag":
                            AddItemNavi.NavigateTo<AddTagViewModel>();
                            break;
                    }
                }
            });
        }
    }
}
