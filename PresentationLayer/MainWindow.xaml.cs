using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using System;
using System.Windows;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TryDatabaseButtonClicked(object sender, RoutedEventArgs e)
        {
            UnitOfWork unitOfWork = new UnitOfWork(new ProfessionalPracticesContext());
            var user = unitOfWork.Users.Get(1);
            UserNameLabel.Content = user.Name;
            Console.WriteLine(user.Name);
        }

        private void ModifyUsernameButtonClicked(object sender, RoutedEventArgs e)
        {
            /*UnitOfWork unitOfWork = new UnitOfWork(new ProfessionalPracticesContext());
            var user = unitOfWork.Users.Get(1);
            user.Name = "Ángel José";
            unitOfWork.Complete();
            unitOfWork.Dispose();
            UserNameLabel.Content = "Se modificó el nombre. Vuelve a cargarlo para comprobar";*/
            RegisterResponsableProject register = new RegisterResponsableProject();
            register.Show();
            this.Close();
        }
    }
}
