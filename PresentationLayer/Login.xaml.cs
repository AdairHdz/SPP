﻿using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using PresentationLayer.Validators;
using System.Data.Entity.Core;
using System.Windows;
using Utilities;
using FluentValidation.Results;
using System.Collections.Generic;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private Account accountCurrent;
        private Account accountReceived;
        private User user;
        private string _password;
        public Login()
        {
            InitializeComponent();
        }

        private void LoginButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                if (!IsAccount(unitOfWork))
                {
                    if (IsValidAccountPassword())
                    {
                        if (IsActiveUser(unitOfWork))
                        {
                            OpenWindowUser();
                        }
                        else
                        {
                            MessageBox.Show("La cuenta no esta activa", "Cuenta inactiva", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Contraseña incorrecta", "Login Fallido", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Usuario incorrecto", "Login Fallido", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (EntityException)
            {
                MessageBox.Show("No se pudo Iniciar sesión. Intente más tarde", "Inicio de sesión Fallido", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private bool IsActiveUser(UnitOfWork unitOfWork)
        {
            user = unitOfWork.Users.FindFirstOccurence(userObtein => userObtein.IdAccount == accountReceived.IdAccount);
            return user.UserStatus == UserStatus.ACTIVE;
        }

        private bool IsAccount(UnitOfWork unitOfWork)
        {
            accountCurrent = new Account();
            accountCurrent.Username = TextBoxUsername.Text;
            accountCurrent.Password = PasswordBoxPassword.Password;
            accountReceived = unitOfWork.Accounts.FindFirstOccurence(accountObtein => accountObtein.Username == accountCurrent.Username);
            return object.ReferenceEquals(null, accountReceived);
        }

        private void OpenWindowUser()
        {
            if (!accountReceived.FirstLogin) {
                if (user.UserType == UserType.Coordinator)
                {
                    CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
                    CoordinatorMenu.User = user;
                    coordinatorMenu.Show();
                    Close();
                }
                else
                {
                    if (user.UserType == UserType.Practicioner)
                    {
                        PracticionerMenu practicionerMenu = new PracticionerMenu(TextBoxUsername.Text);
                        ReportList._User = user;
                        practicionerMenu.Show();
                        Close();
                    }
                    else
                    {
                        if (user.UserType == UserType.Manager)
                        {
                            ManagerMenu managerMenu = new ManagerMenu();
                            managerMenu.Show();
                            Close();
                        }
                        else
                        {
                            string staffNumber = GetStaffNumberTeacher();
                            TeacherMenu teacherMenu = new TeacherMenu(staffNumber);
                            TeacherMenu._User = user;
                            teacherMenu.Show();
                            Close();
                        }
                    }
                }
            }
            else
            {
                FirstLogin firstLogin = new FirstLogin();
                firstLogin.InitializeAccount(accountReceived);
                firstLogin.Show();
                Close();
            }
        }

        private string GetStaffNumberTeacher()
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            string staffNumber = null;
            try
            {
                staffNumber = unitOfWork.Teachers.GetStaffNumberTeacher(_password, TextBoxUsername.Text);
            }
            catch (EntityException)
            {
                MessageBox.Show("No se pudo Iniciar sesión. Intente más tarde", "Inicio de sesión Fallido", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return staffNumber;
        }

        private bool IsValidAccountPassword()
        {
            BCryptHashGenerator bCryptHashGenerator = new BCryptHashGenerator();
            string hashedPassword = bCryptHashGenerator.GenerateHashedString(accountCurrent.Password, accountReceived.Salt);
            accountCurrent.Password = hashedPassword;
            _password = hashedPassword;
            AccountValidator accountValidator = new AccountValidator(accountReceived);
            ValidationResult dataValidationResult = accountValidator.Validate(accountCurrent);
            IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
            UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
            userFeedback.ShowFeedback();
            return dataValidationResult.IsValid;
        }
    }
}
