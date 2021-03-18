using System.Windows;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using System.Data.Entity.Core;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para DeleteResponsibleProject.xaml
    /// </summary>
    public partial class ResponsibleProjectDeletion : Window
    {
        private ResponsibleProject responsibleProject;
        public ResponsibleProjectDeletion()
        {
            InitializeComponent();
        }

        public void InitializeDataResponsibleProject(ResponsibleProject responsibleProjectReceived)
        {
            LabelName.Content = responsibleProjectReceived.Name;
            LabelLastName.Content = responsibleProjectReceived.LastName;
            LabelEmail.Content = responsibleProjectReceived.EmailAddress;
            LabelCharge.Content = responsibleProjectReceived.Charge;
            responsibleProject = responsibleProjectReceived;
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea cancelar?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                ResponsibleProjectConsult listResponsibleProject = new ResponsibleProjectConsult();
                listResponsibleProject.Show();
                Close();
            }
        }

        private void BehindButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            ResponsibleProjectConsult listResponsibleProject = new ResponsibleProjectConsult();
            listResponsibleProject.Show();
            Close();
        }

        private void DeleteButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea eliminar el responsable del proyecto?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(messageBoxResult == MessageBoxResult.Yes)
            {
                try
                {
                    responsibleProject.ResponsibleProjectStatus = ResponsibleProjectStatus.INACTIVE;
                    ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                    UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                    unitOfWork.ResponsibleProjects.SoftDeleteResponsibleProject(responsibleProject);
                    int rowsAffected = unitOfWork.Complete();
                    unitOfWork.Dispose();
                    if (rowsAffected == 1)
                    {
                        MessageBox.Show("El Responsable del proyecto se eliminó exitosamente", "Elimiación Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("El responsable del proyecto no pudo eliminarse", "Eliminación Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (EntityException)
                {
                    MessageBox.Show("El responsable del proyecto no pudo eliminarse", "Eliminación Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                ResponsibleProjectConsult listResponsibleProject = new ResponsibleProjectConsult();
                listResponsibleProject.Show();
                Close();
            }
        }
    }
}
