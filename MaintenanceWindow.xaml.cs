using System;
using System.Windows;
using System.Windows.Input;

namespace WpfApp1
{
    public partial class MaintenanceWindow : Window
    {
        public MaintenanceWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Environment.Exit(0);
        }
    }
}
