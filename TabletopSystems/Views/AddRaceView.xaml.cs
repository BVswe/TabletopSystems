﻿using System;
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
    /// Interaction logic for AddRaceView.xaml
    /// </summary>
    public partial class AddRaceView : UserControl
    {
        public AddRaceView()
        {
            InitializeComponent();
        }
        private void CapabilityComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            /*if (e.Key == Key.Enter)
            {
                var viewModel = (AddMonsterViewModel)this.DataContext;
                viewModel.AddToCapabilityListCommand.Execute((sender as ComboBox).SelectedItem);
                e.Handled = true;
                return;
            }
            (sender as ComboBox).IsDropDownOpen = true;*/
        }

        private void TagComboBox_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
