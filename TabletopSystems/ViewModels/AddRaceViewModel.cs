using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using TabletopSystems.Database_Access;
using TabletopSystems.Helper_Methods;
using TabletopSystems.Models;
using TabletopTags.Database_Access;

namespace TabletopSystems.ViewModels
{
    public class AddRaceViewModel : ObservableObject
    {
        private MainWindowViewModel _mainWinViewModel;
        private UserConnection _userConnection;
        private TTRPGRace _race;
        private List<TTRPGCapability> _allCapabilities;
        private List<TTRPGTag> _allTags;
        private ObservableCollection<CapabilityAndLevel> _capabilitiesToAdd;
        private ObservableCollection<SubraceStats> _subraces;
        private ObservableCollection<TTRPGTag> _tagsToAdd;

        public string RaceName
        {
            get { return _race.RaceName; }
            set { _race.RaceName = value; OnPropertyChanged(); }
        }
        public string RaceDescription
        {
            get { return _race.RaceDescription; }
            set { _race.RaceDescription = value; OnPropertyChanged(); }
        }
        public List<TTRPGCapability> AllCapabilities
        {
            get { return _allCapabilities; }
            set { _allCapabilities = value; OnPropertyChanged(); }
        }
        public List<TTRPGTag> AllTags
        {
            get { return _allTags; }
            set { _allTags = value; OnPropertyChanged(); }
        }
        public ObservableCollection<CapabilityAndLevel> CapabilitiesToAdd
        {
            get { return _capabilitiesToAdd; }
            set { _capabilitiesToAdd = value; OnPropertyChanged(); }
        }
        public ObservableCollection<TTRPGTag> TagsToAdd
        {
            get { return _tagsToAdd; }
            set { _tagsToAdd = value; OnPropertyChanged(); }
        }
        public ObservableCollection<SubraceStats> Subraces
        {
            get { return _subraces; }
            set { _subraces = value; OnPropertyChanged(); }
        }

        public ICommand AddRaceCommand { get; set; }
        public ICommand AddToCapabilityListCommand { get; set; }
        public ICommand RemoveFromCapabilityListCommand { get; set; }
        public ICommand AddToTagListCommand { get; set; }
        public ICommand RemoveFromTagListCommand { get; set; }
        public ICommand AddSubraceCommand { get; set; }
        public ICommand RemoveSubraceCommand { get; set; }

        public AddRaceViewModel(UserConnection conn, MainWindowViewModel MainWinVM)
        {
            _userConnection = conn;
            _mainWinViewModel = MainWinVM;
            _allCapabilities = new List<TTRPGCapability>();
            _capabilitiesToAdd = new ObservableCollection<CapabilityAndLevel>();
            _subraces = new ObservableCollection<SubraceStats>();
            _tagsToAdd = new ObservableCollection<TTRPGTag>();
            _race = new TTRPGRace();
            _race.SystemID = _mainWinViewModel.TbltopSys.SystemID;

            TagRepository tempTagRepo = new TagRepository(_userConnection);
            AttributesRepository tempAttrRepo = new AttributesRepository();
            CapabilityRepository tempCapaRepo = new CapabilityRepository(_userConnection);
            Dictionary<TTRPGAttribute, ObservableInt> tempDictionary = new Dictionary<TTRPGAttribute, ObservableInt>();

            _allTags = new List<TTRPGTag>(tempTagRepo.GetTags(_mainWinViewModel.TbltopSys.SystemID));
            _allCapabilities = new List<TTRPGCapability>(tempCapaRepo.GetTTRPGCapabilities(_mainWinViewModel.TbltopSys.SystemID));

            foreach (TTRPGAttribute attr in tempAttrRepo.GetTTRPGAttributes(_userConnection, _mainWinViewModel.TbltopSys.SystemID))
            {
                tempDictionary.Add(attr, new ObservableInt());
            }
            _subraces.Add(new SubraceStats("", tempDictionary));

            #region Commands
            AddRaceCommand = new RelayCommand(o =>
            {
                foreach(SubraceStats sub in _subraces)
                {
                    _race.SubraceAttributes.Add(sub);
                }
                foreach (CapabilityAndLevel capa in _capabilitiesToAdd)
                {
                    _race.Capabilities.Add(capa.Capability, capa.Level);
                }
                foreach(TTRPGTag tag in  _tagsToAdd)
                {
                    _race.Tags.Add(tag);
                }

                RaceRepository rr = new RaceRepository(_userConnection);
                rr.Add(_race);

                #region Reset to default
                RaceName = string.Empty;
                RaceDescription = string.Empty;
                _race.SubraceAttributes.Clear();
                _race.Capabilities.Clear();
                _race.Tags.Clear();

                CapabilitiesToAdd.Clear();
                TagsToAdd.Clear();
                if (Subraces.Count > 1)
                {
                    for (int i = 1; i < Subraces.Count; i++)
                    {
                        Subraces.RemoveAt(i);
                    }
                }
                Subraces[0].SubraceName = "";
                foreach(KeyValuePair<TTRPGAttribute, ObservableInt> kvp in Subraces[0].SubraceAttributes)
                {
                    kvp.Value.IntValue = 0;
                }
                #endregion
            });
            AddToCapabilityListCommand = new RelayCommand(o =>
            {
                if (o == null || (o as TTRPGCapability) == null)
                {
                    return;
                }
                CapabilityAndLevel capaAndLevel = new CapabilityAndLevel((TTRPGCapability)o);
                if (CapabilitiesToAdd.Contains(capaAndLevel))
                {
                    return;
                }
                CapabilitiesToAdd.Add(capaAndLevel);
            });

            RemoveFromCapabilityListCommand = new RelayCommand(o =>
            {
                if (o == null || (o as CapabilityAndLevel) == null)
                {
                    return;
                }
                CapabilitiesToAdd.Remove((CapabilityAndLevel)o);
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

            AddSubraceCommand = new RelayCommand(o => {
                _subraces.Add(new SubraceStats("", CreateNewAttrDict(_subraces[0].SubraceAttributes)));
            });

            RemoveSubraceCommand = new RelayCommand(o => {
                if ((o as SubraceStats) == null)
                {
                    return;
                }
                _subraces.Remove((SubraceStats)o);
            });
            #endregion
        }

        /// <summary>
        /// Creates a dictionary of TTRPGAttribute and ObservableInt using attributes from an existing dictionary
        /// </summary>
        /// <param name="origDict"></param>
        /// <returns></returns>
        public Dictionary<TTRPGAttribute, ObservableInt> CreateNewAttrDict(Dictionary<TTRPGAttribute, ObservableInt> origDict)
        {
            //For attributes, it doesn't matter that a the reference is the same because attributes stay the same across the system - the ObservableInt is what matters
            Dictionary<TTRPGAttribute, ObservableInt> copyDict = new Dictionary<TTRPGAttribute, ObservableInt>();
            foreach (KeyValuePair<TTRPGAttribute, ObservableInt> kvp in origDict)
            {
                copyDict.Add(kvp.Key, new ObservableInt());
            }
            return copyDict;
        }
    }
}
