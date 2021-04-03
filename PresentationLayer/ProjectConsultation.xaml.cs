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
    /// Lógica de interacción para ProjectConsultation.xaml
    /// </summary>
    public partial class ProjectConsultation : Window
    {
        public ProjectConsultation()
        {
            InitializeComponent();
        }

        private void ModifyButtonClicked(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButtonClicked(object sender, RoutedEventArgs e)
        {

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

        private void SearchButtonClicked(object sender, RoutedEventArgs e)
        {
            IList<Project> retrievedProjects = GetMatchingResults();
            if (retrievedProjects.Count == 0)
            {
                MessageBox.Show("No se encontraron registros");
            }
            DisplayProjectsData(retrievedProjects);
        }

        private void DisplayProjectsData(IList<Project> projects)
        {
            ListViewProject.Items.Clear();
            foreach (Project project in projects)
            {
                ListViewProject.Items.Add(project);
            }
        }

        private IList<Project> GetMatchingResults()
        {
            IList<Project> retrievedProjects = new List<Project>();
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            string optionFilter = ((ComboBoxItem)ComboBoxFilter.SelectedItem).Content.ToString();
            try
            {
                switch (optionFilter)
                {
                    case "Todos":
                        retrievedProjects = unitOfWork.Projects.GetAll().ToList();
                        break;
                    case "Activos":
                        retrievedProjects = unitOfWork.Projects.Find(project =>
                            project.Status == ProjectStatus.ACTIVE).ToList();
                        break;
                    case "Inactivos":
                        retrievedProjects = unitOfWork.Projects.Find(project =>
                            project.Status == ProjectStatus.INACTIVE).ToList();
                        break;
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
            return retrievedProjects;
        }

        private void NotifyErrorAndExit()
        {
            MessageBox.Show("No se pudo obtener información de la base de datos. Intente más tarde",
                    "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
            GoBackToCoordinatorMenu();
        }

        private void CoordinatorListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Project project = ((Project)ListViewProject.SelectedItem);
            if (project != null)
            {
                ButtonModify.IsEnabled = true;
                if (project.Status == ProjectStatus.ACTIVE)
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

        private void GoBackToCoordinatorMenu()
        {
            CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
            coordinatorMenu.Show();
            Close();
        }
    }
}
