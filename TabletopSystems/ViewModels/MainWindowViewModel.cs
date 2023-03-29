using System.Diagnostics;
using TabletopSystems.Helper_Methods;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Windows;
using TabletopSystems.Database_Access;
using Microsoft.Data.SqlClient;
using System;
using Microsoft.Data.Sqlite;

namespace TabletopSystems.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    #region Fields and Properties
    private UserConnection _connection;
    
    private INavigationService _navi;
    //tie back button to a bool
    public INavigationService Navi
    {
        get { return _navi; }
        set
        {
            _navi = value;
            OnPropertyChanged();
        }
    }
    
    public string connection
    {
        get
        {
            if (_connection.connectedToSqlServer)
            {
                return _connection.sqlString;
            }
            else{
                return _connection.sqliteString;
            }
        }
        set
        {
            _connection.sqlString = "Data Source=" + value + "; Initial Catalog=TabletopSystems; Integrated Security=true; Encrypt=false ;Connection Timeout=3";
            OnPropertyChanged();
        }
    }
    public TabletopSystem TbltopSys { get; set; }
    public RelayCommand NavigateSystemMainPageCommand { get; set; }
    public RelayCommand BackCommand { get; set; }
    #endregion
    public MainWindowViewModel(UserConnection conn, INavigationService navi)
    {
        _connection = conn;
        _navi = navi;
        TbltopSys = new TabletopSystem();
        BackCommand = new RelayCommand(o => { ExecuteBackCommand(); }, o => true);
        CreateDB();
        Trace.WriteLine("MainWindowView was constructed!");
    }
    /// <summary>
    /// Go back to System Selection view
    /// </summary>
    public void ExecuteBackCommand()
    {
        MessageBoxResult messageBoxResult = MessageBox.Show("Go back to system selection?", "", System.Windows.MessageBoxButton.YesNo);
        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }
        Navi.NavigateTo<SystemSelectionViewModel>();
        GC.Collect();
    }

    private void CreateDB()
    {
        //Create DB in SQL Server
        if (_connection.connectedToSqlServer)
        {
            using (SqlConnection conn = new SqlConnection(_connection.sqlString))
            {
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.tables WHERE table_name='Systems'";
                    if (Convert.ToInt32(cmd.ExecuteNonQuery()) == 1)
                    {
                        return;
                    }
                }
                using (var transaction = conn.BeginTransaction())
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = transaction;
                        try
                        {
                            #region Table Creation Strings
                            string systemCreation = "CREATE TABLE Systems(\r\nSystemID INT IDENTITY(1,1),\r\nSystemName VARCHAR(100) NOT NULL UNIQUE,\r\nPRIMARY KEY (SystemID)\r\n)";
                            string actionsCreation = "CREATE TABLE Actions(\r\nSystemID INT FOREIGN KEY REFERENCES Systems(SystemID),\r\nActionName Varchar(30),\r\nActionFormula VARCHAR(100),\r\nPRIMARY KEY (SystemID, ActionName)\r\n)";
                            string attributesCreation = "CREATE TABLE Attributes(\r\nSystemID INT FOREIGN KEY REFERENCES Systems(SystemID),\r\nAttributeName Varchar(30),\r\nAttributeFormula VARCHAR(100),\r\nPRIMARY KEY (SystemID, AttributeName)\r\n)";
                            string charactersCreation = "CREATE TABLE Characters(\r\nSystemID INT FOREIGN KEY REFERENCES Systems(SystemID),\r\nCharacterName VARCHAR(50),\r\nCharacterLevel INT,\r\nPRIMARY KEY (SystemID, CharacterName)\r\n)";
                            string gearCreation = "CREATE TABLE Gear(\r\nSystemID INT FOREIGN KEY REFERENCES Systems(SystemID),\r\nGearName VARCHAR(50),\r\nGearDescription VARCHAR(1000)\r\nPRIMARY KEY (SystemID, GearName)\r\n)";
                            string monstersCreation = "CREATE TABLE Monsters(\r\nSystemID INT FOREIGN KEY REFERENCES Systems(SystemID),\r\nMonsterName VARCHAR(50),\r\nStandardDamage INT,\r\nPRIMARY KEY (SystemID, MonsterName)\r\n)";
                            string capabilitiesCreation = "CREATE TABLE Capabilities(\r\nSystemID INT FOREIGN KEY REFERENCES Systems(SystemID),\r\nCapabilityName VARCHAR(100),\r\nCapabilityDescription VARCHAR(2000),\r\nCapabilityArea VARCHAR(20),\r\nCapabilityRange INT,\r\nCapabilityUseTime VARCHAR(30),\r\nCapabilityCost VARCHAR(30),\r\nPRIMARY KEY (SystemID, CapabilityName)\r\n)";
                            string classesCreation = "CREATE TABLE Classes(\r\nSystemID INT FOREIGN KEY REFERENCES Systems(SystemID),\r\nClassName VARCHAR(50),\r\nPRIMARY KEY (SystemID, ClassName)\r\n)";
                            string tagsCreation = "CREATE TABLE Tags(\r\nSystemID INT FOREIGN KEY REFERENCES Systems(SystemID),\r\nTagName VARCHAR(30)\r\nPRIMARY KEY (SystemID, TagName)\r\n)";
                            string racesCreation = "CREATE TABLE Races(\r\nSystemID INT FOREIGN KEY REFERENCES Systems(SystemID),\r\nRaceName VARCHAR(50),\r\nPRIMARY KEY (SystemID, RaceName)\r\n)";
                            string attr_capaCreation = "CREATE TABLE Attributes_Capabilities(\r\nAttributes_SystemID INT,\r\nAttributeName VARCHAR(30),\r\nCapabilities_SystemID INT,\r\nCapabilityName VARCHAR(100),\r\nPRIMARY KEY (Attributes_SystemID, AttributeName, Capabilities_SystemID, CapabilityName),\r\nCONSTRAINT Attr_Capa_Attributes FOREIGN KEY (Attributes_SystemID, AttributeName) REFERENCES Attributes(SystemID, AttributeName),\r\nCONSTRAINT Attr_Capa_Capabilites FOREIGN KEY (Capabilities_SystemID, CapabilityName) REFERENCES Capabilities(SystemID, CapabilityName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string atr_charCreation = "CREATE TABLE Attributes_Characters(\r\nAttributes_SystemID INT,\r\nAttributeName VARCHAR(30),\r\nCharacters_SystemID INT,\r\nCharacterName VARCHAR(50),\r\nAttributeValue INT,\r\nPRIMARY KEY (Attributes_SystemID, AttributeName, Characters_SystemID, CharacterName),\r\nCONSTRAINT Attr_Char_Attributes FOREIGN KEY (Attributes_SystemID, AttributeName) REFERENCES Attributes(SystemID, AttributeName),\r\nCONSTRAINT Attr_Char_Characters FOREIGN KEY (Characters_SystemID, CharacterName) REFERENCES Characters(SystemID, CharacterName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string atr_gearCreation = "CREATE TABLE Attributes_Gear(\r\nAttributes_SystemID INT,\r\nAttributeName VARCHAR(30),\r\nGear_SystemID INT,\r\nGearName VARCHAR(50),\r\nAttributeValue INT,\r\nPRIMARY KEY (Attributes_SystemID, AttributeName, Gear_SystemID, GearName),\r\nCONSTRAINT Attr_Gear_Attributes FOREIGN KEY (Attributes_SystemID, AttributeName) REFERENCES Attributes(SystemID, AttributeName),\r\nCONSTRAINT Attr_Gear_Gear FOREIGN KEY (Gear_SystemID, GearName) REFERENCES Gear(SystemID, GearName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string atr_monstersCreation = "CREATE TABLE Attributes_Monsters(\r\nAttributes_SystemID INT,\r\nAttributeName VARCHAR(30),\r\nMonsters_SystemID INT,\r\nMonsterName VARCHAR(50),\r\nAttributeValue INT,\r\nPRIMARY KEY (Attributes_SystemID, AttributeName, Monsters_SystemID, MonsterName),\r\nCONSTRAINT Attr_Monst_Attributes FOREIGN KEY (Attributes_SystemID, AttributeName) REFERENCES Attributes(SystemID, AttributeName),\r\nCONSTRAINT Attr_Monst_Monsters FOREIGN KEY (Monsters_SystemID, MonsterName) REFERENCES Monsters(SystemID, MonsterName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string atr_classesCreation = "CREATE TABLE Attributes_Classes(\r\nAttributes_SystemID INT,\r\nAttributeName VARCHAR(30),\r\nClasses_SystemID INT,\r\nClassName VARCHAR(50),\r\nAttributeValue INT,\r\nSubclass BIT,\r\nPRIMARY KEY (Attributes_SystemID, AttributeName, Classes_SystemID, ClassName),\r\nCONSTRAINT Attr_Classes_Attributes FOREIGN KEY (Attributes_SystemID, AttributeName) REFERENCES Attributes(SystemID, AttributeName),\r\nCONSTRAINT Attr_Classes_Classes FOREIGN KEY (Classes_SystemID, ClassName) REFERENCES Classes(SystemID, ClassName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string atr_racesCreation = "CREATE TABLE Attributes_Races(\r\nAttributes_SystemID INT,\r\nAttributeName VARCHAR(30),\r\nRaces_SystemID INT,\r\nRaceName VARCHAR(50),\r\nAttributeValue INT,\r\nPRIMARY KEY (Attributes_SystemID, AttributeName, Races_SystemID, RaceName),\r\nCONSTRAINT Attr_Races_Attributes FOREIGN KEY (Attributes_SystemID, AttributeName) REFERENCES Attributes(SystemID, AttributeName),\r\nCONSTRAINT Attr_Races_Races FOREIGN KEY (Races_SystemID, RaceName) REFERENCES Races(SystemID, RaceName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string char_capaCreation = "CREATE TABLE Characters_Capabilities(\r\nCharacters_SystemID INT,\r\nCharacterName VARCHAR(50),\r\nCapabilities_SystemID INT,\r\nCapabilityName VARCHAR(100),\r\nPRIMARY KEY (Characters_SystemID, CharacterName, Capabilities_SystemID, CapabilityName),\r\nCONSTRAINT Char_Capa_Characters FOREIGN KEY (Characters_SystemID, CharacterName) REFERENCES Characters(SystemID, CharacterName),\r\nCONSTRAINT Char_Capa_Capabilites FOREIGN KEY (Capabilities_SystemID, CapabilityName) REFERENCES Capabilities(SystemID, CapabilityName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string char_classesCreation = "CREATE TABLE Characters_Classes(\r\nCharacters_SystemID INT,\r\nCharacterName VARCHAR(50),\r\nClasses_SystemID INT,\r\nClassName VARCHAR(50),\r\nPRIMARY KEY (Characters_SystemID, CharacterName, Classes_SystemID, ClassName),\r\nCONSTRAINT Char_Classes_Characters FOREIGN KEY (Characters_SystemID, CharacterName) REFERENCES Characters(SystemID, CharacterName),\r\nCONSTRAINT Char_Classes_Classes FOREIGN KEY (Classes_SystemID, ClassName) REFERENCES Classes(SystemID, ClassName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string char_racesCreation = "CREATE TABLE Characters_Races(\r\nCharacters_SystemID INT,\r\nCharacterName VARCHAR(50),\r\nRaces_SystemID INT,\r\nRaceName VARCHAR(50),\r\nPRIMARY KEY (Characters_SystemID, CharacterName, Races_SystemID, RaceName),\r\nCONSTRAINT Char_Races_Characters FOREIGN KEY (Characters_SystemID, CharacterName) REFERENCES Characters(SystemID, CharacterName),\r\nCONSTRAINT Char_Races_Races FOREIGN KEY (Races_SystemID, RaceName) REFERENCES Races(SystemID, RaceName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string char_gearCreation = "CREATE TABLE Characters_Gear(\r\nCharacters_SystemID INT,\r\nCharacterName VARCHAR(50),\r\nGear_SystemID INT,\r\nGearName VARCHAR(50),\r\nPRIMARY KEY (Characters_SystemID, CharacterName, Gear_SystemID, GearName),\r\nCONSTRAINT Char_Gear_Characters FOREIGN KEY (Characters_SystemID, CharacterName) REFERENCES Characters(SystemID, CharacterName),\r\nCONSTRAINT Char_Gear_Gear FOREIGN KEY (Gear_SystemID, GearName) REFERENCES Gear(SystemID, GearName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string gear_tagsCreation = "CREATE TABLE Gear_Tags(\r\nGear_SystemID INT,\r\nGearName VARCHAR(50),\r\nTags_SystemID INT,\r\nTagName VARCHAR(30),\r\nPRIMARY KEY (Gear_SystemID, GearName, Tags_SystemID, TagName),\r\nCONSTRAINT Gear_Tags_Gear FOREIGN KEY (Gear_SystemID, GearName) REFERENCES Gear(SystemID, GearName) ON DELETE CASCADE ON UPDATE CASCADE,\r\nCONSTRAINT Gear_Tags_Tags FOREIGN KEY (Tags_SystemID, TagName) REFERENCES Tags(SystemID, TagName)\r\n)";
                            string monsters_tagsCreation = "CREATE TABLE Monsters_Tags(\r\nMonsters_SystemID INT,\r\nMonsterName VARCHAR(50),\r\nTags_SystemID INT,\r\nTagName VARCHAR(30),\r\nPRIMARY KEY (Monsters_SystemID, MonsterName, Tags_SystemID, TagName),\r\nCONSTRAINT Monsters_Tags_Monsters FOREIGN KEY (Monsters_SystemID, MonsterName) REFERENCES Monsters(SystemID, MonsterName) ON DELETE CASCADE ON UPDATE CASCADE,\r\nCONSTRAINT Monsters_Tags_Tags FOREIGN KEY (Tags_SystemID, TagName) REFERENCES Tags(SystemID, TagName)\r\n)";
                            string races_tagsCreation = "CREATE TABLE Races_Tags(\r\nRaces_SystemID INT,\r\nRaceName VARCHAR(50),\r\nTags_SystemID INT,\r\nTagName VARCHAR(30),\r\nPRIMARY KEY (Races_SystemID, RaceName, Tags_SystemID, TagName),\r\nCONSTRAINT Races_Tags_Races FOREIGN KEY (Races_SystemID, RaceName) REFERENCES Races(SystemID, RaceName) ON DELETE CASCADE ON UPDATE CASCADE,\r\nCONSTRAINT Races_Tags_Tags FOREIGN KEY (Tags_SystemID, TagName) REFERENCES Tags(SystemID, TagName)\r\n)";
                            string races_capaCreation = "CREATE TABLE Races_Capabilities(\r\nRaces_SystemID INT,\r\nRaceName VARCHAR(50),\r\nCapabilities_SystemID INT,\r\nCapabilityName VARCHAR(100),\r\nPRIMARY KEY (Races_SystemID, RaceName, Capabilities_SystemID, CapabilityName),\r\nCONSTRAINT Races_Capa_Races FOREIGN KEY (Races_SystemID, RaceName) REFERENCES Races(SystemID, RaceName),\r\nCONSTRAINT Races_Capa_Capabilities FOREIGN KEY (Capabilities_SystemID, CapabilityName) REFERENCES Capabilities(SystemID, CapabilityName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string capa_tagsCreation = "CREATE TABLE Capabilities_Tags(\r\nCapabilities_SystemID INT,\r\nCapabilityName VARCHAR(100),\r\nTags_SystemID INT,\r\nTagName VARCHAR(30),\r\nPRIMARY KEY (Capabilities_SystemID, CapabilityName, Tags_SystemID, TagName),\r\nCONSTRAINT Capabilities_Tags_Capabilities FOREIGN KEY (Capabilities_SystemID, CapabilityName) REFERENCES Capabilities(SystemID, CapabilityName) ON DELETE CASCADE ON UPDATE CASCADE,\r\nCONSTRAINT Capabilities_Tags_Tags FOREIGN KEY (Tags_SystemID, TagName) REFERENCES Tags(SystemID, TagName)\r\n)";
                            string monsters_capaCreation = "CREATE TABLE Monsters_Capabilities(\r\nMonsters_SystemID INT,\r\nMonsterName VARCHAR(50),\r\nCapabilities_SystemID INT,\r\nCapabilityName VARCHAR(100),\r\nPRIMARY KEY (Monsters_SystemID, MonsterName, Capabilities_SystemID, CapabilityName),\r\nCONSTRAINT Monsters_Capa_Monsters FOREIGN KEY (Monsters_SystemID, MonsterName) REFERENCES Monsters(SystemID, MonsterName),\r\nCONSTRAINT Monsters_Capa_Capabilities FOREIGN KEY (Capabilities_SystemID, CapabilityName) REFERENCES Capabilities(SystemID, CapabilityName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string monsters_gearCreation = "CREATE TABLE Monsters_Gear(\r\nMonsters_SystemID INT,\r\nMonsterName VARCHAR(50),\r\nGear_SystemID INT,\r\nGearName VARCHAR(50),\r\nPRIMARY KEY (Monsters_SystemID, MonsterName, Gear_SystemID, GearName),\r\nCONSTRAINT Monsters_Gear_Monsters FOREIGN KEY (Monsters_SystemID, MonsterName) REFERENCES Monsters(SystemID, MonsterName),\r\nCONSTRAINT Monsters_Gear_Capabilities FOREIGN KEY (Gear_SystemID, GearName) REFERENCES Gear(SystemID, GearName)  ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string systemDelTrigger = "CREATE TRIGGER Systems_Delete_Cascade ON Systems\r\nINSTEAD OF DELETE\r\nNOT FOR REPLICATION\r\nAS\r\nBEGIN\r\n\tDELETE FROM Attributes_Capabilities WHERE Attributes_SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Attributes_Characters WHERE Attributes_SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Attributes_Classes WHERE Attributes_SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Attributes_Gear WHERE Attributes_SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Attributes_Monsters WHERE Attributes_SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Attributes_Races WHERE Attributes_SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Capabilities_Tags WHERE Tags_SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Characters_Capabilities WHERE Characters_SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Characters_Classes WHERE Characters_SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Characters_Gear WHERE Characters_SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Characters_Races WHERE Characters_SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Gear_Tags WHERE Gear_SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Monsters_Capabilities WHERE Monsters_SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Monsters_Tags WHERE Monsters_SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Monsters_Gear WHERE Monsters_SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Races_Tags WHERE Races_SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Races_Capabilities WHERE Races_SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Actions WHERE Actions.SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Attributes WHERE Attributes.SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Capabilities WHERE Capabilities.SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Characters WHERE Characters.SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Classes WHERE Classes.SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Gear WHERE Gear.SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Monsters WHERE Monsters.SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Races WHERE Races.SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Tags WHERE Tags.SystemID = (SELECT deleted.SystemID FROM deleted)\r\n\tDELETE FROM Systems WHERE SystemID=(SELECT deleted.SystemID FROM deleted)\r\nEND";
                            string attrDelTrigger = "CREATE TRIGGER Attributes_Delete_Cascade ON Attributes\r\nINSTEAD OF DELETE\r\nNOT FOR REPLICATION\r\nAS\r\nBEGIN\r\n\tDELETE FROM Attributes_Capabilities FROM Attributes_Capabilities JOIN deleted d ON Attributes_Capabilities.AttributeName=d.AttributeName AND Attributes_Capabilities.Attributes_SystemID=d.SystemID\r\n\tDELETE FROM Attributes_Characters FROM Attributes_Characters JOIN deleted d ON Attributes_Characters.AttributeName=d.AttributeName AND Attributes_Characters.Attributes_SystemID=d.SystemID\r\n\tDELETE FROM Attributes_Classes FROM Attributes_Classes JOIN deleted d ON Attributes_Classes.AttributeName=d.AttributeName AND Attributes_Classes.Attributes_SystemID=d.SystemID\r\n\tDELETE FROM Attributes_Gear FROM Attributes_Gear JOIN deleted d ON Attributes_Gear.AttributeName=d.AttributeName AND Attributes_Gear.Attributes_SystemID=d.SystemID\r\n\tDELETE FROM Attributes_Monsters FROM Attributes_Monsters JOIN deleted d ON Attributes_Monsters.AttributeName=d.AttributeName AND Attributes_Monsters.Attributes_SystemID=d.SystemID\r\n\tDELETE FROM Attributes_Races FROM Attributes_Races JOIN deleted d ON Attributes_Races.AttributeName=d.AttributeName AND Attributes_Races.Attributes_SystemID=d.SystemID\r\n\tDELETE FROM Attributes FROM Attributes JOIN deleted d ON Attributes.AttributeName=d.AttributeName AND Attributes.SystemID=d.SystemID\r\nEND";
                            string charDelTrigger = "CREATE TRIGGER Characters_Delete_Cascade ON Characters\r\nINSTEAD OF DELETE\r\nNOT FOR REPLICATION\r\nAS\r\nBEGIN\r\n\tDELETE FROM Characters_Capabilities FROM Characters_Capabilities JOIN deleted d ON Characters_Capabilities.CharacterName=d.CharacterName AND Characters_Capabilities.Characters_SystemID=d.SystemID\r\n\tDELETE FROM Characters_Classes FROM Characters_Classes JOIN deleted d ON Characters_Classes.CharacterName=d.CharacterName AND Characters_Classes.Characters_SystemID=d.SystemID\r\n\tDELETE FROM Characters_Gear FROM Characters_Gear JOIN deleted d ON Characters_Gear.CharacterName=d.CharacterName AND Characters_Gear.Characters_SystemID=d.SystemID\r\n\tDELETE FROM Characters_Races FROM Characters_Races JOIN deleted d ON Characters_Races.CharacterName=d.CharacterName AND Characters_Races.Characters_SystemID=d.SystemID\r\n\tDELETE FROM Characters FROM Characters JOIN deleted d ON Characters.CharacterName=d.CharacterName AND Characters.SystemID=d.SystemID\r\nEND";
                            string tagDelTrigger = "CREATE TRIGGER Tags_Delete_Cascade ON Tags\r\nINSTEAD OF DELETE\r\nNOT FOR REPLICATION\r\nAS\r\nBEGIN\r\n\tDELETE FROM Capabilities_Tags FROM Capabilities_Tags JOIN deleted d ON Capabilities_Tags.TagName=d.TagName AND Capabilities_Tags.Tags_SystemID=d.SystemID\r\n\tDELETE FROM Gear_Tags FROM Gear_Tags JOIN deleted d ON Gear_Tags.TagName=d.TagName AND Gear_Tags.Tags_SystemID=d.SystemID\r\n\tDELETE FROM Monsters_Tags FROM Monsters_Tags JOIN deleted d ON Monsters_Tags.TagName=d.TagName AND Monsters_Tags.Tags_SystemID=d.SystemID\r\n\tDELETE FROM Races_Tags FROM Races_Tags JOIN deleted d ON Races_Tags.TagName=d.TagName AND Races_Tags.Tags_SystemID=d.SystemID\r\n\tDELETE FROM Tags FROM Tags JOIN deleted d ON Tags.TagName=d.TagName AND Tags.SystemID=d.SystemID\r\nEND";
                            string racesDelTrigger = "CREATE TRIGGER Races_Delete_Cascade ON Races\r\nINSTEAD OF DELETE\r\nNOT FOR REPLICATION\r\nAS\r\nBEGIN\r\n\tDELETE FROM Races_Capabilities FROM Races_Capabilities JOIN deleted d ON Races_Capabilities.RaceName=d.RaceName AND Races_Capabilities.Races_SystemID=d.SystemID\r\n\tDELETE FROM Races FROM Races JOIN deleted d ON Races.RaceName=d.RaceName AND Races.SystemID=d.SystemID\r\nEND";
                            string monstersDelTrigger = "CREATE TRIGGER Monsters_Delete_Cascade ON Monsters\r\nINSTEAD OF DELETE\r\nNOT FOR REPLICATION\r\nAS\r\nBEGIN\r\n\tDELETE FROM Monsters_Capabilities FROM Monsters_Capabilities JOIN deleted d ON Monsters_Capabilities.MonsterName=d.MonsterName AND Monsters_Capabilities.Monsters_SystemID=d.SystemID\r\n\tDELETE FROM Monsters_Gear FROM Monsters_Gear JOIN deleted d ON Monsters_Gear.MonsterName=d.MonsterName AND Monsters_Gear.Monsters_SystemID=d.SystemID\r\n\tDELETE FROM Monsters FROM Monsters JOIN deleted d ON Monsters.MonsterName=d.MonsterName AND Monsters.SystemID=d.SystemID\r\nEND";
                            string attrUpdateTrigger = "CREATE TRIGGER Attributes_Update_Cascade ON Attributes\r\nINSTEAD OF UPDATE\r\nAS\r\nBEGIN\r\n\tIF(UPDATE(AttributeName) AND ((SELECT inserted.AttributeName FROM inserted) != (SELECT deleted.AttributeName FROM deleted)))\r\n\t\tBEGIN\r\n\t\t\tINSERT INTO Attributes(SystemID, AttributeName, AttributeFormula) VALUES((SELECT inserted.SystemID FROM inserted),(SELECT inserted.AttributeName FROM inserted),(SELECT inserted.AttributeFormula FROM inserted))\r\n\t\t\tUPDATE Attributes_Capabilities SET AttributeName=(SELECT inserted.AttributeName FROM inserted) WHERE Attributes_SystemID = (SELECT deleted.SystemID FROM deleted) AND AttributeName = (SELECT deleted.AttributeName FROM deleted)\r\n\t\t\tUPDATE Attributes_Characters SET AttributeName=(SELECT inserted.AttributeName FROM inserted) WHERE Attributes_SystemID = (SELECT deleted.SystemID FROM deleted) AND AttributeName = (SELECT deleted.AttributeName FROM deleted)\r\n\t\t\tUPDATE Attributes_Classes SET AttributeName=(SELECT inserted.AttributeName FROM inserted) WHERE Attributes_SystemID = (SELECT deleted.SystemID FROM deleted) AND AttributeName = (SELECT deleted.AttributeName FROM deleted)\r\n\t\t\tUPDATE Attributes_Gear SET AttributeName=(SELECT inserted.AttributeName FROM inserted) WHERE Attributes_SystemID = (SELECT deleted.SystemID FROM deleted) AND AttributeName = (SELECT deleted.AttributeName FROM deleted)\r\n\t\t\tUPDATE Attributes_Monsters SET AttributeName=(SELECT inserted.AttributeName FROM inserted) WHERE Attributes_SystemID = (SELECT deleted.SystemID FROM deleted) AND AttributeName = (SELECT deleted.AttributeName FROM deleted)\r\n\t\t\tUPDATE Attributes_Races SET AttributeName=(SELECT inserted.AttributeName FROM inserted) WHERE Attributes_SystemID = (SELECT deleted.SystemID FROM deleted) AND AttributeName = (SELECT deleted.AttributeName FROM deleted)\r\n\t\t\tDELETE FROM Attributes WHERE SystemID=(SELECT deleted.SystemID FROM deleted) AND AttributeName=(SELECT deleted.AttributeName FROM deleted)\r\n\t\tEND\r\n\tELSE\r\n\t\tBEGIN\r\n\t\tUPDATE Attributes SET AttributeFormula=(SELECT inserted.AttributeFormula FROM inserted) WHERE SystemID=(SELECT deleted.SystemID FROM deleted) AND AttributeName=(SELECT deleted.AttributeName FROM deleted)\r\n\t\tEND\r\nEND";
                            string charUpdateTrigger = "CREATE TRIGGER Characters_Update_Cascade ON Characters\r\nINSTEAD OF UPDATE\r\nAS\r\nBEGIN\r\n\tIF(UPDATE(CharacterName) AND ((SELECT inserted.CharacterName FROM inserted) != (SELECT deleted.CharacterName FROM deleted)))\r\n\t\tBEGIN\r\n\t\t\tINSERT INTO Characters(SystemID,CharacterName,CharacterLevel) VALUES((SELECT SystemID FROM inserted),(SELECT CharacterName FROM inserted),(SELECT CharacterLevel FROM inserted))\r\n\t\t\tUPDATE Characters_Capabilities SET CharacterName=(SELECT inserted.CharacterName FROM inserted) WHERE Characters_SystemID = (SELECT deleted.SystemID FROM deleted) AND CharacterName = (SELECT deleted.CharacterName FROM deleted)\r\n\t\t\tUPDATE Characters_Classes SET CharacterName=(SELECT inserted.CharacterName FROM inserted) WHERE Characters_SystemID = (SELECT deleted.SystemID FROM deleted) AND CharacterName = (SELECT deleted.CharacterName FROM deleted)\r\n\t\t\tUPDATE Characters_Gear SET CharacterName=(SELECT inserted.CharacterName FROM inserted) WHERE Characters_SystemID = (SELECT deleted.SystemID FROM deleted) AND CharacterName = (SELECT deleted.CharacterName FROM deleted)\r\n\t\t\tUPDATE Characters_Races SET CharacterName=(SELECT inserted.CharacterName FROM inserted) WHERE Characters_SystemID = (SELECT deleted.SystemID FROM deleted) AND CharacterName = (SELECT deleted.CharacterName FROM deleted)\r\n\t\t\tUPDATE Attributes_Characters SET CharacterName=(SELECT inserted.CharacterName FROM inserted) WHERE Characters_SystemID = (SELECT deleted.SystemID FROM deleted) AND CharacterName = (SELECT deleted.CharacterName FROM deleted)\r\n\t\t\tDELETE FROM Characters WHERE SystemID=(SELECT deleted.SystemID FROM deleted) AND CharacterName=(SELECT deleted.CharacterName FROM deleted)\r\n\t\tEND\r\n\tELSE\r\n\t\tBEGIN\r\n\t\tUPDATE Characters SET CharacterLevel=(SELECT inserted.CharacterLevel FROM inserted) WHERE SystemID=(SELECT deleted.SystemID FROM deleted) AND CharacterName=(SELECT deleted.CharacterName FROM deleted)\r\n\t\tEND\r\nEND";
                            string tagUpdateTrigger = "CREATE TRIGGER Tags_Update_Cascade ON Tags\r\nINSTEAD OF UPDATE\r\nAS\r\nBEGIN\r\n\tIF(UPDATE(TagName) AND ((SELECT inserted.TagName FROM inserted) != (SELECT deleted.TagName FROM deleted)))\r\n\t\tBEGIN\r\n\t\t\tINSERT INTO Tags(SystemID,TagName) VALUES((SELECT SystemID FROM inserted),(SELECT TagName FROM inserted))\r\n\t\t\tUPDATE Capabilities_Tags SET TagName=(SELECT inserted.TagName FROM inserted) WHERE Tags_SystemID = (SELECT deleted.SystemID FROM deleted) AND TagName=(SELECT deleted.TagName FROM deleted)\r\n\t\t\tUPDATE Gear_Tags SET TagName=(SELECT inserted.TagName FROM inserted) WHERE Gear_SystemID = (SELECT deleted.SystemID FROM deleted) AND TagName=(SELECT deleted.TagName FROM deleted)\r\n\t\t\tUPDATE Monsters_Tags SET TagName=(SELECT inserted.TagName FROM inserted) WHERE Monsters_SystemID = (SELECT deleted.SystemID FROM deleted) AND TagName=(SELECT deleted.TagName FROM deleted)\r\n\t\t\tUPDATE Races_Tags SET TagName=(SELECT inserted.TagName FROM inserted) WHERE Races_SystemID = (SELECT deleted.SystemID FROM deleted) AND TagName=(SELECT deleted.TagName FROM deleted)\r\n\t\t\tDELETE FROM Tags WHERE SystemID=(SELECT deleted.SystemID FROM deleted) AND TagName=(SELECT deleted.TagName FROM deleted)\r\n\t\tEND\r\nEND";
                            string racesUpdateTrigger = "CREATE TRIGGER Races_Update_Cascade ON Races\r\nINSTEAD OF UPDATE\r\nAS\r\nBEGIN\r\n\tIF(UPDATE(RaceName) AND ((SELECT inserted.RaceName FROM inserted) != (SELECT deleted.RaceName FROM deleted)))\r\n\t\tBEGIN\r\n\t\t\tINSERT INTO Races(SystemID,RaceName) VALUES((SELECT SystemID FROM inserted),(SELECT RaceName FROM inserted))\r\n\t\t\tUPDATE Races_Capabilities SET RaceName=(SELECT inserted.RaceName FROM inserted) WHERE Races_SystemID = (SELECT deleted.SystemID FROM deleted) AND RaceName = (SELECT deleted.RaceName FROM deleted)\r\n\t\t\tUPDATE Attributes_Races SET RaceName=(SELECT inserted.RaceName FROM inserted) WHERE Races_SystemID = (SELECT deleted.SystemID FROM deleted) AND RaceName = (SELECT deleted.RaceName FROM deleted)\r\n\t\t\tUPDATE Races_Tags SET RaceName=(SELECT inserted.RaceName FROM inserted) WHERE Races_SystemID = (SELECT deleted.SystemID FROM deleted) AND RaceName = (SELECT deleted.RaceName FROM deleted)\r\n\t\t\tUPDATE Characters_Races SET RaceName=(SELECT inserted.RaceName FROM inserted) WHERE Races_SystemID = (SELECT deleted.SystemID FROM deleted) AND RaceName = (SELECT deleted.RaceName FROM deleted)\r\n\t\t\tDELETE FROM Races WHERE SystemID=(SELECT deleted.SystemID FROM deleted) AND RaceName=(SELECT deleted.RaceName FROM deleted)\r\n\t\tEND\r\nEND";
                            string monstersUpdateTrigger = "CREATE TRIGGER Monsters_Update_Cascade ON Monsters\r\nINSTEAD OF UPDATE\r\nAS\r\nBEGIN\r\n\tIF(UPDATE(MonsterName) AND ((SELECT inserted.MonsterName FROM inserted) != (SELECT deleted.MonsterName FROM deleted)))\r\n\t\tBEGIN\r\n\t\t\tINSERT INTO Monsters(SystemID, MonsterName, StandardDamage) VALUES((SELECT SystemID FROM inserted),(SELECT MonsterName FROM inserted),(SELECT StandardDamage FROM inserted))\r\n\t\t\tUPDATE Monsters_Capabilities SET MonsterName=(SELECT inserted.MonsterName FROM inserted) WHERE Monsters_SystemID = (SELECT deleted.SystemID FROM deleted) AND MonsterName=(SELECT deleted.MonsterName FROM deleted)\r\n\t\t\tUPDATE Monsters_Gear SET MonsterName=(SELECT inserted.MonsterName FROM inserted) WHERE Monsters_SystemID = (SELECT deleted.SystemID FROM deleted) AND MonsterName=(SELECT deleted.MonsterName FROM deleted)\r\n\t\t\tUPDATE Monsters_Tags SET MonsterName=(SELECT inserted.MonsterName FROM inserted) WHERE Monsters_SystemID = (SELECT deleted.SystemID FROM deleted) AND MonsterName=(SELECT deleted.MonsterName FROM deleted)\r\n\t\t\tUPDATE Attributes_Monsters SET MonsterName=(SELECT inserted.MonsterName FROM inserted) WHERE Monsters_SystemID = (SELECT deleted.SystemID FROM deleted) AND MonsterName=(SELECT deleted.MonsterName FROM deleted)\r\n\t\t\tDELETE FROM Monsters WHERE SystemID=(SELECT deleted.SystemID FROM deleted) AND MonsterName=(SELECT deleted.MonsterName FROM deleted)\r\n\t\tEND\r\n\tELSE\r\n\t\tBEGIN\r\n\t\t\tUPDATE Monsters SET StandardDamage=(SELECT inserted.StandardDamage FROM inserted) WHERE SystemID=(SELECT deleted.SystemID FROM deleted) AND MonsterName=(SELECT deleted.MonsterName FROM deleted)\r\n\t\tEND\r\nEND";

                            #endregion
                            #region Table and Trigger Creation
                            cmd.CommandText = systemCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = actionsCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = attributesCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = charactersCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = gearCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = monstersCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = capabilitiesCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = classesCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = tagsCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = racesCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = attr_capaCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = atr_charCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = atr_gearCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = atr_monstersCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = atr_classesCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = atr_racesCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = char_capaCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = char_classesCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = char_racesCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = char_gearCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = gear_tagsCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = monsters_tagsCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = races_tagsCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = races_capaCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = capa_tagsCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = monsters_capaCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = monsters_gearCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = systemDelTrigger;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = attrDelTrigger;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = charDelTrigger;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = tagDelTrigger;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = racesDelTrigger;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = monstersDelTrigger;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = attrUpdateTrigger;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = charUpdateTrigger;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = tagUpdateTrigger;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = racesUpdateTrigger;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = monstersUpdateTrigger;
                            cmd.ExecuteNonQuery();
                            transaction.Commit();
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine("Error creating datatabase.");
                        }
                    }
                }
            }
        }
        //Create DB in SQLite
        else
        {
            using (SqliteConnection conn = new SqliteConnection(_connection.sqliteString))
            {
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    conn.Open();

                    cmd.CommandText = "SELECT COUNT(*) FROM sqlite_master\r\nWHERE type='table'\r\nAND name='Systems';";
                    if (Convert.ToInt32(cmd.ExecuteNonQuery()) == 1)
                    {
                        return;
                    }
                }
                using (var transaction = conn.BeginTransaction())
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        try
                        {
                            #region Table Creation Strings
                            string systemCreation = "CREATE TABLE Systems(\r\nSystemID INTEGER PRIMARY KEY,\r\nSystemName VARCHAR(100) NOT NULL UNIQUE)";
                            string actionsCreation = "CREATE TABLE Actions(\r\nSystemID INT,\r\nActionName Varchar(30),\r\nActionFormula VARCHAR(100),\r\nPRIMARY KEY (SystemID, ActionName),\r\nFOREIGN KEY (SystemID) REFERENCES Systems(SystemID)\r\n)\r\n";
                            string attributesCreation = "CREATE TABLE Attributes(\r\nSystemID INT,\r\nAttributeName Varchar(30),\r\nAttributeFormula VARCHAR(100),\r\nPRIMARY KEY (SystemID, AttributeName),\r\nFOREIGN KEY (SystemID) REFERENCES Systems(SystemID)\r\n)";
                            string charactersCreation = "CREATE TABLE Characters(\r\nSystemID INT,\r\nCharacterName VARCHAR(50),\r\nCharacterLevel INT,\r\nPRIMARY KEY (SystemID, CharacterName),\r\nFOREIGN KEY (SystemID) REFERENCES Systems(SystemID)\r\n)";
                            string gearCreation = "CREATE TABLE Gear(\r\nSystemID INT,\r\nGearName VARCHAR(50),\r\nGearDescription VARCHAR(1000),\r\nPRIMARY KEY (SystemID, GearName),\r\nFOREIGN KEY (SystemID) REFERENCES Systems(SystemID)\r\n)";
                            string monstersCreation = "CREATE TABLE Monsters(\r\nSystemID INT,\r\nMonsterName VARCHAR(50),\r\nStandardDamage INT,\r\nPRIMARY KEY (SystemID, MonsterName),\r\nFOREIGN KEY (SystemID) REFERENCES Systems(SystemID)\r\n)";
                            string capabilitiesCreation = "CREATE TABLE Capabilities(\r\nSystemID INT,\r\nCapabilityName VARCHAR(100),\r\nCapabilityDescription VARCHAR(2000),\r\nCapabilityArea VARCHAR(20),\r\nCapabilityRange INT,\r\nCapabilityUseTime VARCHAR(30),\r\nCapabilityCost VARCHAR(30),\r\nPRIMARY KEY (SystemID, CapabilityName),\r\nFOREIGN KEY (SystemID) REFERENCES Systems(SystemID)\r\n)";
                            string classesCreation = "CREATE TABLE Classes(\r\nSystemID INT,\r\nClassName VARCHAR(50),\r\nPRIMARY KEY (SystemID, ClassName),\r\nFOREIGN KEY (SystemID) REFERENCES Systems(SystemID)\r\n)";
                            string tagsCreation = "CREATE TABLE Tags(\r\nSystemID INT,\r\nTagName VARCHAR(30),\r\nPRIMARY KEY (SystemID, TagName),\r\nFOREIGN KEY (SystemID) REFERENCES Systems(SystemID)\r\n)";
                            string racesCreation = "CREATE TABLE Races(\r\nSystemID INT,\r\nRaceName VARCHAR(50),\r\nPRIMARY KEY (SystemID, RaceName),\r\nFOREIGN KEY (SystemID) REFERENCES Systems(SystemID)\r\n)";
                            string attr_capaCreation = "CREATE TABLE Attributes_Capabilities(\r\nAttributes_SystemID INT,\r\nAttributeName VARCHAR(30),\r\nCapabilities_SystemID INT,\r\nCapabilityName VARCHAR(100),\r\nPRIMARY KEY (Attributes_SystemID, AttributeName, Capabilities_SystemID, CapabilityName),\r\nCONSTRAINT Attr_Capa_Attributes FOREIGN KEY (Attributes_SystemID, AttributeName) REFERENCES Attributes(SystemID, AttributeName),\r\nCONSTRAINT Attr_Capa_Capabilites FOREIGN KEY (Capabilities_SystemID, CapabilityName) REFERENCES Capabilities(SystemID, CapabilityName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string atr_charCreation = "CREATE TABLE Attributes_Characters(\r\nAttributes_SystemID INT,\r\nAttributeName VARCHAR(30),\r\nCharacters_SystemID INT,\r\nCharacterName VARCHAR(50),\r\nAttributeValue INT,\r\nPRIMARY KEY (Attributes_SystemID, AttributeName, Characters_SystemID, CharacterName),\r\nCONSTRAINT Attr_Char_Attributes FOREIGN KEY (Attributes_SystemID, AttributeName) REFERENCES Attributes(SystemID, AttributeName),\r\nCONSTRAINT Attr_Char_Characters FOREIGN KEY (Characters_SystemID, CharacterName) REFERENCES Characters(SystemID, CharacterName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string atr_gearCreation = "CREATE TABLE Attributes_Gear(\r\nAttributes_SystemID INT,\r\nAttributeName VARCHAR(30),\r\nGear_SystemID INT,\r\nGearName VARCHAR(50),\r\nAttributeValue INT,\r\nPRIMARY KEY (Attributes_SystemID, AttributeName, Gear_SystemID, GearName),\r\nCONSTRAINT Attr_Gear_Attributes FOREIGN KEY (Attributes_SystemID, AttributeName) REFERENCES Attributes(SystemID, AttributeName),\r\nCONSTRAINT Attr_Gear_Gear FOREIGN KEY (Gear_SystemID, GearName) REFERENCES Gear(SystemID, GearName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string atr_monstersCreation = "CREATE TABLE Attributes_Monsters(\r\nAttributes_SystemID INT,\r\nAttributeName VARCHAR(30),\r\nMonsters_SystemID INT,\r\nMonsterName VARCHAR(50),\r\nAttributeValue INT,\r\nPRIMARY KEY (Attributes_SystemID, AttributeName, Monsters_SystemID, MonsterName),\r\nCONSTRAINT Attr_Monst_Attributes FOREIGN KEY (Attributes_SystemID, AttributeName) REFERENCES Attributes(SystemID, AttributeName),\r\nCONSTRAINT Attr_Monst_Monsters FOREIGN KEY (Monsters_SystemID, MonsterName) REFERENCES Monsters(SystemID, MonsterName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string atr_classesCreation = "CREATE TABLE Attributes_Classes(\r\nAttributes_SystemID INT,\r\nAttributeName VARCHAR(30),\r\nClasses_SystemID INT,\r\nClassName VARCHAR(50),\r\nAttributeValue INT,\r\nSubclass BIT,\r\nPRIMARY KEY (Attributes_SystemID, AttributeName, Classes_SystemID, ClassName),\r\nCONSTRAINT Attr_Classes_Attributes FOREIGN KEY (Attributes_SystemID, AttributeName) REFERENCES Attributes(SystemID, AttributeName),\r\nCONSTRAINT Attr_Classes_Classes FOREIGN KEY (Classes_SystemID, ClassName) REFERENCES Classes(SystemID, ClassName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string atr_racesCreation = "CREATE TABLE Attributes_Races(\r\nAttributes_SystemID INT,\r\nAttributeName VARCHAR(30),\r\nRaces_SystemID INT,\r\nRaceName VARCHAR(50),\r\nAttributeValue INT,\r\nPRIMARY KEY (Attributes_SystemID, AttributeName, Races_SystemID, RaceName),\r\nCONSTRAINT Attr_Races_Attributes FOREIGN KEY (Attributes_SystemID, AttributeName) REFERENCES Attributes(SystemID, AttributeName),\r\nCONSTRAINT Attr_Races_Races FOREIGN KEY (Races_SystemID, RaceName) REFERENCES Races(SystemID, RaceName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string char_capaCreation = "CREATE TABLE Characters_Capabilities(\r\nCharacters_SystemID INT,\r\nCharacterName VARCHAR(50),\r\nCapabilities_SystemID INT,\r\nCapabilityName VARCHAR(100),\r\nPRIMARY KEY (Characters_SystemID, CharacterName, Capabilities_SystemID, CapabilityName),\r\nCONSTRAINT Char_Capa_Characters FOREIGN KEY (Characters_SystemID, CharacterName) REFERENCES Characters(SystemID, CharacterName),\r\nCONSTRAINT Char_Capa_Capabilites FOREIGN KEY (Capabilities_SystemID, CapabilityName) REFERENCES Capabilities(SystemID, CapabilityName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string char_classesCreation = "CREATE TABLE Characters_Classes(\r\nCharacters_SystemID INT,\r\nCharacterName VARCHAR(50),\r\nClasses_SystemID INT,\r\nClassName VARCHAR(50),\r\nPRIMARY KEY (Characters_SystemID, CharacterName, Classes_SystemID, ClassName),\r\nCONSTRAINT Char_Classes_Characters FOREIGN KEY (Characters_SystemID, CharacterName) REFERENCES Characters(SystemID, CharacterName),\r\nCONSTRAINT Char_Classes_Classes FOREIGN KEY (Classes_SystemID, ClassName) REFERENCES Classes(SystemID, ClassName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string char_racesCreation = "CREATE TABLE Characters_Races(\r\nCharacters_SystemID INT,\r\nCharacterName VARCHAR(50),\r\nRaces_SystemID INT,\r\nRaceName VARCHAR(50),\r\nPRIMARY KEY (Characters_SystemID, CharacterName, Races_SystemID, RaceName),\r\nCONSTRAINT Char_Races_Characters FOREIGN KEY (Characters_SystemID, CharacterName) REFERENCES Characters(SystemID, CharacterName),\r\nCONSTRAINT Char_Races_Races FOREIGN KEY (Races_SystemID, RaceName) REFERENCES Races(SystemID, RaceName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string char_gearCreation = "CREATE TABLE Characters_Gear(\r\nCharacters_SystemID INT,\r\nCharacterName VARCHAR(50),\r\nGear_SystemID INT,\r\nGearName VARCHAR(50),\r\nPRIMARY KEY (Characters_SystemID, CharacterName, Gear_SystemID, GearName),\r\nCONSTRAINT Char_Gear_Characters FOREIGN KEY (Characters_SystemID, CharacterName) REFERENCES Characters(SystemID, CharacterName),\r\nCONSTRAINT Char_Gear_Gear FOREIGN KEY (Gear_SystemID, GearName) REFERENCES Gear(SystemID, GearName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string gear_tagsCreation = "CREATE TABLE Gear_Tags(\r\nGear_SystemID INT,\r\nGearName VARCHAR(50),\r\nTags_SystemID INT,\r\nTagName VARCHAR(30),\r\nPRIMARY KEY (Gear_SystemID, GearName, Tags_SystemID, TagName),\r\nCONSTRAINT Gear_Tags_Gear FOREIGN KEY (Gear_SystemID, GearName) REFERENCES Gear(SystemID, GearName) ON DELETE CASCADE ON UPDATE CASCADE,\r\nCONSTRAINT Gear_Tags_Tags FOREIGN KEY (Tags_SystemID, TagName) REFERENCES Tags(SystemID, TagName)\r\n)";
                            string monsters_tagsCreation = "CREATE TABLE Monsters_Tags(\r\nMonsters_SystemID INT,\r\nMonsterName VARCHAR(50),\r\nTags_SystemID INT,\r\nTagName VARCHAR(30),\r\nPRIMARY KEY (Monsters_SystemID, MonsterName, Tags_SystemID, TagName),\r\nCONSTRAINT Monsters_Tags_Monsters FOREIGN KEY (Monsters_SystemID, MonsterName) REFERENCES Monsters(SystemID, MonsterName) ON DELETE CASCADE ON UPDATE CASCADE,\r\nCONSTRAINT Monsters_Tags_Tags FOREIGN KEY (Tags_SystemID, TagName) REFERENCES Tags(SystemID, TagName)\r\n)";
                            string races_tagsCreation = "CREATE TABLE Races_Tags(\r\nRaces_SystemID INT,\r\nRaceName VARCHAR(50),\r\nTags_SystemID INT,\r\nTagName VARCHAR(30),\r\nPRIMARY KEY (Races_SystemID, RaceName, Tags_SystemID, TagName),\r\nCONSTRAINT Races_Tags_Races FOREIGN KEY (Races_SystemID, RaceName) REFERENCES Races(SystemID, RaceName) ON DELETE CASCADE ON UPDATE CASCADE,\r\nCONSTRAINT Races_Tags_Tags FOREIGN KEY (Tags_SystemID, TagName) REFERENCES Tags(SystemID, TagName)\r\n)";
                            string races_capaCreation = "CREATE TABLE Races_Capabilities(\r\nRaces_SystemID INT,\r\nRaceName VARCHAR(50),\r\nCapabilities_SystemID INT,\r\nCapabilityName VARCHAR(100),\r\nPRIMARY KEY (Races_SystemID, RaceName, Capabilities_SystemID, CapabilityName),\r\nCONSTRAINT Races_Capa_Races FOREIGN KEY (Races_SystemID, RaceName) REFERENCES Races(SystemID, RaceName),\r\nCONSTRAINT Races_Capa_Capabilities FOREIGN KEY (Capabilities_SystemID, CapabilityName) REFERENCES Capabilities(SystemID, CapabilityName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string capa_tagsCreation = "CREATE TABLE Capabilities_Tags(\r\nCapabilities_SystemID INT,\r\nCapabilityName VARCHAR(100),\r\nTags_SystemID INT,\r\nTagName VARCHAR(30),\r\nPRIMARY KEY (Capabilities_SystemID, CapabilityName, Tags_SystemID, TagName),\r\nCONSTRAINT Capabilities_Tags_Capabilities FOREIGN KEY (Capabilities_SystemID, CapabilityName) REFERENCES Capabilities(SystemID, CapabilityName) ON DELETE CASCADE ON UPDATE CASCADE,\r\nCONSTRAINT Capabilities_Tags_Tags FOREIGN KEY (Tags_SystemID, TagName) REFERENCES Tags(SystemID, TagName)\r\n)";
                            string monsters_capaCreation = "CREATE TABLE Monsters_Capabilities(\r\nMonsters_SystemID INT,\r\nMonsterName VARCHAR(50),\r\nCapabilities_SystemID INT,\r\nCapabilityName VARCHAR(100),\r\nPRIMARY KEY (Monsters_SystemID, MonsterName, Capabilities_SystemID, CapabilityName),\r\nCONSTRAINT Monsters_Capa_Monsters FOREIGN KEY (Monsters_SystemID, MonsterName) REFERENCES Monsters(SystemID, MonsterName),\r\nCONSTRAINT Monsters_Capa_Capabilities FOREIGN KEY (Capabilities_SystemID, CapabilityName) REFERENCES Capabilities(SystemID, CapabilityName) ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string monsters_gearCreation = "CREATE TABLE Monsters_Gear(\r\nMonsters_SystemID INT,\r\nMonsterName VARCHAR(50),\r\nGear_SystemID INT,\r\nGearName VARCHAR(50),\r\nPRIMARY KEY (Monsters_SystemID, MonsterName, Gear_SystemID, GearName),\r\nCONSTRAINT Monsters_Gear_Monsters FOREIGN KEY (Monsters_SystemID, MonsterName) REFERENCES Monsters(SystemID, MonsterName),\r\nCONSTRAINT Monsters_Gear_Capabilities FOREIGN KEY (Gear_SystemID, GearName) REFERENCES Gear(SystemID, GearName)  ON DELETE CASCADE ON UPDATE CASCADE\r\n)";
                            string systemDelTrigger = "CREATE TRIGGER Systems_Delete_Cascade BEFORE DELETE ON Systems\r\nBEGIN\r\n\tDELETE FROM Attributes_Capabilities WHERE Attributes_SystemID = OLD.SystemID;\r\n\tDELETE FROM Attributes_Characters WHERE Attributes_SystemID = OLD.SystemID;\r\n\tDELETE FROM Attributes_Classes WHERE Attributes_SystemID = OLD.SystemID;\r\n\tDELETE FROM Attributes_Gear WHERE Attributes_SystemID = OLD.SystemID;\r\n\tDELETE FROM Attributes_Monsters WHERE Attributes_SystemID = OLD.SystemID;\r\n\tDELETE FROM Attributes_Races WHERE Attributes_SystemID = OLD.SystemID;\r\n\tDELETE FROM Capabilities_Tags WHERE Tags_SystemID = OLD.SystemID;\r\n\tDELETE FROM Characters_Capabilities WHERE Characters_SystemID = OLD.SystemID;\r\n\tDELETE FROM Characters_Classes WHERE Characters_SystemID = OLD.SystemID;\r\n\tDELETE FROM Characters_Gear WHERE Characters_SystemID = OLD.SystemID;\r\n\tDELETE FROM Characters_Races WHERE Characters_SystemID = OLD.SystemID;\r\n\tDELETE FROM Gear_Tags WHERE Gear_SystemID = OLD.SystemID;\r\n\tDELETE FROM Monsters_Capabilities WHERE Monsters_SystemID = OLD.SystemID;\r\n\tDELETE FROM Monsters_Tags WHERE Monsters_SystemID = OLD.SystemID;\r\n\tDELETE FROM Monsters_Gear WHERE Monsters_SystemID = OLD.SystemID;\r\n\tDELETE FROM Races_Tags WHERE Races_SystemID = OLD.SystemID;\r\n\tDELETE FROM Races_Capabilities WHERE Races_SystemID = OLD.SystemID;\r\n\tDELETE FROM Actions WHERE Actions.SystemID = OLD.SystemID;\r\n\tDELETE FROM Attributes WHERE Attributes.SystemID = OLD.SystemID;\r\n\tDELETE FROM Capabilities WHERE Capabilities.SystemID = OLD.SystemID;\r\n\tDELETE FROM Characters WHERE Characters.SystemID = OLD.SystemID;\r\n\tDELETE FROM Classes WHERE Classes.SystemID = OLD.SystemID;\r\n\tDELETE FROM Gear WHERE Gear.SystemID = OLD.SystemID;\r\n\tDELETE FROM Monsters WHERE Monsters.SystemID = OLD.SystemID;\r\n\tDELETE FROM Races WHERE Races.SystemID = OLD.SystemID;\r\n\tDELETE FROM Tags WHERE Tags.SystemID = OLD.SystemID;\r\nEND";
                            string attrDelTrigger = "CREATE TRIGGER Attributes_Delete_Cascade BEFORE DELETE ON Attributes\r\nBEGIN\r\n\tDELETE FROM Attributes_Capabilities WHERE AttributeName = OLD.AttributeName AND Attributes_SystemID = OLD.SystemID;\r\n\tDELETE FROM Attributes_Characters WHERE AttributeName = OLD.AttributeName AND Attributes_SystemID = OLD.SystemID;\r\nDELETE FROM Attributes_Classes WHERE AttributeName = OLD.AttributeName AND Attributes_SystemID = OLD.SystemID;\r\n\tDELETE FROM Attributes_Gear WHERE AttributeName = OLD.AttributeName AND Attributes_SystemID = OLD.SystemID;\r\n\tDELETE FROM Attributes_Monsters WHERE AttributeName = OLD.AttributeName AND Attributes_SystemID = OLD.SystemID;\r\n\tDELETE FROM Attributes_Races WHERE AttributeName = OLD.AttributeName AND Attributes_SystemID = OLD.SystemID;\r\nEND;";
                            string charDelTrigger = "CREATE TRIGGER Characters_Delete_Cascade BEFORE DELETE ON Characters\r\nBEGIN\r\n\tDELETE FROM Characters_Capabilities WHERE Characters_Capabilities.CharacterName=OLD.CharacterName AND Characters_Capabilities.Characters_SystemID=OLD.SystemID;\r\n\tDELETE FROM Characters_Classes WHERE Characters_Classes.CharacterName=OLD.CharacterName AND Characters_Classes.Characters_SystemID=OLD.SystemID;\r\n\tDELETE FROM Characters_Gear WHERE Characters_Gear.CharacterName=OLD.CharacterName AND Characters_Gear.Characters_SystemID=OLD.SystemID;\r\n\tDELETE FROM Characters_Races WHERE Characters_Races.CharacterName=OLD.CharacterName AND Characters_Races.Characters_SystemID=OLD.SystemID;\r\nEND";
                            string tagDelTrigger = "CREATE TRIGGER Tags_Delete_Cascade BEFORE DELETE ON Tags\r\nBEGIN\r\n\tDELETE FROM Capabilities_Tags WHERE Capabilities_Tags.TagName=OLD.TagName AND Capabilities_Tags.Tags_SystemID=OLD.SystemID;\r\n\tDELETE FROM Gear_Tags WHERE Gear_Tags.TagName=OLD.TagName AND Gear_Tags.Tags_SystemID=OLD.SystemID;\r\n\tDELETE FROM Monsters_Tags WHERE Monsters_Tags.TagName=OLD.TagName AND Monsters_Tags.Tags_SystemID=OLD.SystemID;\r\n\tDELETE FROM Races_Tags WHERE Races_Tags.TagName=OLD.TagName AND Races_Tags.Tags_SystemID=OLD.SystemID;\r\nEND";
                            string racesDelTrigger = "CREATE TRIGGER Races_Delete_Cascade BEFORE DELETE ON Races\r\nBEGIN\r\n\tDELETE FROM Races_Capabilities WHERE Races_Capabilities.RaceName=OLD.RaceName AND Races_Capabilities.Races_SystemID=OLD.SystemID;\r\nEND;";
                            string monstersDelTrigger = "CREATE TRIGGER Monsters_Delete_Cascade BEFORE DELETE ON Monsters\r\nBEGIN\r\n\tDELETE FROM Monsters_Capabilities WHERE Monsters_Capabilities.MonsterName=OLD.MonsterName AND Monsters_Capabilities.Monsters_SystemID=OLD.SystemID;\r\n\tDELETE FROM Monsters_Gear WHERE Monsters_Gear.MonsterName=OLD.MonsterName AND Monsters_Gear.Monsters_SystemID=OLD.SystemID;\r\nEND;";
                            string attrUpdateTrigger = "CREATE TRIGGER Attributes_Update_Cascade BEFORE UPDATE OF AttributeName ON Attributes\r\nWHEN NEW.AttributeName!=OLD.AttributeName \r\nBEGIN\r\n    INSERT INTO Attributes(SystemID, AttributeName, AttributeFormula) VALUES(NEW.SystemID, NEW.AttributeName, NEW.AttributeFormula);\r\n    UPDATE Attributes_Capabilities SET AttributeName=New.AttributeName WHERE Attributes_SystemID = OLD.SystemID AND AttributeName = OLD.AttributeName;\r\n    UPDATE Attributes_Characters SET AttributeName=New.AttributeName WHERE Attributes_SystemID = OLD.SystemID AND AttributeName = OLD.AttributeName;\r\n    UPDATE Attributes_Classes SET AttributeName=New.AttributeName WHERE Attributes_SystemID = OLD.SystemID AND AttributeName = OLD.AttributeName;\r\n    UPDATE Attributes_Gear SET AttributeName=New.AttributeName WHERE Attributes_SystemID = OLD.SystemID AND AttributeName = OLD.AttributeName;\r\n    UPDATE Attributes_Monsters SET AttributeName=New.AttributeName WHERE Attributes_SystemID = OLD.SystemID AND AttributeName = OLD.AttributeName;\r\n    UPDATE Attributes_Races SET AttributeName=New.AttributeName WHERE Attributes_SystemID = OLD.SystemID AND AttributeName = OLD.AttributeName;\r\n    DELETE FROM Attributes WHERE SystemID=OLD.SystemID AND AttributeName=OLD.AttributeName;\r\nEND;";
                            string charUpdateTrigger = "CREATE TRIGGER Characters_Update_Cascade BEFORE UPDATE OF CharacterName ON Characters\r\nWHEN NEW.CharacterName!=OLD.CharacterName \r\nBEGIN\r\n    INSERT INTO Characters(SystemID, CharacterName, CharacterLevel) VALUES(New.SystemID,NEW.CharacterName,New.CharacterLevel);\r\n    UPDATE Characters_Capabilities SET CharacterName=NEW.CharacterName WHERE Characters_SystemID = OLD.SystemID AND CharacterName = OLD.CharacterName;\r\n    UPDATE Characters_Classes SET CharacterName=NEW.CharacterName WHERE Characters_SystemID = OLD.SystemID AND CharacterName = OLD.CharacterName;\r\n    UPDATE Characters_Gear SET CharacterName=NEW.CharacterName WHERE Characters_SystemID = OLD.SystemID AND CharacterName = OLD.CharacterName;\r\n    UPDATE Characters_Races SET CharacterName=NEW.CharacterName WHERE Characters_SystemID = OLD.SystemID AND CharacterName = OLD.CharacterName;\r\n\tUPDATE Attributes_Characters SET CharacterName=NEW.CharacterName WHERE Characters_SystemID = OLD.SystemID AND CharacterName = OLD.CharacterName;\r\n    DELETE FROM Characters WHERE SystemID = OLD.SystemID AND CharacterName = OLD.CharacterName;\r\nEND;";
                            string tagUpdateTrigger = "CREATE TRIGGER Tags_Update_Cascade BEFORE UPDATE OF TagName ON Tags\r\nWHEN NEW.TagName!=OLD.TagName \r\nBEGIN\r\n    INSERT INTO Tags(TagName,SystemID) VALUES(NEW.TagName, NEW.SystemID);\r\n    UPDATE Capabilities_Tags SET TagName=NEW.TagName WHERE Tags_SystemID = OLD.SystemID AND TagName=OLD.TagName;\r\n    UPDATE Gear_Tags SET TagName=NEW.TagName WHERE Tags_SystemID = OLD.SystemID AND TagName=OLD.TagName;\r\n    UPDATE Monsters_Tags SET TagName=NEW.TagName WHERE Tags_SystemID = OLD.SystemID AND TagName=OLD.TagName;\r\n    UPDATE Races_Tags SET TagName=NEW.TagName WHERE Tags_SystemID = OLD.SystemID AND TagName=OLD.TagName;\r\n    DELETE FROM Tags WHERE SystemID = OLD.SystemID AND TagName=OLD.TagName;\r\nEND;";
                            string racesUpdateTrigger = "CREATE TRIGGER Races_Update_Cascade BEFORE UPDATE OF RaceName ON Races\r\nWHEN NEW.RaceName!=OLD.RaceName \r\nBEGIN\r\n    INSERT INTO Races(SystemID,RaceName) VALUES(NEW.SystemID,NEW.RaceName);\r\n    UPDATE Races_Capabilities SET RaceName=NEW.RaceName WHERE Races_SystemID = OLD.SystemID AND RaceName = OLD.RaceName;\r\n    UPDATE Races_Tags SET RaceName=NEW.RaceName WHERE Races_SystemID = OLD.SystemID AND RaceName = OLD.RaceName;\r\n    UPDATE Attributes_Races SET RaceName=NEW.RaceName WHERE Races_SystemID = OLD.SystemID AND RaceName = OLD.RaceName;\r\n    UPDATE Characters_Races SET RaceName=NEW.RaceName WHERE Races_SystemID = OLD.SystemID AND RaceName = OLD.RaceName;\r\n    DELETE FROM Races WHERE SystemID = OLD.SystemID AND RaceName = OLD.RaceName;\r\nEND;";
                            string monstersUpdateTrigger = "CREATE TRIGGER Monsters_Update_Cascade BEFORE UPDATE OF MonsterName ON Monsters\r\nWHEN NEW.MonsterName!=OLD.MonsterName\r\nBEGIN\r\n    INSERT INTO Monsters(SystemID,MonsterName,StandardDamage) VALUES(NEW.SystemID,NEW.MonsterName,NEW.StandardDamage);\r\n    UPDATE Monsters_Capabilities SET MonsterName=NEW.MonsterName WHERE Monsters_SystemID = OLD.SystemID AND MonsterName = OLD.MonsterName;\r\n    UPDATE Monsters_Gear SET MonsterName=NEW.MonsterName WHERE Monsters_SystemID = OLD.SystemID AND MonsterName=OLD.MonsterName;\r\n    UPDATE Monsters_Tags SET MonsterName=NEW.MonsterName WHERE Monsters_SystemID = OLD.SystemID AND MonsterName=OLD.MonsterName;\r\n    UPDATE Attributes_Monsters SET MonsterName=NEW.MonsterName WHERE Monsters_SystemID = OLD.SystemID AND MonsterName=OLD.MonsterName;\r\n    DELETE FROM Monsters WHERE SystemID = OLD.SystemID AND MonsterName=OLD.MonsterName;\r\nEND;";

                            #endregion
                            #region Table and Trigger Creation
                            cmd.CommandText = systemCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = actionsCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = attributesCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = charactersCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = gearCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = monstersCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = capabilitiesCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = classesCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = tagsCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = racesCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = attr_capaCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = atr_charCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = atr_gearCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = atr_monstersCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = atr_classesCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = atr_racesCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = char_capaCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = char_classesCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = char_racesCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = char_gearCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = gear_tagsCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = monsters_tagsCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = races_tagsCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = races_capaCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = capa_tagsCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = monsters_capaCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = monsters_gearCreation;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = systemDelTrigger;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = attrDelTrigger;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = charDelTrigger;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = tagDelTrigger;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = racesDelTrigger;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = monstersDelTrigger;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = attrUpdateTrigger;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = charUpdateTrigger;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = tagUpdateTrigger;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = racesUpdateTrigger;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = monstersUpdateTrigger;
                            cmd.ExecuteNonQuery();
                            transaction.Commit();
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine("Error creating datatabase.");
                        }
                    }
                }
            }
        }
    }
}
