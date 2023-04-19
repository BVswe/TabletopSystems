

using System;

namespace TabletopSystems.Models
{
    public class TTRPGAttribute : IEquatable<TTRPGAttribute>
    {
        public int SystemID { get; set; }
        public string AttributeName { get; set; }
        public string AttributeFormula { get; set; }

        public TTRPGAttribute()
        {
            AttributeName = string.Empty;
            AttributeFormula = string.Empty;
        }

        public bool Equals(TTRPGAttribute? other)
        {
            if (other is null) return false;
            return (this.AttributeName == other.AttributeName) && (this.SystemID == other.SystemID);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(this.AttributeName, this.SystemID);
        }
    }
}
