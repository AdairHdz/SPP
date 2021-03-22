
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

    }
}
