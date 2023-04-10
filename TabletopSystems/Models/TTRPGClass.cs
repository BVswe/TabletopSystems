using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopSystems.Models
{
    public class TTRPGClass : IEquatable<TTRPGClass>
    {
        public int SystemID { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public Dictionary<TTRPGCapability, int> Capabilities { get; set; }
        public Dictionary<TTRPGAttribute, int> Attributes { get; set; }
        public TTRPGClass()
        {
            ClassName = string.Empty;
            ClassDescription = string.Empty;
            Attributes = new Dictionary<TTRPGAttribute, int>();
            Capabilities = new Dictionary<TTRPGCapability, int>();
        }
        public bool Equals(TTRPGClass? other)
        {
            if (other is null) return false;
            return this.ClassName == other.ClassName && this.SystemID == other.SystemID;
        }
    }
}
