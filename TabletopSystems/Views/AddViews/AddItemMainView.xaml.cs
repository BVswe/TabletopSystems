using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TabletopSystems.ViewModels;

namespace TabletopSystems.Views
{
    /// <summary>
    /// Interaction logic for AddTypeView.xaml
    /// </summary>
    public partial class AddItemMainView : UserControl
    {
        public AddItemMainView()
        {
            InitializeComponent();
        }

        private void AddTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.DataContext as AddItemMainViewModel != null)
            {
                var viewModel = (AddItemMainViewModel)this.DataContext;
                viewModel.ChangeViewModel.Execute((sender as ComboBox).SelectedItem.ToString().Split(" ")[1]);
            }
        }
    }
}
