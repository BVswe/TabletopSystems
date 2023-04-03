using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopSystems.Models
{
    public class TTRPGTag : IEquatable<TTRPGTag>
    {
        public string TagName { get; set; }
        public int SystemID { get; set; }

        public TTRPGTag() {
            TagName = string.Empty;
        }

        public bool Equals(TTRPGTag? other)
        {
            if (other is null) return false;
            return this.TagName == other.TagName && this.SystemID == other.SystemID;
        }
    }
}
