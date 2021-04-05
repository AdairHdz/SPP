

using DataPersistenceLayer.Entities;
using System.Windows;


namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para PracticionerMenu.xaml
    /// </summary>
    public partial class PracticionerMenu : Window
    {
        public PracticionerMenu()
        {
            InitializeComponent();
        }
        private void LogOutButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            Login login = new Login();
            login.Show();
            Close();
        }

        private void AddPartialReport(object sender, RoutedEventArgs e)
        {
            ReportList reportList = new ReportList();
            if (reportList.InitializeStackPanel(ActivityType.PartialReport))
            {
                reportList.Show();
                Close();
            }
            else
            {
                MessageBox.Show("No se encontro actividades. Intente más tarde", "Ingreso Faliido", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddMonthlyReport(object sender, RoutedEventArgs e)
        {
            ReportList reportList = new ReportList();
            if (reportList.InitializeStackPanel(ActivityType.MonthlyReport))
            {
                reportList.Show();
                Close();
            }
            else
            {
                MessageBox.Show("No se encontro actividades. Intente más tarde", "Ingreso Faliido", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
