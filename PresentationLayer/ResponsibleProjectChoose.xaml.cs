using System.Windows;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Windows.Controls;
using System;
using System.Linq;


namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para ChooseResponsibleProject.xaml
    /// </summary>
    public partial class ResponsibleProjectChoose : Window
    {
        private bool handle = true;
        private string textSearch;
        private string optionFilter;
        private bool isFilterWithText; 
        private static ResponsibleProject responsibleProject;
        public ResponsibleProjectChoose()
        {
            InitializeComponent();
        }

        private void FilterComboBoxDropDownClosed(object sender, EventArgs eventArgs)
        {
            if (handle)
            {
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

        private void CancelButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
        }

        private void DisableSearch()
        {
            if (ComboBoxFilter.SelectedItem != null)
            {
                ButtonSearch.IsEnabled = true;
                optionFilter = ((ComboBoxItem)ComboBoxFilter.SelectedItem).Content.ToString();
                if (optionFilter.Equals("Todos"))
                {
                    TextBoxSearch.IsEnabled = false;
                    isFilterWithText = false;
                }
                else
                {
                    TextBoxSearch.IsEnabled = true;
                    isFilterWithText = true;
                }
            }
        }

        private void SearchButtonClicked(object sender, RoutedEventArgs routedEvent)
        {
            textSearch = TextBoxSearch.Text;
            optionFilter = ((ComboBoxItem)ComboBoxFilter.SelectedItem).Content.ToString();
            if (ValidateIsTextSearch())
            {
                ButtonSave.IsEnabled = false;
                ListViewResponsibleProject.Items.Clear();
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
            if (String.IsNullOrWhiteSpace(textSearch) && isFilterWithText)
            {
                return false;
            }
            return true;
        }

        private void AddResponsiblesProjectsInListView(IEnumerable<ResponsibleProject> responsiblesProjects)
        {
            foreach (ResponsibleProject responsibleProject in responsiblesProjects)
            {
                ListViewResponsibleProject.Items.Add(responsibleProject);
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
                        responsiblesProjects = unitOfWork.ResponsibleProjects.Find(ResponsibleProject => ResponsibleProject.ResponsibleProjectStatus == ResponsibleProjectStatus.ACTIVE);
                        break;
                    case "Nombre":
                        responsiblesProjects = unitOfWork.ResponsibleProjects.Find(ResponsibleProject => ResponsibleProject.Name.ToUpperInvariant().Contains(textSearch.ToUpperInvariant()) && ResponsibleProject.ResponsibleProjectStatus == ResponsibleProjectStatus.ACTIVE);
                        break;
                    case "Apellido":
                        responsiblesProjects = unitOfWork.ResponsibleProjects.Find(ResponsibleProject => ResponsibleProject.LastName.ToUpperInvariant().Contains(textSearch.ToUpperInvariant()) && ResponsibleProject.ResponsibleProjectStatus == ResponsibleProjectStatus.ACTIVE);
                        break;
                    case "Correo":
                        responsiblesProjects = unitOfWork.ResponsibleProjects.Find(ResponsibleProject => ResponsibleProject.EmailAddress.Equals(textSearch) && ResponsibleProject.ResponsibleProjectStatus == ResponsibleProjectStatus.ACTIVE);
                        break;
                    case "Cargo":
                        responsiblesProjects = unitOfWork.ResponsibleProjects.Find(ResponsibleProject => ResponsibleProject.Charge.ToUpperInvariant().Contains(textSearch.ToUpperInvariant()) && ResponsibleProject.ResponsibleProjectStatus == ResponsibleProjectStatus.ACTIVE);
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
            ResponsibleProject responsible = ((ResponsibleProject)ListViewResponsibleProject.SelectedItem);
            if (responsible != null)
            {
                ButtonSave.IsEnabled = true;
            }
        }

        private void SaveButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            ResponsibleProject responsibleProjectObtein = ((ResponsibleProject)ListViewResponsibleProject.SelectedItem);
            responsibleProject = responsibleProjectObtein;
            Close();
        }

        public static ResponsibleProject ObteinResponsibleProject()
        {
            return responsibleProject;
        }
    }
}
