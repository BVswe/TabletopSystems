using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using TabletopSystems.Helper_Methods;

namespace TabletopSystems.ViewModels
{
    public class DisplayItemViewModel
    {
        private IViewVM _viewModel;
        public IViewVM ViewModel
        {
            get { return _viewModel; }
        }
        public string CategoryName
        {
            get { return _viewModel.CategoryName; }
        }
        public DisplayItemViewModel(IViewVM viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
