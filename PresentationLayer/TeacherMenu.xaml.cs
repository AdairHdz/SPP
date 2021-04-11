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
        public TeacherMenu()
        {
            InitializeComponent();
        }

        private void LogOutButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            Login login = new Login();
            login.Show();
            Close();
        }

        private void EvaluateReportButtonClicked(object sender, RoutedEventArgs e)
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
                MessageBox.Show("No se encontro actividades. Intente más tarde", "No se puede acceder", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
