

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web;
using System.Windows.Documents;

namespace TabletopSystems.Models
{
    public class TTRPGCapability : IEquatable<TTRPGCapability>
    {
        public string CapabilityName { get; set; }
        public int SystemID { get; set; }
        public string Description { get; set; }
        public string Area { get; set; }
        public string Range { get; set; }
        public string UseTime { get; set; }
        public string Cost { get; set; }
        public ObservableCollection<TTRPGTag> Tags { get; set; }

        public TTRPGCapability() 
        {
            CapabilityName = string.Empty;
            Description = string.Empty;
            Area = string.Empty;
            Range = string.Empty;
            UseTime = string.Empty;
            Cost = string.Empty;
            Tags = new ObservableCollection<TTRPGTag>();
        }

        public bool Equals(TTRPGCapability? other)
        {
            if (other is null) return false;
            return this.CapabilityName == other.CapabilityName && this.SystemID == other.SystemID;
        }
    }                                                                                                                                                                                                       
}
