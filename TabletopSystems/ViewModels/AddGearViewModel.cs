using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TabletopSystems.Database_Access;
using TabletopSystems.Helper_Methods;
using TabletopSystems.Models;

namespace TabletopSystems.ViewModels
{
    public class AddGearViewModel : ObservableObject
    {
        //Header is for dropdown to read viewmodel's name
        public string Header { get; }
        private UserConnection _userConnection;
        private List<TTRPGTag> _allTags;
        private TTRPGGear _gear;
        private TTRPGTag _selectedTag;
        private TTRPGTag _removalTag;
        private Dictionary<TTRPGAttribute, ObservableBool> _attributes;
        private MainWindowViewModel _mainWinViewModel;
 
        public List<TTRPGTag> AllTags
        {
            get { return _allTags; }
            set { _allTags = value; OnPropertyChanged(); }
        }
        public TTRPGTag SelectedTag
        {
            get { return _selectedTag; }
            set { _selectedTag = value; OnPropertyChanged(); }
        }
        public TTRPGTag RemovalTag
        {
            get { return _removalTag; }
            set { _removalTag = value; OnPropertyChanged(); }
        }

        public Dictionary<TTRPGAttribute, ObservableBool> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; OnPropertyChanged(); }
        }

        public ICommand AddGearCommand { get; set; }

        public AddGearViewModel(UserConnection conn, MainWindowViewModel mainWinVM)
        {
            Header = "Gear";
            _userConnection = conn;
            _mainWinViewModel = mainWinVM;
            _gear = new TTRPGGear();
            AttributesRepository tempAttrRepo = new AttributesRepository();
            Dictionary<TTRPGAttribute, ObservableBool> tempDictionary = new Dictionary<TTRPGAttribute, ObservableBool>();
            foreach (TTRPGAttribute attr in tempAttrRepo.GetTTRPGAttributes(_userConnection, _mainWinViewModel.TbltopSys.SystemID))
            {
                tempDictionary.Add(attr, new ObservableBool());
            }
            Attributes = tempDictionary;
        }
    }
}
