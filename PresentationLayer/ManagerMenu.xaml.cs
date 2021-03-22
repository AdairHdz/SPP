
using System.Windows;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para AdministratorMenu.xaml
    /// </summary>
    public partial class ManagerMenu : Window
    {
        public ManagerMenu()
        {
            InitializeComponent();
        }

        private void ConsultTeacherButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            
        }

        private void LogOutButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            Login login = new Login();
            login.Show();
            Close();
        }
    }
}
