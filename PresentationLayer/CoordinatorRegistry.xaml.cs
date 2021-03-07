using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using FluentValidation.Results;
using PresentationLayer.Validators;
using System.Collections.Generic;
using System.Windows;
using Utilities;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para CoordinatorRegistry.xaml
    /// </summary>
    public partial class CoordinatorRegistry : Window
    {
        public Coordinator Coordinator = new Coordinator
        {
            User = new User
            {
                Account = new Account()
            }
        };

        public CoordinatorRegistry()
        {
            InitializeComponent();
            this.DataContext = Coordinator;   
        }

        private void RegisterButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateCoordinatorFromInputData();
            if (IsValidData())
            {
                if (!CoordinatorIsAlreadyRegistered())
                {
                    if (!ThereIsAnActiveCoordinator())
                    {
                        HashAccountPassword();
                        if (RegisternewCoordinator())
                        {
                            MessageBox.Show("Coordinador registrado exitosamente");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ya existe un coordinador activo en el sistema.");
                    }
                }
                else
                {
                    MessageBox.Show("Este coordinador ya está registrado");
                }
            }
            else
            {
                MessageBox.Show("Datos no válidos");
            }
        }

        private void CreateCoordinatorFromInputData()
        {            
            Coordinator.User.UserType = UserType.Coordinator;
            Coordinator.User.Account.Password = PasswordBoxPassword.Password;

            if (ManRadioButton.IsChecked == true)
            {                
                Coordinator.User.Gender = Gender.MALE;
            }
            else if (WomanRadioButton.IsChecked == true)
            {                
                Coordinator.User.Gender = Gender.FEMALE;
            }            
        }

        private bool IsValidData()
        {
            CoordinatorValidator coordinatorDataValidator = new CoordinatorValidator();
            FluentValidation.Results.ValidationResult dataValidationResult = coordinatorDataValidator.Validate(Coordinator);
            IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
            UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
            userFeedback.ShowFeedback();
            return dataValidationResult.IsValid;
        }

        private bool CoordinatorIsAlreadyRegistered()
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            bool coordinatorIsAlreadyRegistered = unitOfWork.Coordinators.CoordinatorIsAlreadyRegistered(Coordinator);
            bool userIsAlreadyRegistered = unitOfWork.Users.UserIsAlreadyRegistered(Coordinator.User);
            unitOfWork.Dispose();
            if (coordinatorIsAlreadyRegistered || userIsAlreadyRegistered)
            {
                return true;
            }
            return false;
        }

        private bool ThereIsAnActiveCoordinator()
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            User retrievedUser =
                    unitOfWork.Users.FindFirstOccurence(user => user.UserStatus == UserStatus.ACTIVE && user.UserType == UserType.Coordinator);
            unitOfWork.Complete();
            unitOfWork.Dispose();
            return retrievedUser != null;
        }

        private void HashAccountPassword()
        {
            BCryptHashGenerator bCryptHashGenerator = new BCryptHashGenerator();
            string salt = bCryptHashGenerator.GenerateSalt();
            string hashedPassword = bCryptHashGenerator.GenerateHashedString(Coordinator.User.Account.Password, salt);
            Coordinator.User.Account.Password = hashedPassword;
        }

        private bool RegisternewCoordinator()
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            unitOfWork.Accounts.Add(Coordinator.User.Account);
            unitOfWork.Users.Add(Coordinator.User);
            unitOfWork.Coordinators.Add(Coordinator);
            int rowsAffected = unitOfWork.Complete();
            unitOfWork.Dispose();
            return rowsAffected == 3;
        }
    }
}
