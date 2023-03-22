using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TabletopSystems.ViewModels;

namespace TabletopSystems.Views
{
    /// <summary>
    /// Interaction logic for SystemSelectionView.xaml
    /// </summary>
    public partial class SystemSelectionView : UserControl
    {
        public SystemSelectionView()
        {
            InitializeComponent();
        }
        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (SystemSelectionViewModel)this.DataContext;
            viewModel.SystemSelectedCommand.Execute(null);
        }

        private void SystemsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item == null)
            {
                return;
            }
            var viewModel = (SystemSelectionViewModel)this.DataContext;
            viewModel.SelectedSystem = (TabletopSystem)item;
        }
    }
}
