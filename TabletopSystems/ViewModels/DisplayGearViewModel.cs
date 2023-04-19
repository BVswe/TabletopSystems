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
        private ObservableCollection<TTRPGAttribute> _attributes;
        private Dictionary<string, int> _attributeBonuses;

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
        }
        public ObservableCollection<TTRPGAttribute> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; OnPropertyChanged(); }
        }
        public Dictionary<string, int> AttributeBonuses
        {
            get { return _attributeBonuses; }
            set { _attributeBonuses = value; OnPropertyChanged(); }
        }

        public string CategoryName
        {
            get { return "Gear"; }
        }

        public void GetItem(string itemName, int systemID)
        {
            GearRepository gr = new GearRepository(_userConnection);
            
        }
    }
}
