
using System.Collections.Generic;

namespace TabletopSystems;

public interface IDBAccess
{
    void add(object objectToAdd);
    void remove(object objectToRemove);
    void edit(object objectToRemove);
    object GetByPrimaryKey(object objectToGet);

}
