
using System.Collections.Generic;

namespace TabletopSystems;

public interface IDBAccess
{
    void add(string objectName,object objectToAdd);
    void remove(string objectName, object objectToRemove);
    void edit(string objectName, object objectToRemove);
    object GetByPrimaryKey(string objectName,object objectToGet);

}
