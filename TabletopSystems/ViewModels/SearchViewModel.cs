using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using TabletopSystems.Helper_Methods;
using TabletopSystems.Models;
using TabletopTags.Database_Access;

namespace TabletopSystems.ViewModels
{
    public class SearchViewModel : ObservableObject
    {
        private UserConnection _userConnection;
        private MainWindowViewModel _mainWinViewModel;
        private Dictionary<string, ObservableBool> _categories;
        private Dictionary<string, ObservableBool> _tags;
        private string _searchTerm;

        public Dictionary<string, ObservableBool> Categories
        {
            get { return _categories; }
            set { _categories = value; OnPropertyChanged(); }
        }
        public Dictionary<string, ObservableBool> Tags
        {
            get { return _tags; }
            set { _tags = value; OnPropertyChanged(); }
        }
        public String SearchTerm
        {
            get { return _searchTerm; }
            set { _searchTerm = value; OnPropertyChanged(); }
        }

        //Header is to set the name of this tab in the view
        public string Header { get; }
        public ICommand SearchCommand { get; set; }
        
        public SearchViewModel(UserConnection conn, MainWindowViewModel mainWinVM)
        {
            Header = "Search";
            _userConnection = conn;
            _mainWinViewModel = mainWinVM;
            _searchTerm = string.Empty;
            _categories = new Dictionary<string, ObservableBool>();
            _tags = new Dictionary<string, ObservableBool>();
            _categories.Add("Capability", new ObservableBool());
            _categories.Add("Class", new ObservableBool());
            _categories.Add("Gear", new ObservableBool());
            _categories.Add("Monster", new ObservableBool());
            _categories.Add("Race", new ObservableBool());

            TagRepository tempTagRepo = new TagRepository(_userConnection);
            foreach(TTRPGTag tag in tempTagRepo.GetTags(_mainWinViewModel.TbltopSys.SystemID))
            {
                _tags.Add(tag.TagName, new ObservableBool());
            }
            SearchCommand = new RelayCommand(o => {
                string query = BuildQuery();
                Trace.WriteLine(query);
            });
        }

        public string BuildQuery()
        {
            //Local function to format WHERE portion of SQL strings
            string formatter(string table, string name, string tagTable, List<string> tags)
            {
                string returnString = "";
                if (!String.IsNullOrEmpty(_searchTerm))
                {
                    returnString += $" WHERE {name}=@searchTerm";
                }
                else
                {
                    returnString = " WHERE 1=1";
                }
                if (tags.Count > 0)
                {
                    foreach (string s in tags)
                    {
                        returnString += $" AND EXISTS (SELECT {name} FROM {tagTable} WHERE {tagTable}.{name} = {table}.{name} AND {tagTable}.TagName = @{s})";
                    }
                }
                return returnString;
            }

            string query = string.Empty;
            if (_userConnection.connectedToSqlServer)
            {
                #region Queries with no WHERE clause
                string capaQuery = "SELECT CapabilityName as [Name], 'Capability' as [Category]," +
                    " Coalesce((SELECT STRING_AGG(TagName, ',') as [Tags] FROM Capabilities_Tags WHERE CapabilityName=Capabilities.CapabilityName),'') as [Tags]" +
                    " FROM Capabilities";
                string classQuery = "SELECT ClassName as [Name], 'Class' as [Category], '' as [Tags]" +
                    " FROM Classes";
                string gearQuery = "SELECT GearName as [Name], 'Gear' as [Category]," +
                    " Coalesce((SELECT STRING_AGG(TagName, ',') as [Tags] FROM Gear_Tags WHERE GearName=Gear.GearName),'') as [Tags]" +
                    " FROM Gear";
                string monsterQuery = "SELECT MonsterName as [Name], 'Monster' as [Category]," +
                    " Coalesce((SELECT STRING_AGG(TagName, ',') as [Tags] FROM Monsters_Tags WHERE MonsterName=Monsters.MonsterName),'') as [Tags]" +
                    " FROM Monsters";
                string raceQuery = "SELECT RaceName as [Name], 'Race' as [Category]," +
                    " Coalesce((SELECT STRING_AGG(TagName, ',') as [Tags] FROM Races_Tags WHERE RaceName=Races.RaceName), '') as [Tags]" +
                    " FROM Races";
                #endregion

                List<string> tagsToSearch = new List<string>();
                bool firstQuery = true;
                bool noCategoryFilter = true;
                foreach(KeyValuePair<string, ObservableBool> kvp in _tags)
                {
                    if (kvp.Value.BoolValue)
                    {
                        tagsToSearch.Add(kvp.Key);
                    }
                }
                foreach(KeyValuePair<string, ObservableBool> kvp in _categories)
                {
                    //Check if filter is enabled
                    if (kvp.Value.BoolValue)
                    {
                        noCategoryFilter = false;
                        //Add where clause taking filters into account
                        switch (kvp.Key)
                        {
                            case ("Capability"):
                                capaQuery += formatter("Capabilities", "CapabilityName", "Capabilities_Tags", tagsToSearch);
                                if (!firstQuery)
                                {
                                    query += " UNION " + capaQuery;
                                }
                                else
                                {
                                    query = capaQuery;
                                    firstQuery = false;
                                }
                                break;
                            case ("Class"):
                                if (tagsToSearch.Count > 0)
                                {
                                    break;
                                }
                                if (!firstQuery)
                                {
                                    query += " UNION " + classQuery;
                                }
                                else
                                {
                                    query = classQuery;
                                    firstQuery = false;
                                }
                                break;
                            case ("Gear"):
                                gearQuery += formatter("Gear", "GearName", "Gear_Tags", tagsToSearch);
                                if (!firstQuery)
                                {
                                    query += " UNION " + gearQuery;
                                }
                                else
                                {
                                    query = gearQuery;
                                    firstQuery = false;
                                }
                                break;
                            case ("Monster"):
                                monsterQuery += formatter("Monsters", "MonsterName", "Monsters_Tags", tagsToSearch);
                                if (!firstQuery)
                                {
                                    query += " UNION " + monsterQuery;
                                }
                                else
                                {
                                    query = monsterQuery;
                                    firstQuery = false;
                                }
                                break;
                            case ("Race"):
                                raceQuery += formatter("Races", "RaceName", "Races_Tags", tagsToSearch);
                                if (!firstQuery)
                                {
                                    query += " UNION " + raceQuery;
                                }
                                else
                                {
                                    query = raceQuery;
                                    firstQuery = false;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (noCategoryFilter)
                {
                    query = capaQuery + formatter("Capabilities", "CapabilityName", "Capabilities_Tags", tagsToSearch);
                    if (tagsToSearch.Count == 0)
                    {
                        query += " UNION " + classQuery;
                    }
                    query += " UNION " + gearQuery + formatter("Gear", "GearName", "Gear_Tags", tagsToSearch);
                    query += " UNION " + monsterQuery + formatter("Monsters", "MonsterName", "Monsters_Tags", tagsToSearch);
                    query += " UNION " + raceQuery + formatter("Races", "RaceName", "Races_Tags", tagsToSearch);
                }
                return query;
            }
            else
            {

            }
            return query;
        }
    }
}
