using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopSystems.Models
{
    //Class to store Capability associated with level so properties can notify when they are changed
    //Allows use of ObservableCollection (don't have to make an ObservableDictionary)
    public class CapabilityAndLevel : ObservableObject, IEquatable<CapabilityAndLevel>
    {
        private TTRPGCapability _capability;
        private int _level;
        public TTRPGCapability Capability
        {
            get { return _capability; }
            set { _capability = value; OnPropertyChanged(); }
        }
        public int Level
        {
            get { return _level; }
            set { _level = value; OnPropertyChanged(); }
        }
        public CapabilityAndLevel()
        {
            _capability = new TTRPGCapability();
            _level = 0;
        }
        public CapabilityAndLevel(TTRPGCapability capability)
        {
            _capability = capability;
        }

        public bool Equals(CapabilityAndLevel? other)
        {
            if (other is null) return false;
            return this.Capability == other.Capability;
        }
    }
}
