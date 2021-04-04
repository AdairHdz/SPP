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
    /// Lógica de interacción para ChooseLinkedOrganization.xaml
    /// </summary>
    public partial class LinkedOrganizationChoose : Window
    {
        private bool handle = true;
        private string textSearch;
        private string optionFilter;
        private bool isFilterWithText;
        private static LinkedOrganization linkedOrganization;
        public LinkedOrganizationChoose()
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
                ListViewLinkedOrganization.Items.Clear();
                IEnumerable<LinkedOrganization> linkedOrganizations = ConsultLinkedOrganization();
                if (linkedOrganizations.Count() > 0)
                {
                    AddLinkedOrganizationsInListView(linkedOrganizations);
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

        private void AddLinkedOrganizationsInListView(IEnumerable<LinkedOrganization> linkedOrganizations)
        {
            foreach (LinkedOrganization linkedOrganization in linkedOrganizations)
            {
                ListViewLinkedOrganization.Items.Add(linkedOrganization);
            }
        }

        private IEnumerable<LinkedOrganization> ConsultLinkedOrganization()
        {
            IEnumerable<LinkedOrganization> linkedOrganizations = null;
            try
            {
                ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                switch (optionFilter)
                {
                    case "Todos":
                        linkedOrganizations = unitOfWork.LinkedOrganizations.Find(LinkedOrganization => LinkedOrganization.LinkedOrganizationStatus == LinkedOrganizationStatus.ACTIVE);
                        break;
                    case "Nombre":
                        linkedOrganizations = unitOfWork.LinkedOrganizations.Find(LinkedOrganization => LinkedOrganization.Name.ToUpperInvariant().Contains(textSearch.ToUpperInvariant()) && LinkedOrganization.LinkedOrganizationStatus == LinkedOrganizationStatus.ACTIVE);
                        break;
                    case "Ciudad":
                        linkedOrganizations = unitOfWork.LinkedOrganizations.Find(LinkedOrganization => LinkedOrganization.City.NameCity.ToUpperInvariant().Contains(textSearch.ToUpperInvariant()) && LinkedOrganization.LinkedOrganizationStatus == LinkedOrganizationStatus.ACTIVE);
                        break;
                    case "Estado":
                        linkedOrganizations = unitOfWork.LinkedOrganizations.Find(LinkedOrganization => LinkedOrganization.State.NameState.ToUpperInvariant().Contains(textSearch.ToUpperInvariant()) && LinkedOrganization.LinkedOrganizationStatus == LinkedOrganizationStatus.ACTIVE);
                        break;
                    case "Sector":
                        linkedOrganizations = unitOfWork.LinkedOrganizations.Find(LinkedOrganization => LinkedOrganization.Sector.NameSector.ToUpperInvariant().Contains(textSearch.ToUpperInvariant()) && LinkedOrganization.LinkedOrganizationStatus == LinkedOrganizationStatus.ACTIVE);
                        break;
                    case "Correo":
                        linkedOrganizations = unitOfWork.LinkedOrganizations.Find(LinkedOrganization => LinkedOrganization.Email.Equals(textSearch) && LinkedOrganization.LinkedOrganizationStatus == LinkedOrganizationStatus.ACTIVE);
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
            return linkedOrganizations;
        }

        private void LinkedOrganizationListViewSelectionChanged(object sender, SelectionChangedEventArgs selectionChanged)
        {
            LinkedOrganization linkedOrganization = ((LinkedOrganization)ListViewLinkedOrganization.SelectedItem);
            if (linkedOrganization != null)
            {
                ButtonSave.IsEnabled = true;
            }
        }

        private void SaveButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            LinkedOrganization linkedOrganizationObtein = ((LinkedOrganization)ListViewLinkedOrganization.SelectedItem);
            linkedOrganization = linkedOrganizationObtein;
            Close();
        }

        public static LinkedOrganization ObteinLinkedOrganization()
        {
            return linkedOrganization;
        }
    }
}
