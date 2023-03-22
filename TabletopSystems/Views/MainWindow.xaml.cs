using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TabletopSystems
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
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
            WindowState = WindowState.Minimized;
        }
        private void Button_Back(object sender, RoutedEventArgs e)
        {

        }
    }
}
