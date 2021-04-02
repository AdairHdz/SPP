
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
        
        private void ConsultPracticionerButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            PracticionerConsult practicionerConsult = new PracticionerConsult();
            practicionerConsult.Show();
            Close();
        }
        
        private void RegisterPracticionerButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            PracticionerRegistry practicionerRegistry = new PracticionerRegistry();
            practicionerRegistry.Show();
            Close();
        }

        private void ConsultLinkedOganizationButtonClicked(object sender, RoutedEventArgs e)
        {
            LinkedOrganizationConsultation linkedOrganizationConsultation = new LinkedOrganizationConsultation();
            linkedOrganizationConsultation.Show();
            this.Close();
        }
    }
}
