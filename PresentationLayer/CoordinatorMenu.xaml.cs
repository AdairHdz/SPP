using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para CoordinatorMenu.xaml
    /// </summary>
    public partial class CoordinatorMenu : Window
    {
        public CoordinatorMenu()
        {
            InitializeComponent();
        }

        private void ConsultResponsibleProjectButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            ListResponsibleProject listResponsibleProject = new ListResponsibleProject();
            listResponsibleProject.Show();
            Close();
        }
        private void RegisterResponsibleProjectButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            RegisterResponsableProject registerResponsable = new RegisterResponsableProject();
            registerResponsable.Show();
            Close();
        }
    }
}
