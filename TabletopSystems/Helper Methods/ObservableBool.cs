using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopSystems.Helper_Methods
{
    public class ObservableBool : INotifyPropertyChanged, IEquatable<ObservableBool>
    {
        private bool _value;
        public event PropertyChangedEventHandler? PropertyChanged;
        public bool BoolValue
        {
            get { return _value; }
            set { _value = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BoolValue")); }
        }
        /// <summary>
        /// Creates an ObservableBool set to false by default
        /// </summary>
        public ObservableBool()
        {
            _value = false;
        }
        public ObservableBool(bool givenBool)
        {
            _value = givenBool;
        }

        public bool Equals(ObservableBool? other)
        {
            if (other is null) return false;
            return _value == other._value;
        }
    }
}
