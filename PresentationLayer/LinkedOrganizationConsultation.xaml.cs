using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para LinkedOrganizationConsultation.xaml
    /// </summary>
    public partial class LinkedOrganizationConsultation : Window
    {
        public LinkedOrganizationConsultation()
        {
            InitializeComponent();
        }

        private void ModifyButtonClicked(object sender, RoutedEventArgs e)
        {
            LinkedOrganization selectedLinkedOrganization = (LinkedOrganization)ListViewLinkedOrganization.SelectedItem;
            int idOfSelectedLinkedOrganization = selectedLinkedOrganization.IdLinkedOrganization;
            LinkedOrganizationModification linkedOrganizationModification = new LinkedOrganizationModification(idOfSelectedLinkedOrganization);
            linkedOrganizationModification.Show();
            this.Close();
            
        }

        private void DeleteButtonClicked(object sender, RoutedEventArgs e)
        {
            LinkedOrganization selectedLinkedOrganization = (LinkedOrganization)ListViewLinkedOrganization.SelectedItem;
            int idOfSelectedLinkedOrganization = selectedLinkedOrganization.IdLinkedOrganization;
            LinkedOrganizationDeletion linkedOrganizationDeletion = new LinkedOrganizationDeletion(idOfSelectedLinkedOrganization);
            linkedOrganizationDeletion.Show();
            this.Close();
        }

        private void SearchButtonClicked(object sender, RoutedEventArgs e)
        {
            IList<LinkedOrganization> retrievedLinkedOrganizations = GetMatchingResults();
            if (retrievedLinkedOrganizations.Count == 0)
            {
                MessageBox.Show("No se encontraron registros");                
            }
            DisplayLinkedOrganizationsData(retrievedLinkedOrganizations);
        }

        private IList<LinkedOrganization> GetMatchingResults()
        {
            IEnumerable<LinkedOrganization> retrievedLinkedOrganizations = new List<LinkedOrganization>();
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            string optionFilter = ((ComboBoxItem)ComboBoxFilter.SelectedItem).Content.ToString();
            IList<LinkedOrganization> linkedOrganizationsList = new List<LinkedOrganization>();
            try
            {
                switch (optionFilter)
                {
                    case "Todos":
                        retrievedLinkedOrganizations = unitOfWork.LinkedOrganizations.GetAll();
                        break;
                    case "Activos":
                        retrievedLinkedOrganizations = unitOfWork.LinkedOrganizations.Find(linkedOrg =>
                        linkedOrg.LinkedOrganizationStatus == LinkedOrganizationStatus.ACTIVE);
                        break;
                    case "Inactivos":
                        retrievedLinkedOrganizations = unitOfWork.LinkedOrganizations.Find(linkedOrg =>
                            linkedOrg.LinkedOrganizationStatus == LinkedOrganizationStatus.INACTIVE);
                        break;
                    case "Nombre":
                        retrievedLinkedOrganizations = unitOfWork.LinkedOrganizations.Find(linkedOrg => linkedOrg.Name
                            .ToUpperInvariant().Contains(TextBoxSearch.Text.ToUpperInvariant()));
                        break;
                    case "Correo":
                        retrievedLinkedOrganizations = unitOfWork.LinkedOrganizations.Find(linkedOrg => linkedOrg.Email
                            .Contains(TextBoxSearch.Text));
                        break;
                }

                if (retrievedLinkedOrganizations.Count() > 0)
                {
                    linkedOrganizationsList = retrievedLinkedOrganizations.ToList();
                }
            }
            catch (SqlException)
            {
                NotifyErrorAndExit();                
            }
            catch (EntityException)
            {
                NotifyErrorAndExit();
            }
            finally
            {
                unitOfWork.Dispose();
            }

            return linkedOrganizationsList;
        }

        private void NotifyErrorAndExit()
        {
            MessageBox.Show("No se pudo obtener información de la base de datos. Intente más tarde",
                    "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
            GoBackToCoordinatorMenu();
        }

        private void GoBackToCoordinatorMenu()
        {
            CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
            coordinatorMenu.Show();
            this.Close();
        }

        private void DisplayLinkedOrganizationsData(IList<LinkedOrganization> linkedOrganizations)
        {
            ListViewLinkedOrganization.Items.Clear();
            foreach (LinkedOrganization linkedOrganization in linkedOrganizations)
            {
                ListViewLinkedOrganization.Items.Add(linkedOrganization);
            }
        }

        private void FilterComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonSearch.IsEnabled = true;
            string optionFilter = ((ComboBoxItem)ComboBoxFilter.SelectedItem).Content.ToString();
            if (optionFilter.Equals("Todos") || optionFilter.Equals("Activos") || optionFilter.Equals("Inactivos"))
            {
                TextBoxSearch.IsEnabled = false;
            }
            else
            {
                TextBoxSearch.IsEnabled = true;
            }
        }

        private void BackButtonClicked(object sender, RoutedEventArgs e)
        {
            GoBackToCoordinatorMenu();
        }

        private void LinkedOrganizationListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LinkedOrganization linkedOrganization = ((LinkedOrganization)ListViewLinkedOrganization.SelectedItem);
            if (linkedOrganization != null)
            {
                ButtonModify.IsEnabled = true;
                if (linkedOrganization.LinkedOrganizationStatus == LinkedOrganizationStatus.ACTIVE)
                {
                    ButtonDelete.IsEnabled = true;
                }
                else
                {
                    ButtonDelete.IsEnabled = false;
                }
            }
            else
            {
                ButtonModify.IsEnabled = false;
                ButtonDelete.IsEnabled = false;
            }
        }
    }
}
