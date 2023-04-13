using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopSystems.Helper_Methods;

namespace TabletopSystems.Models
{
    public class SubraceStats : ObservableObject, IEquatable<SubraceStats>
    {
        public string _subraceName;
        private Dictionary<TTRPGAttribute, ObservableInt> _subraceAttributes;
        public string SubraceName
        {
            get { return _subraceName; }
            set { _subraceName = value; OnPropertyChanged(); }
        }
        public Dictionary<TTRPGAttribute, ObservableInt> SubraceAttributes
        {
            get { return _subraceAttributes; }
            set { _subraceAttributes = value; OnPropertyChanged(); }
        }

        public SubraceStats()
        {
            _subraceName = "";
            _subraceAttributes = new Dictionary<TTRPGAttribute, ObservableInt>();
        }
        public SubraceStats(string s)
        {
            _subraceName = s;
            _subraceAttributes = new Dictionary<TTRPGAttribute, ObservableInt>();
        }
        public SubraceStats(string s, Dictionary<TTRPGAttribute, ObservableInt> attr)
        {
            _subraceName = s;
            _subraceAttributes = attr;
        }

        public bool Equals(SubraceStats? other)
        {
            if (other is null) return false;
            return this.SubraceName == other.SubraceName;
        }
    }
}
