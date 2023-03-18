

using System.Collections.Generic;

namespace TabletopSystems.Database_Access;

public interface ITabletopSystemRepository
{
    public void Add(TabletopSystem systemToAdd);
    public void EditSystemName(TabletopSystem systemToAdd);
    public void Delete(TabletopSystem systemToRemove);
    public int GetIDBySystemName(string name);
    public Dictionary<string, int> GetSystems();

}
