
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace TabletopSystems;

public class TableCreation
{
    private UserConnection _userConnection;
    public TableCreation(UserConnection u)
    {
        _userConnection = u;
    }

    #region Table Creation Methods
    /// <summary>
    /// Processes SqlCommand to create an Attributes table
    /// </summary>
    /// <param name="cmd">Command to be altered</param>
    /// <param name="systemID">ID of system associated with this table</param>
    /// <returns></returns>
    public void ProcessCreateAttributesCommand(ref SqlCommand cmd, int systemID)
    {
        cmd.CommandText = "CREATE TABLE " + systemID + "_Attributes" + " (" +
                "SystemName varchar(100) NOT NULL," +
                "AttributeName varchar(30) NOT NULL," +
                "StatFormulas varchar(30)," +
                "PRIMARY KEY (SystemName, AttributeName)," +
                "CONSTRAINT @tableFK FOREIGN KEY (SystemName) REFERENCES Systems(SystemName)" +
                ")";
    }

    /// <summary>
    /// Processes SqlCommand to create an Actions table
    /// </summary>
    /// <param name="cmd">Command to be altered</param>
    /// <param name="systemID">ID of system associated with this table</param>
    public void CreateActionsCommand(ref SqlCommand cmd, int systemID)
    {
        cmd.CommandText =  "CREATE TABLE " + systemID + "_Actions" + " (" +
                "SystemID int NOT NULL," +
                "ActionName varchar(30) NOT NULL," +
                "ActionFormula varchar(50) NOT NULL," +
                "PRIMARY KEY (SystemName, ActionName)," +
                "CONSTRAINT FK_" + systemID + "_Actions" + " FOREIGN KEY (SystemID) REFERENCES [Systems](SystemID)" +
                ")";
    }

    //public string CreateGearCommand(List<string> attributes)
    //{

    //}
    //public string CreateCapabilitiesCommand(List<string> attributes)
    //{
    //    string temp = "";
    //    foreach (string attribute in attributes)
    //    {
    //        temp += attribute + " int, ";
    //    }
    //}

    //public string CreateTagsCommand(List<string> attributes)
    //{
    //    string temp = "";
    //    foreach (string attribute in attributes)
    //    {
    //        temp += attribute + " int, ";
    //    }
    //}
    #endregion
}
