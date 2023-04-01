using System.Collections.ObjectModel;


namespace TabletopSystems.ViewModels
{
    public class AddItemMainViewModel : ObservableObject
    {
        //Header is to set the name of this tab in the view
        public string Header { get;}
        private ObservableCollection<ObservableObject> _viewModels;
        private UserConnection _userConnection;
        private string _selectedItemToEnter;
        public string SelectedItemToEnter
        {
            get { return _selectedItemToEnter; }
            set
            {
                _selectedItemToEnter = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ObservableObject> ViewModels
        {
            get { return _viewModels; }
        }
        public AddItemMainViewModel(UserConnection conn, AddCapabilityViewModel addCapaVM, AddGearViewModel addGearVM, AddClassViewModel addClassVM, AddMonsterViewModel addMonsterVM, AddRaceViewModel addRaceVM, AddTagViewModel addTagVM)
        {
            Header = "Add To System";
            _userConnection = conn;
            _viewModels = new ObservableCollection<ObservableObject>()
            {
                addCapaVM,
                addGearVM,
                addClassVM,
                addMonsterVM,
                addRaceVM,
                addTagVM
            };
        }
    }
}
