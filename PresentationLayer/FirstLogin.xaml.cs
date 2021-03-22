using System.Windows;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
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
            if (PasswordBoxNewPassword.Password.Equals(PasswordBoxPasswordConfirmation.Password))
            {
                if (IsValidAccount())
                {
                    try
                    {
                        ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                        UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                        HashAccountPassword();
                        if (SaveAccount(unitOfWork))
                        {
                            MessageBox.Show("La cuenta se guardo con éxito", "Cambios exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
                            Login login = new Login();
                            login.Show();
                            Close();
                        }
                    }
                    catch (EntityException)
                    {
                        MessageBox.Show("No se pudo guardar los cambios. Intente más tarde", "Guardar Cambios Fallido", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Contraseña y Confirmación inválidos", "Datos inválidos", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                
            }else
            {
                PasswordBoxPasswordConfirmation.BorderBrush = Brushes.Red;
                PasswordBoxNewPassword.BorderBrush = Brushes.Red;
                MessageBox.Show("La contraseña y la confirmación no son iguales", "Contraseñas diferentes", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool IsValidAccount()
        {
            PasswordBoxPasswordConfirmation.BorderBrush = Brushes.Gray;
            PasswordBoxNewPassword.BorderBrush = Brushes.Gray;
            string password = PasswordBoxNewPassword.Password;
            if (!string.IsNullOrWhiteSpace(password) && password.Length<61 && password.Length>7)
            {
                PasswordBoxPasswordConfirmation.BorderBrush = Brushes.Green;
                PasswordBoxNewPassword.BorderBrush = Brushes.Green;
                account.Password = PasswordBoxNewPassword.Password;
                return true;
            }
            else
            {
                PasswordBoxPasswordConfirmation.BorderBrush = Brushes.Red;
                PasswordBoxNewPassword.BorderBrush = Brushes.Red;
                return false;
            }

        }

        private bool SaveAccount(UnitOfWork unitOfWork)
        {
            Account accountCurrent = unitOfWork.Accounts.Get(account.IdAccount);
            accountCurrent.Password = account.Password;
            accountCurrent.Salt = account.Salt;
            accountCurrent.FirstLogin = false;
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
