

using System.Collections.Generic;
using System.Windows.Documents;

namespace TabletopSystems.Models
{
    public class TTRPGCapability
    {
        public string CapabilityName { get; set; }
        public int SystemID { get; set; }
        public string Description { get; set; }
        public string Area { get; set; }
        public int Range { get; set; }
        public string UseTime { get; set; }
        public string Cost { get; set; }
        public List<TTRPGTag> Tags { get; set; }
    }                                                                                                                                                                                                       
}
