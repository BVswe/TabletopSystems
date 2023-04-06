using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopSystems.Helper_Methods
{
    public class ObservableInt : ObservableObject
    {
        private int _value;
        public int IntValue
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// Creates an Observableint set to false by default
        /// </summary>
        public ObservableInt()
        {
            _value = 0;
        }
        public ObservableInt(int value)
        {
            _value = value;
        }
    }
}
