using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopSystems.Models
{
    public class TTRPGGear
    {
        public int SystemID { get; set; }
        public string GearName { get; set; }
        public string Description { get; set; }
        public List<TTRPGTag> Tags { get; set; }
        public Dictionary<TTRPGAttribute, int> Attributes { get; set; }
        public TTRPGGear()
        {
            GearName = string.Empty;
            Description = string.Empty;
            Attributes = new Dictionary<TTRPGAttribute, int>();
            Tags = new List<TTRPGTag>();
        }
    }
}
