using System.Windows;
using DataPersistenceLayer.Entities;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para ModifyResponsibleProject.xaml
    /// </summary>
    public partial class ModifyResponsibleProject : Window
    {
        public ModifyResponsibleProject()
        {
            InitializeComponent();
        }
        public void InitializeDataResponsibleProject(ResponsibleProject responsibleProject)
        {
            NameTextBox.Text = responsibleProject.Name;
            LastNameTextBox.Text = responsibleProject.LastName;
            EmailTextBox.Text = responsibleProject.EmailAddress;
            ChargeTextBox.Text = responsibleProject.Charge;
        }
    }
}
