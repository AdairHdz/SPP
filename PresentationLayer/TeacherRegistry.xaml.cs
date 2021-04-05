using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using FluentValidation.Results;
using PresentationLayer.Utils;
using PresentationLayer.Validators;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Windows;
using Utilities;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para TeacherRegistry.xaml
    /// </summary>
    public partial class TeacherRegistry : Window
    {
        private Teacher _teacher = new Teacher
        {
            User = new User
            {
                Account = new Account()
            }

        };

        public TeacherRegistry()
        {
            InitializeComponent();
            this.DataContext = _teacher;
        }

        private void RegisterButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateTeacherFromInputData();
            if (IsValidData())
            {
                TryRegisterNewTeacher();
            }
        }

        private bool IsValidData()
        {
            TeacherValidator teacherDataValidator = new TeacherValidator();
            FluentValidation.Results.ValidationResult dataValidationResult = teacherDataValidator.Validate(_teacher);
            IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
            UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
            userFeedback.ShowFeedback();
            return dataValidationResult.IsValid;
        }

        private void TryRegisterNewTeacher()
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            try
            {
                if (!TeacherIsAlreadyRegistered(unitOfWork))
                {
                    if (!ThereIsAnActiveTeacher(unitOfWork))
                    {
                        HashAccountPassword();
                        RegisterNewTeacher(unitOfWork);
                        MessageBox.Show("Profesor registrado exitosamente");
                        GoBackToManagerMenu();
                    }
                    else
                    {
                        MessageBox.Show("Ya existe un profesor activo en el sistema.");
                    }
                }
                else
                {
                    MessageBox.Show("Este profesor ya está registrado");
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("No hay conexión a la base de datos. Intente más tarde");
                GoBackToManagerMenu();
            }
            catch (EntityException)
            {
                MessageBox.Show("No hay conexión a la base de datos. Intente más tarde");
                GoBackToManagerMenu();
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }

        private void CreateTeacherFromInputData()
        {
            _teacher.User.UserType = UserType.Teacher;
            _teacher.User.Account.Password = PasswordBoxPassword.Password;
            _teacher.User.Gender = GenderParser.ParseFromRadioButtonsToObject(ManRadioButton);
        }

        private bool TeacherIsAlreadyRegistered(UnitOfWork unitOfWork)
        {
            bool teacherIsAlreadyRegistered = unitOfWork.Teachers.TeacherIsAlreadyRegistered(_teacher, false);
            return teacherIsAlreadyRegistered;
        }

        private bool ThereIsAnActiveTeacher(UnitOfWork unitOfWork)
        {
            User retrievedUser =
                    unitOfWork.Users.FindFirstOccurence(user => user.UserStatus == UserStatus.ACTIVE && user.UserType == UserType.Teacher);
            return retrievedUser != null;
        }

        private void HashAccountPassword()
        {
            BCryptHashGenerator bCryptHashGenerator = new BCryptHashGenerator();
            string salt = bCryptHashGenerator.GenerateSalt();
            string hashedPassword = bCryptHashGenerator.GenerateHashedString(_teacher.User.Account.Password, salt);
            _teacher.User.Account.Password = hashedPassword;
        }

        private void RegisterNewTeacher(UnitOfWork unitOfWork)
        {
            unitOfWork.Accounts.Add(_teacher.User.Account);
            unitOfWork.Users.Add(_teacher.User);
            unitOfWork.Teachers.Add(_teacher);
            unitOfWork.Complete();
        }

        private void GoBackToManagerMenu()
        {
            ManagerMenu managerMenu = new ManagerMenu();
            managerMenu.Show();
            Close();
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            GoBackToManagerMenu();
        }

        private void BackButtonClicked(object sender, RoutedEventArgs e)
        {
            GoBackToManagerMenu();
        }
    }
}
