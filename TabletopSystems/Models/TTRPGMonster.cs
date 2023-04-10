using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopSystems.Models
{
    public class TTRPGMonster : IEquatable<TTRPGMonster>
    {
        public int SystemID { get; set; }
        public string MonsterName { get; set; }
        public int StandardDamage { get; set; }
        public List<TTRPGTag> Tags { get; set; }
        public List<TTRPGGear> Gear { get; set; }
        public List<TTRPGCapability> Capabilities { get; set; }
        public Dictionary<TTRPGAttribute, int> Attributes { get; set; }
        public int HP { get; set; }

        public TTRPGMonster()
        {
            MonsterName = string.Empty;
            StandardDamage = 0;
            HP = 0;
            Tags = new List<TTRPGTag>();
            Attributes = new Dictionary<TTRPGAttribute, int>();
            Capabilities = new List<TTRPGCapability>();
            Gear = new List<TTRPGGear>();
        }

        public bool Equals(TTRPGMonster? other)
        {
            if (other is null) return false;
            return this.MonsterName == other.MonsterName && this.SystemID == other.SystemID;
        }
    }
}
