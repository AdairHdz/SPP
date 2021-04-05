
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
            TeacherConsultation teacherConsultation = new TeacherConsultation();
            teacherConsultation.Show();
            Close();
        }

        private void LogOutButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            Login login = new Login();
            login.Show();
            Close();
        }

        private void RegisterCoordinatorButtonClicked(object sender, RoutedEventArgs e)
        {
            CoordinatorRegistry coordinatorRegistry = new CoordinatorRegistry();
            coordinatorRegistry.Show();
            Close();
        }

        private void RegisterTeacherButtonClicked(object sender, RoutedEventArgs e)
        {
            TeacherRegistry teacherRegistry = new TeacherRegistry();
            teacherRegistry.Show();
            Close();
        }
    }
}
