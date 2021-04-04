using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para ProjectDeletion.xaml
    /// </summary>
    public partial class ProjectDeletion : Window
    {
        private ProfessionalPracticesContext _professionalPracticesContext;
        private UnitOfWork _unitOfWork;
        private Project _projectToBeDeleted;

        public ProjectDeletion(int projectId)
        {
            InitializeComponent();
            _professionalPracticesContext = new ProfessionalPracticesContext();
            _unitOfWork = new UnitOfWork(_professionalPracticesContext);
            LoadProjectData(projectId);
            this.DataContext = _projectToBeDeleted;
        }

        private void LoadProjectData(int projectId)
        {
            try
            {
                _projectToBeDeleted = _unitOfWork.Projects.Get(projectId);
            }
            catch (SqlException)
            {
                _unitOfWork.Dispose();
                NotifyErrorAndExit();                
            }
            catch (EntityException)
            {
                _unitOfWork.Dispose();
                NotifyErrorAndExit();
            }
            
        }

        private void NotifyErrorAndExit()
        {
            MessageBox.Show("No se pudo obtener información de la base de datos");
            GoBackToProjectConsultation();
        }

        private void GoBackToProjectConsultation()
        {
            ProjectConsultation projectConsultation = new ProjectConsultation();
            projectConsultation.Show();
            Close();
        }

        private void BackButtonClicked(object sender, RoutedEventArgs e)
        {
            GoBackToProjectConsultation();
        }

        private void DeleteButtonClicked(object sender, RoutedEventArgs e)
        {
            if (UserConfirmedAction())
            {
                try
                {
                    Assignment activeAssignment = _unitOfWork.Assignments.FindFirstOccurence(assignment =>
                    assignment.IdProject == _projectToBeDeleted.IdProject);
                    if(activeAssignment == null)
                    {
                        _projectToBeDeleted.Status = ProjectStatus.INACTIVE;
                        int rowsAffected = _unitOfWork.Complete();
                        if(rowsAffected == 1)
                        {
                            MessageBox.Show("El proyecto se eliminó exitosamente");
                        }
                        else
                        {
                            MessageBox.Show("El proyecto no pudo eliminarse");
                        }                        
                    }
                    else
                    {
                        MessageBox.Show("El proyecto no puede eliminarse debido a que tiene uno o más practicantes activos");
                    }
                    GoBackToProjectConsultation();
                }
                catch (SqlException)
                {                    
                    NotifyErrorAndExit();
                }
                catch (EntityException)
                {                    
                    NotifyErrorAndExit();
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
        }

        private bool UserConfirmedAction()
        {
            MessageBoxResult userConfirmedAction = MessageBox.Show("¿Seguro desea eliminar el proyecto?",
                "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return userConfirmedAction == MessageBoxResult.Yes;
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            GoBackToProjectConsultation();
        }
    }
}
