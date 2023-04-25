using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopSystems.Database_Access;
using TabletopSystems.Helper_Methods;
using TabletopSystems.Models;

namespace TabletopSystems.ViewModels
{
    public class DisplayMonsterViewModel : ObservableObject, IViewVM
    {
        private TTRPGMonster _monster;
        private UserConnection _userConnection;
        private ObservableCollection<TTRPGCapability> _capabilities;
        private Dictionary<TTRPGAttribute, ObservableInt> _attributes;
        private ObservableCollection<TTRPGGear> _gear;
        private ObservableCollection<TTRPGTag> _tags;
        
        
        public string MonsterName
        {
            get { return _monster.MonsterName; }
            set { _monster.MonsterName = value; OnPropertyChanged(); }
        }
        public int MonsterHP
        {
            get { return _monster.HP; }
            set { _monster.HP = value; OnPropertyChanged(); }
        }
        public int MonsterDamage
        {
            get { return _monster.StandardDamage; }
            set { _monster.StandardDamage = value; OnPropertyChanged(); }
        }
        public ObservableCollection<TTRPGCapability> Capabilities
        {
            get { return _capabilities; }
            set { _capabilities = value; OnPropertyChanged(); }
        }
        public Dictionary<TTRPGAttribute, ObservableInt> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; OnPropertyChanged(); }
        }
        public ObservableCollection<TTRPGGear> Gear
        {
            get { return _gear; }
            set { _gear = value; OnPropertyChanged(); }
        }
        public ObservableCollection<TTRPGTag> Tags
        {
            get { return _tags; }
            set { _tags = value; OnPropertyChanged(); }
        }
        

        public string CategoryName
        {
            get { return "Monster"; }
        }
        public DisplayMonsterViewModel(UserConnection conn)
        {
            _userConnection = conn;
            _monster = new TTRPGMonster();
            _capabilities = new ObservableCollection<TTRPGCapability>();
            _attributes = new Dictionary<TTRPGAttribute, ObservableInt>();
            _gear = new ObservableCollection<TTRPGGear>();
            _tags = new ObservableCollection<TTRPGTag>();
        }

        public void GetItem(string itemName, int systemID)
        {
            MonsterRepository mr = new MonsterRepository(_userConnection);
            _monster = mr.SearchMonster(itemName, systemID);
            Capabilities = new ObservableCollection<TTRPGCapability>(_monster.Capabilities);
            Dictionary<TTRPGAttribute, ObservableInt> tempDictionary = new Dictionary<TTRPGAttribute, ObservableInt>();
            foreach (KeyValuePair<TTRPGAttribute, int> kvp in _monster.Attributes)
            {
                tempDictionary.Add(kvp.Key, new ObservableInt(kvp.Value));
            }
            Attributes = tempDictionary;
            Gear = new ObservableCollection<TTRPGGear>(_monster.Gear);
            Tags = new ObservableCollection<TTRPGTag>(_monster.Tags);

        }
    }
}
