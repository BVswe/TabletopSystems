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
using TabletopSystems.Helper_Methods;
using TabletopSystems.Models;
using TabletopSystems.ViewModels;

namespace TabletopSystems.Views
{
    /// <summary>
    /// Interaction logic for SearchView.xaml
    /// </summary>
    public partial class SearchView : UserControl
    {
        public SearchView()
        {
            InitializeComponent();
        }

        private void Tags_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Change bool value and refresh results here
            var viewModel = (SearchViewModel)this.DataContext;
            viewModel.Tags[(sender as TextBlock).Text].BoolValue = !viewModel.Tags[(sender as TextBlock).Text].BoolValue;
        }
        private void Categories_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Change bool value and refresh results here
            var viewModel = (SearchViewModel)this.DataContext;
            viewModel.Categories[(sender as TextBlock).Text].BoolValue = !viewModel.Categories[(sender as TextBlock).Text].BoolValue;
        }

        private void SearchTermTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = (SearchViewModel)this.DataContext;
            if (e.Key == Key.Enter)
            {
                viewModel.SearchCommand.Execute(null);
            }
        }
    }
}
