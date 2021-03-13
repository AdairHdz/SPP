using System;
using System.Collections.Generic;
using System.Linq;
using DataPersistenceLayer.Entities;
using System.Windows;
using System.Windows.Controls;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using System.Windows.Input;
using System.Data.Entity.Core;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para ListResponsibleProject.xaml
    /// </summary>
    public partial class ListResponsibleProject : Window
    {
        private bool handle = true;
        private string textSearch;
        private string optionFilter;
        public ListResponsibleProject()
        {
            InitializeComponent();
        }

        private void FilterComboBoxDropDownClosed(object sender, EventArgs eventArgs)
        {
            if (handle) {
                DisableSearch();
            }
            handle = true;
        }

        private void FilterComboBoxSelectionChanged(object sender, SelectionChangedEventArgs selectionChanged)
        {
            ComboBox FilterSelectComboBox = sender as ComboBox;
            handle = !FilterSelectComboBox.IsDropDownOpen;
            DisableSearch();
        }

        private void DisableSearch()
        {
            if (FilterComboBox.SelectedItem != null)
            {
                string opcionFilter = ((ComboBoxItem)FilterComboBox.SelectedItem).Content.ToString();
                if (opcionFilter.Equals("Todos") || opcionFilter.Equals("Activos")
                    || opcionFilter.Equals("Inactivos"))
                {
                    SearchButton.IsEnabled = true;
                    SearchTextBox.IsEnabled = false;
                }
                else
                {
                    SearchButton.IsEnabled = true;
                    SearchTextBox.IsEnabled = true;
                }
            }
        }

        private void SearchButtonClicked(object sender, RoutedEventArgs routedEvent)
        {
            textSearch = SearchTextBox.Text;
            optionFilter = ((ComboBoxItem)FilterComboBox.SelectedItem).Content.ToString();
            if (ValidateIsTextSearch())
            {
                ResponsibleProjectListView.Items.Clear();
                IEnumerable<ResponsibleProject> responsiblesProjects = ConsultResponsibleProject();
                if (responsiblesProjects.Count() > 0)
                {
                    AddResponsiblesProjectsInListView(responsiblesProjects);
                }
                else
                {
                    MessageBox.Show("No se encontraron registros", "No hay registros", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private bool ValidateIsTextSearch()
        {
            if (String.IsNullOrWhiteSpace(textSearch) && (optionFilter.Equals("Nombre") || optionFilter.Equals("Apellido")
                || optionFilter.Equals("Cargo") || optionFilter.Equals("Correo")))
            {
                return false;
            }
            return true;
        }

        private void AddResponsiblesProjectsInListView(IEnumerable<ResponsibleProject> responsiblesProjects)
        {
            foreach (ResponsibleProject responsibleProject in responsiblesProjects)
            {
                ResponsibleProjectListView.Items.Add(responsibleProject);
            }
        }

        private IEnumerable<ResponsibleProject> ConsultResponsibleProject()
        {
            IEnumerable<ResponsibleProject> responsiblesProjects = null;
            try
            {
                ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                switch (optionFilter)
                {
                    case "Todos":
                        responsiblesProjects = unitOfWork.ResponsibleProjects.GetAll();
                        break;
                    case "Activos":
                        responsiblesProjects = unitOfWork.ResponsibleProjects.Find(ResponsibleProject => ResponsibleProject.ResponsibleProjectStatus == ResponsibleProjectStatus.ACTIVE);
                        break;
                    case "Inactivos":
                        responsiblesProjects = unitOfWork.ResponsibleProjects.Find(ResponsibleProject => ResponsibleProject.ResponsibleProjectStatus == ResponsibleProjectStatus.INACTIVE);
                        break;
                    case "Nombre":
                        responsiblesProjects = unitOfWork.ResponsibleProjects.Find(ResponsibleProject => ResponsibleProject.Name.ToUpperInvariant().Contains(textSearch.ToUpperInvariant()));
                        break;
                    case "Apellido":
                        responsiblesProjects = unitOfWork.ResponsibleProjects.Find(ResponsibleProject => ResponsibleProject.LastName.ToUpperInvariant().Contains(textSearch.ToUpperInvariant())); 
                        break;
                    case "Correo":
                        responsiblesProjects = unitOfWork.ResponsibleProjects.Find(ResponsibleProject => ResponsibleProject.EmailAddress.Equals(textSearch));
                        break;
                    case "Cargo":
                        responsiblesProjects = unitOfWork.ResponsibleProjects.Find(ResponsibleProject => ResponsibleProject.Charge.ToUpperInvariant().Contains(textSearch.ToUpperInvariant()));
                        break;
                    default:
                        MessageBox.Show("Ingrese un filtro valido", "Filtro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                }
            }
            catch (EntityException)
            {
                MessageBox.Show("No se pudo obtener información de la base de datos. Intente más tarde", "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return responsiblesProjects;
        }

        private void ResponsibleProjectListViewSelectionChanged(object sender, SelectionChangedEventArgs selectionChanged)
        {
            
                ResponsibleProject responsible = ((ResponsibleProject)ResponsibleProjectListView.SelectedItem);
                if(responsible.ResponsibleProjectStatus == ResponsibleProjectStatus.ACTIVE)
                {
                    ModifyButton.IsEnabled = true;
                    DeleteButton.IsEnabled = true;
                }
                else
                {
                    ModifyButton.IsEnabled = true;
                    DeleteButton.IsEnabled = false;
                }
        }

        private void DeleteButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            ResponsibleProject responsibleProject = ((ResponsibleProject)ResponsibleProjectListView.SelectedItem);
            try
            {
                ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                bool responsibleProjectIsAssigned = unitOfWork.ResponsibleProjects.ResponsibleProjectIsAssigned(responsibleProject.IdResponsibleProject);
                if (responsibleProjectIsAssigned)
                {
                  MessageBox.Show("El responsable del proyecto está asignado, no se puede eliminar", "Eliminar", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    DeleteResponsibleProject deleteResponsibleProject = new DeleteResponsibleProject();
                    deleteResponsibleProject.InitializeDataResponsibleProject(responsibleProject);
                    deleteResponsibleProject.Show();
                    Close();
                }
            }
            catch (EntityException)
            {
                MessageBox.Show("No se pudo obtener información de la base de datos. Intente más tarde", "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ModifyButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            ResponsibleProject responsibleProject = ((ResponsibleProject)ResponsibleProjectListView.SelectedItem);
            ModifyResponsibleProject modifyResponsibleProject = new ModifyResponsibleProject();
            modifyResponsibleProject.InitializeDataResponsibleProject(responsibleProject);
            modifyResponsibleProject.Show();
            Close();
        }
    }
}
 