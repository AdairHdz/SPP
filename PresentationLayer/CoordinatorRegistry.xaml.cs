using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using FluentValidation.Results;
using PresentationLayer.Utils;
using PresentationLayer.Validators;
using System.Collections.Generic;
using System.Data.Entity.Core;
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
                TryRegisterNewCoordinator();
            }
        }

        private void TryRegisterNewCoordinator()
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            try
            {
                if (!CoordinatorIsAlreadyRegistered(unitOfWork))
                {
                    if (!ThereIsAnActiveCoordinator(unitOfWork))
                    {
                        HashAccountPassword();
                        RegisterNewCoordinator(unitOfWork);
                        MessageBox.Show("Coordinador registrado exitosamente");
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
            }catch(EntityException)
            {
                MessageBox.Show("Sin conexión a bd");
            }
            finally
            {
                unitOfWork.Complete();
                unitOfWork.Dispose();
            }
        }

        private void CreateCoordinatorFromInputData()
        {            
            Coordinator.User.UserType = UserType.Coordinator;
            Coordinator.User.Account.Password = PasswordBoxPassword.Password;
            Coordinator.User.Gender = GenderParser.ParseFromRadioButtonsToObject(ManRadioButton);
        }

        private bool IsValidData()
        {
            CoordinatorValidator coordinatorDataValidator = new CoordinatorValidator();
            ValidationResult dataValidationResult = coordinatorDataValidator.Validate(Coordinator);
            IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
            UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
            userFeedback.ShowFeedback();
            return dataValidationResult.IsValid;
        }

        private bool CoordinatorIsAlreadyRegistered(UnitOfWork unitOfWork)
        {
            bool coordinatorIsAlreadyRegistered = unitOfWork.Coordinators.CoordinatorIsAlreadyRegistered(Coordinator, false);            
            return coordinatorIsAlreadyRegistered;
        }

        private bool ThereIsAnActiveCoordinator(UnitOfWork unitOfWork)
        {
            User retrievedUser =
                    unitOfWork.Users.FindFirstOccurence(user => user.UserStatus == UserStatus.ACTIVE && user.UserType == UserType.Coordinator);
            return retrievedUser != null;
        }

        private void HashAccountPassword()
        {
            BCryptHashGenerator bCryptHashGenerator = new BCryptHashGenerator();
            string salt = bCryptHashGenerator.GenerateSalt();
            string hashedPassword = bCryptHashGenerator.GenerateHashedString(Coordinator.User.Account.Password, salt);
            Coordinator.User.Account.Password = hashedPassword;
        }

        private void RegisterNewCoordinator(UnitOfWork unitOfWork)
        {
            unitOfWork.Accounts.Add(Coordinator.User.Account);
            unitOfWork.Users.Add(Coordinator.User);
            unitOfWork.Coordinators.Add(Coordinator);
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea cancelar?", 
                "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if(messageBoxResult == MessageBoxResult.Yes)
            {
                this.Close();
            }
            
        }
    }
}
