using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using System.Data.Entity.Core;
using System.Windows;
using System.Windows.Media;
using Utilities;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private Account account;
        private User user;
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
                if (IsAccount(unitOfWork))
                {
                    PasswordBoxPassword.BorderBrush = Brushes.Green;
                    TextBoxUser.BorderBrush = Brushes.Green;
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
                    PasswordBoxPassword.BorderBrush = Brushes.Red;
                    TextBoxUser.BorderBrush = Brushes.Red;
                    MessageBox.Show("Contraseña o Usuario incorrectos", "Login Fallido", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (EntityException)
            {
                MessageBox.Show("No se pudo Iniciar sesión. Intente más tarde", "Inicio de sesión Fallido", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private bool IsActiveUser(UnitOfWork unitOfWork)
        {
            user = unitOfWork.Users.FindFirstOccurence(userObtein => userObtein.IdAccount == account.IdAccount);
            return user.UserStatus == UserStatus.ACTIVE;
        }

        private bool IsAccount(UnitOfWork unitOfWork)
        {
            string user = TextBoxUser.Text;
            account = unitOfWork.Accounts.FindFirstOccurence(accountObtein => accountObtein.Username == user);
            return !object.ReferenceEquals(null, account) && IsValidAccountPassword();
        }

        private void OpenWindowUser()
        {
            if (!account.FirstLogin) {
                if (user.UserType == UserType.Coordinator)
                {
                    CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
                    coordinatorMenu.Show();
                    Close();
                }
                else
                {
                    if (user.UserType == UserType.Practicioner)
                    {
                        PracticionerMenu practicionerMenu = new PracticionerMenu();
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
                            TeacherMenu teacherMenu = new TeacherMenu();
                            teacherMenu.Show();
                            Close();
                        }
                    }
                }
            }
            else
            {
                FirstLogin firstLogin = new FirstLogin();
                firstLogin.InitializeAccount(account);
                firstLogin.Show();
                Close();
            }
        }

        private bool IsValidAccountPassword()
        {
            BCryptHashGenerator bCryptHashGenerator = new BCryptHashGenerator();
            string hashedPassword = bCryptHashGenerator.GenerateHashedString(PasswordBoxPassword.Password, account.Salt);
            return hashedPassword.Equals(account.Password);
        }

    }
}
