using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using System.Data.SqlClient;
using System.Windows;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para CoordinatorDeletion.xaml
    /// </summary>
    public partial class CoordinatorDeletion : Window
    {
        public Coordinator Coordinator = new Coordinator
        {
            User = new User
            {
                Account = new Account()
            }
        };

        public CoordinatorDeletion()
        {
            InitializeComponent();
            LoadActiveCoordinator();
            this.DataContext = Coordinator;
        }

        private void LoadActiveCoordinator()
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            try
            {                
                Coordinator retrievedCoordinator = unitOfWork.Coordinators.GetActiveCoordinator();
                if (retrievedCoordinator != null)
                {
                    Coordinator = retrievedCoordinator;
                    PrintUserGender();
                }
                else
                {
                    MessageBox.Show("Error. No existe ningún coordinador activo en la base de datos.");
                    GoBackToCoordinatorConsultation();
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("No hay conexión a la base de datos. Intente más tarde");
                GoBackToCoordinatorConsultation();
            }
            finally
            {
                unitOfWork.Dispose();
            }
           
        }

        private void PrintUserGender()
        {
            if(Coordinator.User.Gender == Gender.MALE)
            {
                TextBlockGender.Text = "Masculino";
            }
            else
            {
                TextBlockGender.Text = "Femenino";
            }
        }

        private void DeleteButtonClicked(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro que desea eliminar al coordinador?",
                "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                try
                {                    
                    unitOfWork.Coordinators.SetCoordinatorStatusAsInactive(Coordinator.StaffNumber);
                    unitOfWork.Complete();                    
                    MessageBox.Show("Coordinador eliminado exitosamente");
                }
                catch (SqlException)
                {
                    MessageBox.Show("No hay conexión a la base de datos. Intente más tarde");                    
                }
                finally
                {
                    unitOfWork.Dispose();
                    GoBackToCoordinatorConsultation();
                }                                                
            }
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            GoBackToCoordinatorConsultation();
        }

        private void BackButtonClicked(object sender, RoutedEventArgs e)
        {
            GoBackToCoordinatorConsultation();
        }

        private void GoBackToCoordinatorConsultation()
        {
            CoordinatorConsultation coordinatorConsultation = new CoordinatorConsultation();
            coordinatorConsultation.Show();
            Close();
        }
    }
}
