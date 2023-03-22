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
using System.Windows.Shapes;
using System.Windows.Shell;

namespace TabletopSystems.Views
{
    /// <summary>
    /// Interaction logic for ViewToPlayWith.xaml
    /// </summary>
    public partial class ViewToPlayWith : Window
    {
        public ViewToPlayWith()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton==MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Button_Maximize(object sender, RoutedEventArgs e)
        {
            if (WindowState != WindowState.Maximized)
            {
                WindowState = WindowState.Maximized;
            }
            else
            { 
                WindowState = WindowState.Normal; 
            }
            
        }
        private void Button_Minimize(object sender, RoutedEventArgs e)
        {
            WindowState= WindowState.Minimized;
        }
        private void Button_Back(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
