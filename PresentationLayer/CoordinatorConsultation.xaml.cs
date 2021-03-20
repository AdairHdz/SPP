using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para CoordinatorConsultation.xaml
    /// </summary>
    public partial class CoordinatorConsultation : Window
    {
        public CoordinatorConsultation()
        {
            InitializeComponent();            
        }


        //Ya
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

        //Ya
        private void SearchButtonClicked(object sender, RoutedEventArgs e)
        {            
            IList<Coordinator> retrievedCoordinators = GetMatchingResults();
            if(retrievedCoordinators.Count == 0)
            {
                MessageBox.Show("No se encontraron registros");
                this.Close();
            }
            DisplayCoordinatorsData(retrievedCoordinators);
        }

        //Ya
        private IList<Coordinator> GetMatchingResults()
        {
            IList<Coordinator> retrievedCoordinators = new List<Coordinator>();
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            string optionFilter = ((ComboBoxItem)ComboBoxFilter.SelectedItem).Content.ToString();
            try
            {
                switch (optionFilter)
                {
                    case "Todos":
                        retrievedCoordinators = unitOfWork.Coordinators.GetAllCoordinatorsWithUserData();
                        break;
                    case "Activos":
                        retrievedCoordinators = unitOfWork.Coordinators.GetCoordinatorsWithUserData(coordinator =>
                            coordinator.User.UserStatus == UserStatus.ACTIVE);
                        break;
                    case "Inactivos":
                        retrievedCoordinators = unitOfWork.Coordinators.GetCoordinatorsWithUserData(coordinator =>
                            coordinator.User.UserStatus == UserStatus.INACTIVE);
                        break;
                    case "Nombre":
                        retrievedCoordinators = unitOfWork.Coordinators.GetCoordinatorsWithUserData(coordinator => coordinator.User
                            .Name.ToUpperInvariant().Contains(TextBoxSearch.Text.ToUpperInvariant()));
                        break;
                    case "Apellido":
                        retrievedCoordinators = unitOfWork.Coordinators.GetCoordinatorsWithUserData(coordinator => coordinator.User
                            .LastName.ToUpperInvariant().Contains(TextBoxSearch.Text.ToUpperInvariant()));
                        break;
                    case "Correo":
                        retrievedCoordinators = unitOfWork.Coordinators.GetCoordinatorsWithUserData(coordinator => coordinator.User
                            .Email.Contains(TextBoxSearch.Text));
                        break;
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("No se pudo obtener información de la base de datos. Intente más tarde",
                    "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                unitOfWork.Dispose();
            }
            return retrievedCoordinators;
        }

        ///Ya
        private void ModifyButtonClicked(object sender, RoutedEventArgs e)
        {
            Coordinator coordinator = (Coordinator)ListViewCoordinator.SelectedItem;
            CoordinatorModification coordinatorModification = new CoordinatorModification(coordinator.StaffNumber);
            coordinatorModification.Show();
            this.Close();
        }

        ///Ya
        private void DeleteButtonClicked(object sender, RoutedEventArgs e)
        {            
            CoordinatorDeletion coordinatorDeletion = new CoordinatorDeletion();
            coordinatorDeletion.Show();
            this.Close();
        }

        //Ya
        private void DisplayCoordinatorsData(IList<Coordinator> coordinators)
        {
            ListViewCoordinator.Items.Clear();
            foreach (Coordinator coordinator in coordinators)
            {
                ListViewCoordinator.Items.Add(coordinator);
            }
        }

        //Ya
        private void BackButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Ya
        private void CoordinatorListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Coordinator coordinator = ((Coordinator)ListViewCoordinator.SelectedItem);
            if(coordinator != null)
            {
                ButtonModify.IsEnabled = true;
                if (coordinator.User.UserStatus == UserStatus.ACTIVE)
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
