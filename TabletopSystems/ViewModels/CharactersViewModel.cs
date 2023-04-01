using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopSystems.ViewModels
{
    public class CharactersViewModel : ObservableObject
    {
        //Header is to set the name of this tab in the view
        public string Header { get;}
        public CharactersViewModel()
        {
            Header = "Characters";
        }
    }
}
