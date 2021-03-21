using System.Windows;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using PresentationLayer.Validators;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Windows.Media;
using Utilities;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para FirstLogin.xaml
    /// </summary>
    public partial class FirstLogin : Window
    {
        private Account account;
        public FirstLogin()
        {
            InitializeComponent();
        }

        public  void InitializeAccount(Account accountReceived)
        {
            account = accountReceived;
        }

        private void BehindButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            Login login = new Login();
            login.Show();
            Close();
        }

        private void SaveButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            CreateAccountFromInputData();
            if (PasswordBoxNewPassword.Password.Equals(PasswordBoxPasswordConfirmation.Password))
            {
                if (IsValidAccount()){
                    PasswordBoxPasswordConfirmation.BorderBrush = Brushes.Green;
                    try
                    {
                        ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                        UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                        if (!AccountIsAlreadyRegistered(unitOfWork))
                        {
                            HashAccountPassword();
                            if (RegisternewAccount(unitOfWork))
                            {
                                MessageBox.Show("La cuenta se guardo con éxito", "Cambios exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
                                Login login = new Login();
                                login.Show();
                                Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Existe una cuenta con el mismo usuario registrado", "Dato Repetido", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    catch (EntityException)
                    {
                        MessageBox.Show("No se pudo guardar los cambios. Intente más tarde", "Guardar Cambios Fallido", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    PasswordBoxPasswordConfirmation.BorderBrush = Brushes.Red;
                    MessageBox.Show("Contraseña o Usuario inválidos", "Datos inválidos", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                
            }else
            {
                MessageBox.Show("La contraseña y la confirmación no son iguales", "Contraseñas diferentes", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool AccountIsAlreadyRegistered(UnitOfWork unitOfWork)
        {
            Account accountWithSameEmailAddress = unitOfWork.Accounts.FindFirstOccurence(accountObtein => accountObtein.Username == account.Username && accountObtein.IdAccount != account.IdAccount);
            if (accountWithSameEmailAddress != null)
            {
                return true;
            }
            return false;
        }

        private void CreateAccountFromInputData()
        {
            account.Username = TextBoxNewUser.Text;
            account.Password = PasswordBoxNewPassword.Password;
            account.FirstLogin = false;
        }

        private bool IsValidAccount()
        {
            PasswordBoxPasswordConfirmation.BorderBrush = Brushes.Gray;
            AccountValidator accountValidator = new AccountValidator();
            ValidationResult dataValidationResult = accountValidator.Validate(account);
            IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
            UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
            userFeedback.ShowFeedback();
            return dataValidationResult.IsValid;
        }

        private bool RegisternewAccount(UnitOfWork unitOfWork)
        {
            Account accountCurrent = unitOfWork.Accounts.Get(account.IdAccount);
            accountCurrent.Username = account.Username;
            accountCurrent.Password = account.Password;
            accountCurrent.Salt = account.Salt;
            int rowsAffected = unitOfWork.Complete();
            unitOfWork.Dispose();
            return rowsAffected == 1;
        }

        private void HashAccountPassword()
        {
            BCryptHashGenerator bCryptHashGenerator = new BCryptHashGenerator();
            string salt = bCryptHashGenerator.GenerateSalt();
            account.Salt = salt;
            string hashedPassword = bCryptHashGenerator.GenerateHashedString(account.Password, salt);
            account.Password = hashedPassword;
        }
    }
}
