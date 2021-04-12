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
    /// Lógica de interacción para ManageReportsAddActivity.xaml
    /// </summary>
    public partial class ManageReportsAddActivity : Window
    {
        private string _staffNumber;
        public ManageReportsAddActivity(string staffNumber)
        {
            _staffNumber = staffNumber;
            InitializeComponent();
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
        }

        private void AddButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
           
        }
    }
}
