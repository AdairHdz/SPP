using System;

using System.Windows;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para TeacherMenu.xaml
    /// </summary>
    public partial class TeacherMenu : Window
    {
        public TeacherMenu()
        {
            InitializeComponent();
        }

        private void LogOutButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            Login login = new Login();
            login.Show();
            Close();
        }
    }
}
