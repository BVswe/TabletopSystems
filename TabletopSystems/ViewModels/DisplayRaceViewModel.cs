using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TabletopSystems.Database_Access;
using TabletopSystems.Helper_Methods;
using TabletopSystems.Models;

namespace TabletopSystems.ViewModels
{
    public class DisplayRaceViewModel : ObservableObject, IViewVM
    {
        private TTRPGRace _race;
        private UserConnection _userConnection;
        private ObservableCollection<CapabilityAndLevel> _capabilities;
        private ObservableCollection<SubraceStats> _subraces;
        private ObservableCollection<TTRPGTag> _tags;

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
        public ObservableCollection<CapabilityAndLevel> Capabilities
        {
            get { return _capabilities; }
            set { _capabilities = value; OnPropertyChanged(); }
        }
        public ObservableCollection<SubraceStats> Subraces
        {
            get { return _subraces; }
            set { _subraces = value; OnPropertyChanged(); }
        }
        public ObservableCollection<TTRPGTag> Tags
        {
            get { return _tags; }
            set { _tags = value; OnPropertyChanged(); }
        }

        public string CategoryName
        {
            get { return "Race"; }
        }

        public DisplayRaceViewModel(UserConnection conn)
        {
            _userConnection = conn;
            _race = new TTRPGRace();
            _capabilities = new ObservableCollection<CapabilityAndLevel>();
            _subraces = new ObservableCollection<SubraceStats>();
            _tags = new ObservableCollection<TTRPGTag>();
        }

        public void GetItem(string itemName, int systemID)
        {
            RaceRepository rr = new RaceRepository(_userConnection);
            _race = rr.SearchRace(itemName, systemID);
            Tags = new ObservableCollection<TTRPGTag>(_race.Tags);
            Subraces = new ObservableCollection<SubraceStats>(_race.SubraceAttributes);
            foreach (KeyValuePair<TTRPGCapability, int> kvp in _race.Capabilities)
            {
                Capabilities.Add(new CapabilityAndLevel(kvp.Key, kvp.Value));
            }
        }
    }
}
