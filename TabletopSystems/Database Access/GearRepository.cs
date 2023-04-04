using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopSystems.Models;

namespace TabletopSystems.Database_Access
{
    public class GearRepository
    {
        private UserConnection _userConnection;
        public GearRepository(UserConnection conn)
        {
            _userConnection = conn;
        }

        public void Add(TTRPGGear gear)
        {
            string addToGear = "INSERT INTO Gear(SystemID,GearName,Description)";
            string attachTags;
        }
    }
}
