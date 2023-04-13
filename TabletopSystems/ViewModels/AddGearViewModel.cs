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
        private UserConnection _userConnection;
        private List<TTRPGTag> _allTags;
        private ObservableCollection<TTRPGTag> _tagsToAdd;
        private TTRPGGear _gear;
        private ObservableCollection<AttributeValueAndBool> _attributes;
        private MainWindowViewModel _mainWinViewModel;

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
        public ObservableCollection<AttributeValueAndBool> Attributes
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
            _userConnection = conn;
            _mainWinViewModel = mainWinVM;
            _gear = new TTRPGGear();
            _tagsToAdd = new ObservableCollection<TTRPGTag>();
            _attributes = new ObservableCollection<AttributeValueAndBool>();
            _gear.SystemID = _mainWinViewModel.TbltopSys.SystemID;

            AttributesRepository tempAttrRepo = new AttributesRepository();
            TagRepository tempTagRepo = new TagRepository(_userConnection);

            _allTags = new List<TTRPGTag>(tempTagRepo.GetTags(_mainWinViewModel.TbltopSys.SystemID));

            foreach (TTRPGAttribute attr in tempAttrRepo.GetTTRPGAttributes(_userConnection, _mainWinViewModel.TbltopSys.SystemID))
            {
                _attributes.Add(new AttributeValueAndBool(attr));
            }
            //Using property to trigger OnPropertyChanged()

            #region Commands
            AddGearCommand = new RelayCommand(o =>
            {
                foreach(AttributeValueAndBool attr in _attributes)
                {
                    if (attr.Value == 0 && attr.BoolValue == false)
                    {
                        continue;
                    }
                    _gear.Attributes.Add(attr);
                }
                foreach(TTRPGTag tag in TagsToAdd)
                {
                    _gear.Tags.Add(tag);
                }
                //Add to database
                GearRepository gr = new GearRepository(_userConnection);
                gr.Add(_gear);

                #region Reset to default
                foreach (AttributeValueAndBool attr in _attributes)
                {
                    attr.Value = 0;
                    attr.BoolValue = false;
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
