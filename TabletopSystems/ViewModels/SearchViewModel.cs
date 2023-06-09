﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using TabletopSystems.Database_Access;
using TabletopSystems.Helper_Methods;
using TabletopSystems.Models;
using TabletopTags.Database_Access;
using TabletopSystems.Factories;
using System.Windows;

namespace TabletopSystems.ViewModels
{
    public class SearchViewModel : ObservableObject
    {
        private UserConnection _userConnection;
        private MainWindowViewModel _mainWinViewModel;
        private Dictionary<string, ObservableBool> _categories;
        private Dictionary<string, ObservableBool> _tags;
        private DisplayItemViewFactory _itemFactory;
        private DisplayItemViewModel _itemToDisplay;
        private DataTable _searchResults;
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
        public DataTable SearchResults
        {
            get { return _searchResults; }
            set { _searchResults = value; OnPropertyChanged(); }
        }
        public DisplayItemViewModel ItemToDisplay
        {
            get { return _itemToDisplay; }
            set { _itemToDisplay = value; OnPropertyChanged(); }
        }

        //Header is to set the name of this tab in the view
        public string Header { get; }
        public ICommand SearchCommand { get; set; }
        public ICommand DisplayItemCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }

        public SearchViewModel(UserConnection conn, MainWindowViewModel mainWinVM, DisplayItemViewFactory itemFactory)
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
            _searchResults = new DataTable();
            _itemFactory = itemFactory;

            TagRepository tempTagRepo = new TagRepository(_userConnection);
            foreach(TTRPGTag tag in tempTagRepo.GetTags(_mainWinViewModel.TbltopSys.SystemID))
            {
                _tags.Add(tag.TagName, new ObservableBool());
            }
            SearchCommand = new RelayCommand(o => {
                string query = BuildQuery();
                SearchRepository searchRepo = new SearchRepository(_userConnection);
                //Trace.WriteLine(query);
                SearchResults = searchRepo.SearchDatabase(query, _searchTerm, _tags);
            });
            DisplayItemCommand = new RelayCommand(o =>
            {
                if (o as DataRowView == null)
                {
                    return;
                }
                _itemToDisplay = _itemFactory.GetViewModel(((DataRowView)o)["Category"].ToString()!);
                _itemToDisplay.ViewModel.GetItem(((DataRowView)o)["Name"].ToString()!, _mainWinViewModel.TbltopSys.SystemID);
            });
            DeleteItemCommand = new RelayCommand(o =>
            {
                if (o as DataRowView == null)
                {
                    return;
                }
                if (MessageBox.Show("Are you sure you want to delete " + ((DataRowView)o)["Name"].ToString() + "?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }
                switch (((DataRowView)o)["Category"].ToString()) {
                    case "Capability":
                        CapabilityRepository cr = new CapabilityRepository(_userConnection);
                        cr.Delete(((DataRowView)o)["Name"].ToString()!, _mainWinViewModel.TbltopSys.SystemID);
                        break;
                    case "Gear":
                        GearRepository gr = new GearRepository(_userConnection);
                        gr.Delete(((DataRowView)o)["Name"].ToString()!, _mainWinViewModel.TbltopSys.SystemID);
                        break;
                    case "Monster":
                        MonsterRepository mr = new MonsterRepository(_userConnection);
                        mr.Delete(((DataRowView)o)["Name"].ToString()!, _mainWinViewModel.TbltopSys.SystemID);
                        break;
                    case "Class":
                        ClassRepository clr = new ClassRepository(_userConnection);
                        clr.Delete(((DataRowView)o)["Name"].ToString()!, _mainWinViewModel.TbltopSys.SystemID);
                        break;
                    case "Race":
                        RaceRepository rr = new RaceRepository(_userConnection);
                        rr.Delete(((DataRowView)o)["Name"].ToString()!, _mainWinViewModel.TbltopSys.SystemID);
                        break;
                    default:
                        throw new Exception("Program has enocuntered an error. Attempted to open an invalid item");
                }
                SearchCommand.Execute(null);
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
                    returnString += $" WHERE {name} LIKE @searchTerm";
                }
                else
                {
                    returnString = " WHERE 1=1";
                }
                if (tags.Count > 0)
                {
                    for (int i = 0; i < tags.Count; i++)
                    {
                        returnString += $" AND EXISTS (SELECT {name} FROM {tagTable} WHERE {tagTable}.{name} = {table}.{name} AND {tagTable}.TagName = @tag{i})";
                    }
                }
                return returnString;
            }

            string query = string.Empty;
            string capaQuery;
            string classQuery;
            string gearQuery;
            string monsterQuery;
            string raceQuery;
            if (_userConnection.connectedToSqlServer)
            {
                #region SQL Queries with no WHERE clause
                capaQuery = "SELECT CapabilityName as [Name], 'Capability' as [Category]," +
                    " Coalesce((SELECT STRING_AGG(TagName, ',') as [Tags] FROM Capabilities_Tags WHERE CapabilityName=Capabilities.CapabilityName),'') as [Tags]" +
                    " FROM Capabilities";
                classQuery = "SELECT ClassName as [Name], 'Class' as [Category], '' as [Tags]" +
                    " FROM Classes";
                gearQuery = "SELECT GearName as [Name], 'Gear' as [Category]," +
                    " Coalesce((SELECT STRING_AGG(TagName, ',') as [Tags] FROM Gear_Tags WHERE GearName=Gear.GearName),'') as [Tags]" +
                    " FROM Gear";
                monsterQuery = "SELECT MonsterName as [Name], 'Monster' as [Category]," +
                    " Coalesce((SELECT STRING_AGG(TagName, ',') as [Tags] FROM Monsters_Tags WHERE MonsterName=Monsters.MonsterName),'') as [Tags]" +
                    " FROM Monsters";
                raceQuery = "SELECT RaceName as [Name], 'Race' as [Category]," +
                    " Coalesce((SELECT STRING_AGG(TagName, ',') as [Tags] FROM Races_Tags WHERE RaceName=Races.RaceName), '') as [Tags]" +
                    " FROM Races";
                #endregion
            }
            else
            {
                //SQLITE queries go here
                #region SQLite Queries with no WHERE clause
                capaQuery = "SELECT CapabilityName as [Name], 'Capability' as [Category],"+
                    " Coalesce((SELECT group_concat(TagName) FROM(SELECT TagName FROM Capabilities_Tags WHERE Capabilities.CapabilityName = Capabilities_Tags.CapabilityName ORDER BY TagName)), '') as [Tags]" +
                    " FROM Capabilities";
                classQuery = "SELECT ClassName as [Name], 'Class' as [Category], '' as [Tags]" +
                " FROM Classes";
                gearQuery = "SELECT GearName as [Name], 'Gear' as [Category]," +
                    " Coalesce((SELECT group_concat(TagName) FROM(SELECT TagName FROM Gear_Tags WHERE Gear.GearName = Gear_Tags.GearName ORDER BY TagName)), '') as [Tags]" +
                    " FROM Gear";
                monsterQuery = "SELECT MonsterName as [Name], 'Monster' as [Category]," +
                    " Coalesce((SELECT group_concat(TagName) FROM(SELECT TagName FROM Monsters_Tags WHERE Monsters.MonsterName = Monsters_Tags.MonsterName ORDER BY TagName)), '') as [Tags]" +
                    " FROM Monsters";
                raceQuery = "SELECT RaceName as [Name], 'Race' as [Category]," +
                    " Coalesce((SELECT group_concat(TagName) FROM(SELECT TagName FROM Races_Tags WHERE Races.RaceName = Races_Tags.RaceName ORDER BY TagName)), '') as [Tags]" +
                    " FROM Races";
                #endregion
            }

            List<string> tagsToSearch = new List<string>();
            bool firstQuery = true;
            bool noCategoryFilter = true;
            foreach (KeyValuePair<string, ObservableBool> kvp in _tags)
            {
                if (kvp.Value.BoolValue)
                {
                    tagsToSearch.Add(kvp.Key);
                }
            }
            foreach (KeyValuePair<string, ObservableBool> kvp in _categories)
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
                                if (!String.IsNullOrEmpty(query))
                                {
                                    classQuery += " WHERE ClassName LIKE @searchTerm";
                                }
                                query += " UNION " + classQuery;
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(query))
                                {
                                    classQuery += " WHERE ClassName LIKE @searchTerm";
                                }
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
                    if (!String.IsNullOrEmpty(query))
                    {
                        classQuery += " WHERE ClassName LIKE @searchTerm";
                    }
                    query += " UNION " + classQuery;
                }
                query += " UNION " + gearQuery + formatter("Gear", "GearName", "Gear_Tags", tagsToSearch);
                query += " UNION " + monsterQuery + formatter("Monsters", "MonsterName", "Monsters_Tags", tagsToSearch);
                query += " UNION " + raceQuery + formatter("Races", "RaceName", "Races_Tags", tagsToSearch);
            }
            return query;
        }
    }
}
