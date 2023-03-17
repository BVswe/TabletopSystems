
using System.Collections.Generic;

namespace TabletopSystems;

public interface IDBAccess
{
    void add(string objectName,Dictionary<string, string> objectToAdd);
    void remove(string objectName, Dictionary<string, string> objectToRemove);
    void edit(string objectName, Dictionary<string, string> objectToRemove);
    object GetByPrimaryKey(string objectName,Dictionary<string, string> objectToGet);

}
