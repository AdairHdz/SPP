using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using PresentationLayer.Utils;
using PresentationLayer.Validators;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para CoordinatorModification.xaml
    /// </summary>
    public partial class CoordinatorModification : Window
    {
        private readonly ProfessionalPracticesContext _professionalPracticesContext;
        private readonly UnitOfWork _unitOfWork;
        public Coordinator Coordinator = new Coordinator
        {
            User = new User
            {
                Account = new Account()
            }
        };
        public CoordinatorModification(string staffNumber)
        {
            InitializeComponent();
            _professionalPracticesContext = new ProfessionalPracticesContext();
            _unitOfWork = new UnitOfWork(_professionalPracticesContext);
            LoadCoordinator(staffNumber);
            this.DataContext = Coordinator;
        }

        private void LoadCoordinator(string staffNumber)
        {
            try
            {
                Coordinator = _unitOfWork.Coordinators.GetCoordinatorWithUserAndAccountData(staffNumber);                
                ManRadioButton.IsChecked = Coordinator.User.Gender == Gender.MALE;
                WomanRadioButton.IsChecked = !ManRadioButton.IsChecked;
                if(Coordinator.User.UserStatus == UserStatus.ACTIVE)
                {
                    ComboBoxCoordinatorStatus.SelectedIndex = 0;
                }
                else
                {
                    ComboBoxCoordinatorStatus.SelectedIndex = 1;
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("No hay conexión a la base de datos. Intente más tarde.");
                _unitOfWork.Dispose();
                this.Close();
            }
            
        }

        private void ModifyButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateCoordinatorFromInputData();
            FluentValidation.Results.ValidationResult dataValidationResult = ValidateData();
            ShowUserFeedback(dataValidationResult);

            try
            {
                bool thereIsAnotherCoordinatorWithTheSameInformation =
                _unitOfWork.Coordinators.CoordinatorIsAlreadyRegistered(Coordinator, true);
                if (dataValidationResult.IsValid)
                {
                    if (thereIsAnotherCoordinatorWithTheSameInformation)
                    {
                        MessageBox.Show("El correo, correo alternativo o número " +
                            "de teléfono ingresado ya está ocupado por otro usuario");
                    }
                    else
                    {
                        bool userConfirmedAction = AskForConfirmation();
                        if (userConfirmedAction)
                        {
                            ModifyCoordinator();
                        }
                    }
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("No hay conexión a la base de datos. Intente más tarde.");
                _unitOfWork.Dispose();
                this.Close();
            }
        }

        private void CreateCoordinatorFromInputData()
        {
            Coordinator.User.Gender = GenderParser.ParseFromRadioButtonsToObject(ManRadioButton);
            int selectedTag = int.Parse(((ComboBoxItem)ComboBoxCoordinatorStatus.SelectedItem).Tag.ToString());
            if (selectedTag == 0)
            {
                Coordinator.User.UserStatus = UserStatus.INACTIVE;
            }
            else
            {
                Coordinator.User.UserStatus = UserStatus.ACTIVE;
            }
        }

        private FluentValidation.Results.ValidationResult ValidateData()
        {
            CoordinatorValidator coordinatorDataValidator = new CoordinatorValidator();
            FluentValidation.Results.ValidationResult dataValidationResult = coordinatorDataValidator.Validate(Coordinator);
            return dataValidationResult;
        }

        private void ShowUserFeedback(FluentValidation.Results.ValidationResult dataValidationResult)
        {
            UserFeedback userFeedback = new UserFeedback(FormGrid, dataValidationResult.Errors);
            userFeedback.ShowFeedback();
        }

        private void ModifyCoordinator()
        {
            if (Coordinator.User.UserStatus == UserStatus.ACTIVE)
            {
                User activeCoordinatorUser =
                    _unitOfWork.Users.FindFirstOccurence
                    (user => user.UserStatus == UserStatus.ACTIVE && user.UserType == UserType.Coordinator
                    && user.IdUser != Coordinator.User.IdUser);
                if (activeCoordinatorUser == null || activeCoordinatorUser.IdUser == Coordinator.User.IdUser)
                {
                    SaveChanges();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ya existe un coordinador activo en el sistema");
                }
            }
            else
            {
                SaveChanges();
                this.Close();
            }
        }
        
        private void SaveChanges()
        {
            _unitOfWork.Complete();
            _unitOfWork.Dispose();
            MessageBox.Show("Coordinador modificado exitosamente.");            
        }

        private bool AskForConfirmation()
        {
            MessageBoxResult messageBoxResult = 
                MessageBox.Show("¿Seguro desea modificar al coordinador?", "Confirmación", MessageBoxButton.YesNo);
            return messageBoxResult == MessageBoxResult.Yes;
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            _unitOfWork.Dispose();
            CoordinatorConsultation coordinatorConsultation = new CoordinatorConsultation();
            coordinatorConsultation.Show();
            this.Close();
        }
    }
}
