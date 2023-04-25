using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopSystems.Database_Access;
using TabletopSystems.Models;

namespace TabletopSystems.ViewModels
{
    public class DisplayGearViewModel : ObservableObject, IViewVM
    {
        private TTRPGGear _gear;
        private UserConnection _userConnection;
        private ObservableCollection<TTRPGTag> _tags;
        private ObservableCollection<AttributeValueAndBool> _attributeBonuses;
        private ObservableCollection<TTRPGAttribute> _attributesUsed;

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
        public ObservableCollection<TTRPGTag> Tags
        {
            get { return _tags; }
            set { _tags=value; OnPropertyChanged(); }
        }
        public ObservableCollection<AttributeValueAndBool> AttributeBonuses
        {
            get { return _attributeBonuses; }
            set { _attributeBonuses = value; OnPropertyChanged(); }
        }
        public ObservableCollection<TTRPGAttribute> AttributesUsed
        {
            get { return _attributesUsed; }
            set { _attributesUsed = value; OnPropertyChanged(); }
        }
        public string CategoryName
        {
            get { return "Gear"; }
        }

        public DisplayGearViewModel(UserConnection conn)
        {
            _userConnection = conn;
            _gear = new TTRPGGear();
            _tags = new ObservableCollection<TTRPGTag>();
            _attributeBonuses = new ObservableCollection<AttributeValueAndBool>();
            _attributesUsed = new ObservableCollection<TTRPGAttribute>();
        }

        public void GetItem(string itemName, int systemID)
        {
            GearRepository gr = new GearRepository(_userConnection);
            _gear = gr.SearchGear(itemName, systemID);
            Tags = new ObservableCollection<TTRPGTag>(_gear.Tags);
            AttributeBonuses = new ObservableCollection<AttributeValueAndBool>(_gear.Attributes);
            AttributesUsed = new ObservableCollection<TTRPGAttribute>();
            foreach(AttributeValueAndBool attr in AttributeBonuses)
            {
                if (attr.BoolValue)
                {
                    AttributesUsed.Add(attr.Attribute);
                }
            }
        }
    }
}
