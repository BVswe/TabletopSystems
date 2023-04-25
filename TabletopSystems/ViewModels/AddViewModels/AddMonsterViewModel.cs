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
    public class AddMonsterViewModel : ObservableObject
    {
        private MainWindowViewModel _mainWinViewModel;
        private UserConnection _userConnection;
        private TTRPGMonster _monster;
        private MonsterRepository _monsterRepo;
        private List<TTRPGTag> _allTags;
        private ObservableCollection<TTRPGTag> _tagsToAdd;
        private List<TTRPGGear> _allGear;
        private ObservableCollection<TTRPGGear> _gearToAdd;
        private List<TTRPGCapability> _allCapabilities;
        private ObservableCollection<TTRPGCapability> _capabilitiesToAdd;
        private Dictionary<TTRPGAttribute, ObservableInt> _attributes;
        
        public string MonsterName
        { 
          get { return _monster.MonsterName; } 
          set { _monster.MonsterName = value; OnPropertyChanged(); } 
        }
        public int StandardDamage
        {
            get { return _monster.StandardDamage; }
            set { _monster.StandardDamage = value; OnPropertyChanged(); }
        }
        public int HP
        {
            get { return _monster.HP; }
            set { _monster.HP = value; OnPropertyChanged(); }
        }
        public List<TTRPGGear> AllGear
        {
            get { return _allGear; }
            set { _allGear = value; OnPropertyChanged(); }
        }
        public List<TTRPGTag> AllTags
        {
            get { return _allTags; }
            set { _allTags = value; OnPropertyChanged(); }
        }
        public List<TTRPGCapability> AllCapabilities
        {
            get { return _allCapabilities; }
            set { _allCapabilities = value; OnPropertyChanged(); }
        }
        public ObservableCollection<TTRPGGear> GearToAdd
        {
            get { return _gearToAdd; }
            set { _gearToAdd = value; OnPropertyChanged(); }
        }
        public ObservableCollection<TTRPGTag> TagsToAdd
        {
            get { return _tagsToAdd; }
            set { _tagsToAdd = value; OnPropertyChanged(); }
        }
        public ObservableCollection<TTRPGCapability> CapabilitiesToAdd
        {
            get { return _capabilitiesToAdd; }
            set { _capabilitiesToAdd = value; OnPropertyChanged(); }
        }
        public Dictionary<TTRPGAttribute, ObservableInt> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; OnPropertyChanged(); }
        }

        public ICommand AddMonsterCommand { get; set; }
        public ICommand AddToTagListCommand { get; set; }
        public ICommand RemoveFromTagListCommand { get; set; }
        public ICommand AddToGearListCommand { get; set; }
        public ICommand RemoveFromGearListCommand { get; set; }
        public ICommand AddToCapabilityListCommand { get; set; }
        public ICommand RemoveFromCapabilityListCommand { get; set; }
        public AddMonsterViewModel(UserConnection conn, MainWindowViewModel mainWinVM)
        {
            _userConnection = conn;
            _mainWinViewModel = mainWinVM;
            _gearToAdd = new ObservableCollection<TTRPGGear>();
            _monster = new TTRPGMonster();
            _tagsToAdd = new ObservableCollection<TTRPGTag>();
            _capabilitiesToAdd = new ObservableCollection<TTRPGCapability>();
            _attributes = new Dictionary<TTRPGAttribute, ObservableInt>();
            _monsterRepo = new MonsterRepository(_userConnection);
            _monster.SystemID = _mainWinViewModel.TbltopSys.SystemID;

            GearRepository tempGearRepo = new GearRepository(_userConnection);
            TagRepository tempTagRepo = new TagRepository(_userConnection);
            AttributesRepository tempAttrRepo = new AttributesRepository();
            CapabilityRepository tempCapaRepo = new CapabilityRepository(_userConnection);
            Dictionary<TTRPGAttribute, ObservableInt> tempDictionary = new Dictionary<TTRPGAttribute, ObservableInt>();

            _allGear = new List<TTRPGGear>(tempGearRepo.GetGear(_mainWinViewModel.TbltopSys.SystemID));
            _allTags = new List<TTRPGTag>(tempTagRepo.GetTags(_mainWinViewModel.TbltopSys.SystemID));
            _allCapabilities = new List<TTRPGCapability>(tempCapaRepo.GetTTRPGCapabilities(_mainWinViewModel.TbltopSys.SystemID));

            foreach (TTRPGAttribute attr in tempAttrRepo.GetTTRPGAttributes(_userConnection, _mainWinViewModel.TbltopSys.SystemID))
            {
                tempDictionary.Add(attr, new ObservableInt());
            }
            //Using property to trigger OnPropertyChanged()
            Attributes = tempDictionary;

            #region Commands
            AddMonsterCommand = new RelayCommand(o =>
            {
                foreach (KeyValuePair<TTRPGAttribute, ObservableInt> kvp in _attributes)
                {
                    _monster.Attributes.Add(kvp.Key, kvp.Value.IntValue);
                }
                foreach (TTRPGTag tag in TagsToAdd)
                {
                    _monster.Tags.Add(tag);
                }
                foreach(TTRPGCapability capa in CapabilitiesToAdd)
                {
                    _monster.Capabilities.Add(capa);
                }
                foreach(TTRPGGear gear in GearToAdd)
                {
                    _monster.Gear.Add(gear);
                }
                //Add to database
                _monsterRepo.Add(_monster);

                #region Reset to default
                MonsterName = string.Empty;
                HP = 0;
                StandardDamage = 0;

                TagsToAdd.Clear();
                CapabilitiesToAdd.Clear();
                GearToAdd.Clear();

                _monster.Tags.Clear();
                _monster.Capabilities.Clear();
                _monster.Gear.Clear();
                _monster.Attributes.Clear();
                
                foreach (KeyValuePair<TTRPGAttribute, ObservableInt> kvp in _attributes)
                {
                    kvp.Value.IntValue = 0;
                }
                #endregion
            });

            AddToTagListCommand = new RelayCommand(o => {
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

            AddToGearListCommand = new RelayCommand(o =>
            {
                if (o == null || (o as TTRPGGear) == null)
                {
                    return;
                }
                if (GearToAdd.Contains((TTRPGGear)o))
                {
                    return;
                }
                GearToAdd.Add((TTRPGGear)o);
            });

            RemoveFromGearListCommand = new RelayCommand(o =>
            {
                if (o == null || (o as TTRPGGear) == null)
                {
                    return;
                }
                GearToAdd.Remove((TTRPGGear)o);
            });

            AddToCapabilityListCommand = new RelayCommand(o =>
            {
                if (o == null || (o as TTRPGCapability) == null)
                {
                    return;
                }
                if (CapabilitiesToAdd.Contains((TTRPGCapability)o))
                {
                    return;
                }
                CapabilitiesToAdd.Add((TTRPGCapability)o);
            });

            RemoveFromCapabilityListCommand = new RelayCommand(o =>
            {
                if (o == null || (o as TTRPGCapability) == null)
                {
                    return;
                }
                CapabilitiesToAdd.Remove((TTRPGCapability)o);
            });
            #endregion

        }

        public void ExecuteAddToTagListCommand(TTRPGTag tag)
        {

        }
    }
}
