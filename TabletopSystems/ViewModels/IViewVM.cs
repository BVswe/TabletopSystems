using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopSystems.ViewModels
{
    public interface IViewVM
    {
        public string CategoryName { get; }
        void GetItem(string itemName, int systemID);
    }
}
