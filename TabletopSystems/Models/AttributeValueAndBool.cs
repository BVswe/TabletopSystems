using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopSystems.Models
{
    //Class to store TTRPGAttribute with an int and bool to allow usage in ObservableCollection
    public class AttributeValueAndBool : ObservableObject, IEquatable<AttributeValueAndBool>
    {
        private TTRPGAttribute _attribute;
        private int _value;
        private bool _boolValue;

        public TTRPGAttribute Attribute
        {
            get { return _attribute; }
            set { _attribute = value; OnPropertyChanged(); }
        }
        public int Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(); }
        }
        public bool BoolValue
        {
            get { return _boolValue; }
            set { _boolValue = value; OnPropertyChanged(); }
        }

        public AttributeValueAndBool()
        {
            _attribute = new TTRPGAttribute();
            _value = 0;
            _boolValue = false;
        }

        public AttributeValueAndBool(TTRPGAttribute attr)
        {
            _attribute = attr;
            _value = 0;
            _boolValue = false;
        }

        public bool Equals(AttributeValueAndBool? other)
        {
            if (other is null) return false;
            return this.Attribute == other.Attribute;
        }
    }
}
