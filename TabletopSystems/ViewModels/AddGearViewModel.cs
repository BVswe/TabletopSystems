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
using TabletopTags.Database_Access;

namespace TabletopSystems.ViewModels
{
    public class AddGearViewModel : ObservableObject
    {
        //Header is for dropdown to read viewmodel's name
        public string Header { get; }
        private UserConnection _userConnection;
        private List<TTRPGTag> _allTags;
        private ObservableCollection<TTRPGTag> _tagsToAdd;
        private TTRPGGear _gear;
        private Dictionary<TTRPGAttribute, ObservableInt> _attributes;
        private MainWindowViewModel _mainWinViewModel;
        private GearRepository _gearRepository;

        #region Properties
        public string GearName
        { 
            get { return _gear.GearName; }
            set { _gear.GearName = value; OnPropertyChanged(); }
        }
        public string GearDescription
        {
            get { return _gear.Description; }
            set { _gear.Description = value; OnPropertyChanged(); }
        }
        public ObservableCollection<TTRPGTag> TagsToAdd
        {
            get { return _tagsToAdd; }
            set { _tagsToAdd = value; OnPropertyChanged(); }
        }
        public List<TTRPGTag> AllTags
        {
            get { return _allTags; }
            set { _allTags = value; OnPropertyChanged(); }
        }

        //Attributes to choose from and their value (reset after adding)
        public Dictionary<TTRPGAttribute, ObservableInt> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; OnPropertyChanged(); }
        }

        public ICommand AddGearCommand { get; set; }
        public ICommand AddToTagListCommand { get; set; }
        public ICommand RemoveFromTagListCommand { get; set; }
        #endregion
        public AddGearViewModel(UserConnection conn, MainWindowViewModel mainWinVM)
        {
            Header = "Gear";
            _userConnection = conn;
            _mainWinViewModel = mainWinVM;
            _gear = new TTRPGGear();
            _tagsToAdd = new ObservableCollection<TTRPGTag>();
            _attributes = new Dictionary<TTRPGAttribute, ObservableInt>();
            _gearRepository = new GearRepository(_userConnection);
            _gear.SystemID = _mainWinViewModel.TbltopSys.SystemID;

            AttributesRepository tempAttrRepo = new AttributesRepository();
            TagRepository tempTagRepo = new TagRepository(_userConnection);
            Dictionary<TTRPGAttribute, ObservableInt> tempDictionary = new Dictionary<TTRPGAttribute, ObservableInt>();

            _allTags = new List<TTRPGTag>(tempTagRepo.GetTags(_mainWinViewModel.TbltopSys.SystemID));

            foreach (TTRPGAttribute attr in tempAttrRepo.GetTTRPGAttributes(_userConnection, _mainWinViewModel.TbltopSys.SystemID))
            {
                tempDictionary.Add(attr, new ObservableInt());
            }
            //Using property to trigger OnPropertyChanged()
            Attributes = tempDictionary;

            #region Commands
            AddGearCommand = new RelayCommand(o =>
            {
                foreach(KeyValuePair<TTRPGAttribute, ObservableInt> kvp in _attributes)
                {
                    if (kvp.Value.IntValue != 0)
                    {
                        _gear.Attributes.Add(kvp.Key, kvp.Value.IntValue);
                    }
                }
                foreach(TTRPGTag tag in TagsToAdd)
                {
                    _gear.Tags.Add(tag);
                }
                //Add to database
                _gearRepository.Add(_gear);

                #region Reset to default
                foreach (KeyValuePair<TTRPGAttribute, ObservableInt> kvp in _attributes)
                {
                    kvp.Value.IntValue = 0;
                }
                GearName = "";
                GearDescription = "";
                _tagsToAdd.Clear();
                _gear.Tags.Clear();
                _gear.Attributes.Clear();
                #endregion
            });

            AddToTagListCommand = new RelayCommand(o =>
            {
                if (o == null || (o as TTRPGTag) == null)
                {
                    return; 
                }
                if (TagsToAdd.Contains((TTRPGTag)o))
                {
                    return;
                }
                TagsToAdd.Add((TTRPGTag)o);
            });

            RemoveFromTagListCommand = new RelayCommand(o =>
            {
                if (o == null || (o as TTRPGTag) == null)
                {
                    return;
                }
                TagsToAdd.Remove((TTRPGTag)o);
            });
            #endregion
        }
    }
}
