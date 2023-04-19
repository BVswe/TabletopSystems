using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopSystems.ViewModels
{
    public interface IEditVM
    {
        void FillFromDatabase(string itemName, int systemID);
        void SetEditing();
    }
}
