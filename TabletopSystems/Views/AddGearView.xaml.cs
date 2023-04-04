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
using TabletopSystems.Models;
using TabletopSystems.ViewModels;

namespace TabletopSystems.Views
{
    /// <summary>
    /// Interaction logic for AddGearView.xaml
    /// </summary>
    public partial class AddGearView : UserControl
    {
        public AddGearView()
        {
            InitializeComponent();
        }
        private void TagComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ComboBox).SelectedItem;
            if (item == null)
            {
                return;
            }
            var viewModel = (AddGearViewModel)this.DataContext;
            viewModel.SelectedTag = (TTRPGTag)item;
        }

        private void TagList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item == null)
            {
                return;
            }
            var viewModel = (AddGearViewModel)this.DataContext;
            viewModel.RemovalTag = (TTRPGTag)item;
        }

        private void TagComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            (sender as ComboBox).IsDropDownOpen = true;
        }
    }
}
