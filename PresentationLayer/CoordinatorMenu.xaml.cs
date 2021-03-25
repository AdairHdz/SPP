
using System.Windows;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para CoordinatorMenu.xaml
    /// </summary>
    public partial class CoordinatorMenu : Window
    {
        public CoordinatorMenu()
        {
            InitializeComponent();
        }

        private void ConsultResponsibleProjectButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            ResponsibleProjectConsult listResponsibleProject = new ResponsibleProjectConsult();
            listResponsibleProject.Show();
            Close();
        }
        private void RegisterResponsibleProjectButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            ResponsableProjectRegistry registerResponsable = new ResponsableProjectRegistry();
            registerResponsable.Show();
            Close();
        }

        private void LogOutButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            Login login = new Login();
            login.Show();
            Close();
        }

        private void RegisterLinkedOrganizationButtonClicked(object sender, RoutedEventArgs e)
        {
            LinkedOrganizationRegistry linkedOrganizationRegistry = new LinkedOrganizationRegistry();
            linkedOrganizationRegistry.Show();
            this.Close();
        }
    }
}
