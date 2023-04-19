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
    public class AddCapabilityViewModel : ObservableObject, IEditVM
    {
        //Header is for dropdown to read viewmodel's name
        private readonly MainWindowViewModel _mainWinViewModel;
        private UserConnection _userConnection;
        private TTRPGCapability _capability;
        private TTRPGCapability _oldCapability;
        private ObservableCollection<TTRPGTag> _tagsToAdd;
        private List<TTRPGTag> _allTags;
        private Dictionary<TTRPGAttribute, ObservableBool> _attributes;
        private bool _isEditing;
        #region Properties
        public string CapabilityName
        {
            get { return _capability.CapabilityName; }
            set { _capability.CapabilityName = value; OnPropertyChanged(); }
        }
        public string CapabilityDescription
        {
            get { return _capability.Description; }
            set { _capability.Description = value; OnPropertyChanged(); }
        }
        public string CapabilityArea
        {
            get { return _capability.Area; }
            set { _capability.Area = value; OnPropertyChanged(); }
        }
        public string CapabilityRange
        {
            get { return _capability.Range; }
            set { _capability.Range = value; OnPropertyChanged(); }
        }
        public string CapabilityUseTime
        {
            get { return _capability.UseTime; }
            set { _capability.UseTime = value; OnPropertyChanged(); }
        }
        public string CapabilityCost
        {
            get { return _capability.Cost; }
            set { _capability.Cost = value; OnPropertyChanged(); }
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
        public Dictionary<TTRPGAttribute, ObservableBool> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; OnPropertyChanged(); }
        }
        public bool IsEditing
        {
            get { return _isEditing; }
        }
        public ICommand AddCapabilityCommand { get; }
        public ICommand AddToCapabilityTagsCommand { get; }
        public ICommand RemoveCapabilityTagCommand { get; }
        #endregion
        public AddCapabilityViewModel(UserConnection conn, MainWindowViewModel mainWinViewModel)
        {
            _mainWinViewModel = mainWinViewModel;
            _userConnection = conn;
            
            _capability = new TTRPGCapability();
            _capability.SystemID = _mainWinViewModel.TbltopSys.SystemID;
            _tagsToAdd = new ObservableCollection<TTRPGTag>();
            _attributes = new Dictionary<TTRPGAttribute, ObservableBool>();
            _isEditing = false;

            TagRepository tagRepo = new TagRepository(_userConnection);
            AttributesRepository tempAttrRepo = new AttributesRepository();
            Dictionary<TTRPGAttribute, ObservableBool> tempDictionary = new Dictionary<TTRPGAttribute, ObservableBool>();
            _allTags = new List<TTRPGTag>(tagRepo.GetTags(_mainWinViewModel.TbltopSys.SystemID));

            foreach (TTRPGAttribute attr in tempAttrRepo.GetTTRPGAttributes(_userConnection, _mainWinViewModel.TbltopSys.SystemID))
            {
                tempDictionary.Add(attr, new ObservableBool());
            }
            //Using property to trigger OnPropertyChanged()
            Attributes = tempDictionary;

            #region Commands
            AddCapabilityCommand = new RelayCommand(o =>
            {
                foreach(TTRPGTag tag in _tagsToAdd)
                {
                    _capability.Tags.Add(tag);
                }
                foreach(KeyValuePair<TTRPGAttribute, ObservableBool> attr in Attributes)
                {
                    if (attr.Value.BoolValue == true)
                    {
                        _capability.Attributes.Add(attr.Key);
                    }
                }

                //Add to database
                CapabilityRepository cr = new CapabilityRepository(_userConnection);
                cr.Add(_capability);

                #region Reset to default
                CapabilityName = string.Empty;
                CapabilityDescription = string.Empty;
                CapabilityArea = string.Empty;
                CapabilityRange = string.Empty;
                CapabilityUseTime = string.Empty;
                CapabilityCost = string.Empty;
                TagsToAdd.Clear();
                _capability.Attributes.Clear();
                _capability.Tags.Clear();
                foreach (KeyValuePair<TTRPGAttribute, ObservableBool> attr in Attributes)
                {
                    attr.Value.BoolValue = false;
                }
                #endregion
            });
            AddToCapabilityTagsCommand = new RelayCommand(o =>
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
            RemoveCapabilityTagCommand = new RelayCommand(o =>
            {
                if (o == null || (o as TTRPGTag) == null)
                {
                    return;
                }
                TagsToAdd.Remove((TTRPGTag)o);
            });
            #endregion
        }

        public void FillFromDatabase(string itemName, int systemID)
        {
            CapabilityRepository cr = new CapabilityRepository(_userConnection);
            _oldCapability = cr.SearchCapability(itemName, systemID);
            CapabilityName = _oldCapability.CapabilityName;
            CapabilityDescription = _oldCapability.Description;
            CapabilityArea = _oldCapability.Area;
            CapabilityCost = _oldCapability.Cost;
            CapabilityRange = _oldCapability.Range;
            CapabilityUseTime = _oldCapability.UseTime;
            foreach (TTRPGTag tag in _oldCapability.Tags)
            {
                _capability.Tags.Add(tag);
            }
            foreach (TTRPGAttribute attr in _oldCapability.Attributes)
            {
                if (_oldCapability.Attributes.Contains(attr))
                {
                    Attributes[attr].BoolValue = true;
                }
            }
        }

        public void SetEditing()
        {
            _isEditing = true;
        }
    }
}
