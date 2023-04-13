using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopSystems.Models
{
    public class TTRPGRace : IEquatable<TTRPGRace>
    {
        public int SystemID { get; set; }
        public string RaceName { get; set; }
        public string RaceDescription { get; set; }
        public List<TTRPGTag> Tags { get; set; }
        public List<SubraceStats> SubraceAttributes { get; set; }
        public Dictionary<TTRPGCapability, int> Capabilities { get; set; }

        public TTRPGRace()
        {
            RaceName = string.Empty;
            RaceDescription = string.Empty;
            Tags = new List<TTRPGTag>();
            SubraceAttributes = new List<SubraceStats>();
            Capabilities = new Dictionary<TTRPGCapability, int>();
        }

        public bool Equals(TTRPGRace? other)
        {
            if (other is null) return false;
            return this.RaceName == other.RaceName && this.SystemID == other.SystemID;
        }
    }
}
