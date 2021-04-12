using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Windows;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para TeacherMenu.xaml
    /// </summary>
    public partial class TeacherMenu : Window
    {
        public static User _User { get; set; }
        private readonly string _staffNumber;
        public TeacherMenu()
        {
            InitializeComponent();
        }

        public TeacherMenu(string staffNumber)
        {
            InitializeComponent();
            _staffNumber = staffNumber;
        }

        private void LogOutButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            Login login = new Login();
            login.Show();
            Close();
        }

        private void EvaluateReportButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            ActivityReportList activityReportList = new ActivityReportList();
            ActivityReportList._User = _User;
            if (activityReportList.InitializeStackPanel())
            {
                activityReportList.Show();
                Close();
            }
            else
            {
                MessageBox.Show("No se encontro actividades. Intente más tarde", "Ingreso Faliido", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ManageReportButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            try
            {
                if (unitOfWork.Groups.GetIfThisTeacherHaveActiveGroups(_staffNumber))
                {
                    ManageReports manageReports = new ManageReports(_staffNumber);
                    manageReports.Show();
                    Close();
                } else
                {
                    MessageBox.Show("No tiene grupos asignados en estado activo", "Ingreso Faliido", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (EntityException)
            {
                MessageBox.Show("No hay conexión a la base de datos. Por favor intente más tarde");
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }
    }
}
