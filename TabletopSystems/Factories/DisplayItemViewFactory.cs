using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopSystems.ViewModels;
using TabletopSystems.Views;

namespace TabletopSystems.Factories
{
    public class DisplayItemViewFactory
    {
        private Func<Type, IViewVM> _itemViewModelFactory;
        public DisplayItemViewFactory(Func<Type, IViewVM> itemViewModelFactory)
        {
            _itemViewModelFactory = itemViewModelFactory;
        }
        public DisplayItemViewModel GetViewModel(string s)
        {
            switch (s)
            {
                case "Capability":
                    return new DisplayItemViewModel(_itemViewModelFactory.Invoke(typeof(DisplayCapabilityViewModel)));
                case "Gear":
                    return new DisplayItemViewModel(_itemViewModelFactory.Invoke(typeof(DisplayGearViewModel)));
                case "Monster":
                    return new DisplayItemViewModel(_itemViewModelFactory.Invoke(typeof(AddMonsterViewModel)));
                case "Class":
                    return new DisplayItemViewModel(_itemViewModelFactory.Invoke(typeof(AddClassViewModel)));
                case "Race":
                    return new DisplayItemViewModel(_itemViewModelFactory.Invoke(typeof(AddRaceViewModel)));
                default:
                    throw new Exception("Program has enocuntered an error. Attempted to open an invalid item");
            }
        }
    }
}
