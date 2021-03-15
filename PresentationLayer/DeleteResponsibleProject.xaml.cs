using System.Windows;
using DataPersistenceLayer.Entities;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para DeleteResponsibleProject.xaml
    /// </summary>
    public partial class DeleteResponsibleProject : Window
    {
        public DeleteResponsibleProject()
        {
            InitializeComponent();
        }

        public void InitializeDataResponsibleProject(ResponsibleProject responsibleProject)
        {
            NameLabel.Content = responsibleProject.Name;
            LastNameLabel.Content = responsibleProject.LastName;
            EmailLabel.Content = responsibleProject.EmailAddress;
            ChargeLabel.Content = responsibleProject.Charge;
        }
    }
}
