using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using System.Collections.Generic;
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

        private void ConsultPracticionerButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            IEnumerable<Practicioner> thereArePracticioners = unitOfWork.Practicioners.GetAll();
            if (thereArePracticioners == null)
            {
                MessageBox.Show("No hay ningún practicante registrado", "No se puede acceder", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                PracticionerConsult practicionerConsult = new PracticionerConsult();
                practicionerConsult.Show();
                Close();
            }
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
        
        private void RegisterLinkedOrganizationButtonClicked(object sender, RoutedEventArgs e)
        {
            LinkedOrganizationRegistry linkedOrganizationRegistry = new LinkedOrganizationRegistry();
            linkedOrganizationRegistry.Show();
            this.Close();
        }

        private void ConsultLinkedOrganizationButtonClicked(object sender, RoutedEventArgs e)
        {
            LinkedOrganizationConsultation linkedOrganizationConsultation
                = new LinkedOrganizationConsultation();
            linkedOrganizationConsultation.Show();
            Close();
        }

        private void ConsultProjectButtonClicked(object sender, RoutedEventArgs e)
        {
            ProjectConsultation projectConsultation = new ProjectConsultation();
            projectConsultation.Show();
            Close();
        }
    }
}
