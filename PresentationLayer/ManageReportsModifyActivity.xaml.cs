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
    /// Lógica de interacción para ManageReportsModifyActivity.xaml
    /// </summary>
    public partial class ManageReportsModifyActivity : Window
    {
        private string _staffNumber;
        public ManageReportsModifyActivity(string staffNumber, int idActivity)
        {
            _staffNumber = staffNumber;
            InitializeComponent();
        }
        private void CancelButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
        }

        private void ModifyButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {

        }
    }
}
