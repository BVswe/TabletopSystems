using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public class AddClassViewModel : ObservableObject
    {
        private MainWindowViewModel _mainWinViewModel;
        private UserConnection _userConnection;
        private TTRPGClass _class;
        private List<TTRPGCapability> _allCapabilities;
        private ObservableCollection<CapabilityAndLevel> _capabilitiesToAdd;
        private Dictionary<TTRPGAttribute, ObservableInt> _attributes;

        public string ClassName
        {
            get { return _class.ClassName; }
            set { _class.ClassName = value; OnPropertyChanged(); }
        }
        public string ClassDescription
        {
            get { return _class.ClassDescription; }
            set { _class.ClassDescription = value; OnPropertyChanged(); }
        }
        public List<TTRPGCapability> AllCapabilities
        {
            get { return _allCapabilities; }
            set { _allCapabilities = value; OnPropertyChanged(); }
        }
        public ObservableCollection<CapabilityAndLevel> CapabilitiesToAdd
        {
            get { return _capabilitiesToAdd; }
            set { _capabilitiesToAdd = value; OnPropertyChanged(); }
        }
        public Dictionary<TTRPGAttribute, ObservableInt> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; OnPropertyChanged(); }
        }

        public ICommand AddClassCommand { get; set; }
        public ICommand AddToCapabilityListCommand { get; set; }
        public ICommand RemoveFromCapabilityListCommand { get; set; }
        public AddClassViewModel(UserConnection conn, MainWindowViewModel mainWinVM)
        {
            _userConnection = conn;
            _mainWinViewModel = mainWinVM;
            _class = new TTRPGClass();
            _attributes = new Dictionary<TTRPGAttribute, ObservableInt>();
            _class.SystemID = _mainWinViewModel.TbltopSys.SystemID;
            _capabilitiesToAdd = new ObservableCollection<CapabilityAndLevel>();

            TagRepository tempTagRepo = new TagRepository(_userConnection);
            AttributesRepository tempAttrRepo = new AttributesRepository();
            CapabilityRepository tempCapaRepo = new CapabilityRepository(_userConnection);
            Dictionary<TTRPGAttribute, ObservableInt> tempDictionary = new Dictionary<TTRPGAttribute, ObservableInt>();

            _allCapabilities = new List<TTRPGCapability>(tempCapaRepo.GetTTRPGCapabilities(_mainWinViewModel.TbltopSys.SystemID));

            foreach (TTRPGAttribute attr in tempAttrRepo.GetTTRPGAttributes(_userConnection, _mainWinViewModel.TbltopSys.SystemID))
            {
                tempDictionary.Add(attr, new ObservableInt());
            }
            //Using property to trigger OnPropertyChanged()
            Attributes = tempDictionary;

            #region Commands
            AddClassCommand = new RelayCommand(o =>
            {
                foreach (KeyValuePair<TTRPGAttribute, ObservableInt> kvp in _attributes)
                {
                    if (kvp.Value.IntValue != 0)
                    {
                        _class.Attributes.Add(kvp.Key, kvp.Value.IntValue);
                    }
                }
                foreach (CapabilityAndLevel capa in _capabilitiesToAdd)
                {
                    _class.Capabilities.Add(capa.Capability, capa.Level);
                }

                //Add to database
                ClassRepository cr = new ClassRepository(_userConnection);
                cr.Add(_class);

                #region Reset to default
                ClassName = string.Empty;
                ClassDescription = string.Empty;
                CapabilitiesToAdd.Clear();
                _class.Attributes.Clear();
                _class.Capabilities.Clear();
                foreach (KeyValuePair<TTRPGAttribute, ObservableInt> kvp in _attributes)
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
            #endregion
        }
    }
}
