using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopSystems.Models
{
    public class TTRPGAction : IEquatable<TTRPGAction>
    {
        public int SystemID { get; set; }
        public string ActionName { get; set; }
        public string ActionFormula { get; set; }

        public TTRPGAction()
        {
            ActionName = string.Empty;
            ActionFormula = string.Empty;
        }

        public bool Equals(TTRPGAction? other)
        {
            if (other is null) return false;
            return this.ActionName == other.ActionName && this.SystemID == other.SystemID;
        }
    }
}
