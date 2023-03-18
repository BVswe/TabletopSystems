using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.RegularExpressions;
namespace TabletopSystems.Helper_Methods;

public static class Helpers
{
    public static bool validSystemNickname(string s)
    {
        if (!Regex.Match(s, "^[A-Za-z _0-9]+$").Success)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public static string CreateAttributesTableCommand(int systemID)
    {
        return "CREATE TABLE [" + systemID + "_Attributes]" + " (" +
                "SystemID varchar(100) NOT NULL," +
                "AttributeName varchar(30) NOT NULL," +
                "AttributeFormula varchar(30)," +
                "PRIMARY KEY (SystemID, AttributeName)," +
                "CONSTRAINT @tableFK FOREIGN KEY (SystemID) REFERENCES Systems(SystemID)" +
                ")";
    }
    public static string CreateActionsTableCommand(int systemID)
    {
        return "CREATE TABLE [" + systemID + "_Actions]" + " (" +
                "SystemID int NOT NULL," +
                "ActionName varchar(30) NOT NULL," +
                "ActionFormula varchar(50) NOT NULL," +
                "PRIMARY KEY (SystemID, ActionName)," +
                "CONSTRAINT FK_" + systemID + "_Actions" + " FOREIGN KEY (SystemID) REFERENCES [Systems](SystemID)" +
                ")";
    }
}
