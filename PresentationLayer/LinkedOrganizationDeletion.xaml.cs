using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Windows;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para LinkedOrganizationDeletion.xaml
    /// </summary>
    public partial class LinkedOrganizationDeletion : Window
    {
        private LinkedOrganization _linkedOrganization;

        public LinkedOrganizationDeletion(int idLinkedOrganization)
        {
            InitializeComponent();
            LoadLinkedOrganizationData(idLinkedOrganization);
            this.DataContext = _linkedOrganization;
        }

        private void LoadLinkedOrganizationData(int idLinkedOrganization)
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            try
            {                
                _linkedOrganization = unitOfWork.LinkedOrganizations.Get(idLinkedOrganization);
                TextBlockFirstPhoneNumber.Text = $"+{_linkedOrganization.PhoneNumbers[0].Extension}" +
                    $" {_linkedOrganization.PhoneNumbers[0].PhoneNumber}";
                TextBlockSecondPhoneNumber.Text = $"+{_linkedOrganization.PhoneNumbers[1].Extension}" +
                    $" {_linkedOrganization.PhoneNumbers[1].PhoneNumber}";
            }
            catch (SqlException)
            {
                unitOfWork.Dispose();
                NotifyErrorAndExit();
            }
            catch (EntityException)
            {
                unitOfWork.Dispose();
                NotifyErrorAndExit();
            }
        }

        private void NotifyErrorAndExit()
        {
            MessageBox.Show("No se pudo obtener información de la base de datos");            
            GoBackToLinkedOrganizationConsultation();
        }

        private void GoBackToLinkedOrganizationConsultation()
        {
            LinkedOrganizationConsultation linkedOrganizationConsultation = new LinkedOrganizationConsultation();
            linkedOrganizationConsultation.Show();
            this.Close();
        }

        private void DeleteButtonClicked(object sender, RoutedEventArgs e)
        {
            if (UserConfirmedDeletion())
            {
                ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                try
                {
                    int linkedOrganizationId = _linkedOrganization.IdLinkedOrganization;
                    bool linkedOrganizationHasActiveProjects = unitOfWork.LinkedOrganizations.HasActiveProjects(linkedOrganizationId);
                    if (linkedOrganizationHasActiveProjects)
                    {
                        MessageBox.Show("La organización vinculada no pudo eliminarse debido a que tiene uno o más proyectos activos");
                    }
                    else
                    {
                        LinkedOrganization linkedOrganization = unitOfWork.LinkedOrganizations.Get(linkedOrganizationId);
                        linkedOrganization.LinkedOrganizationStatus = LinkedOrganizationStatus.INACTIVE;
                        int affectedRows = unitOfWork.Complete();
                        if (affectedRows == 1)
                        {
                            MessageBox.Show("La organización vinculada se eliminó exitosamente");
                        }
                        else
                        {
                            MessageBox.Show("La organización vinculada no pudo eliminarse");

                        }
                    }
                    GoBackToLinkedOrganizationConsultation();
                }
                catch (SqlException)
                {
                    unitOfWork.Dispose();
                    NotifyErrorAndExit();
                }
                catch (EntityException)
                {
                    unitOfWork.Dispose();
                    NotifyErrorAndExit();
                }

            }
        }

        private bool UserConfirmedDeletion()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea eliminar la Organización vinculada?",
                "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return messageBoxResult == MessageBoxResult.Yes;
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            GoBackToLinkedOrganizationConsultation();
        }

        private void BackButtonClicked(object sender, RoutedEventArgs e)
        {
            GoBackToLinkedOrganizationConsultation();
        }
    }
}
