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
    public partial class PracticionerConsult : Window
    {
        private bool handle = true;
        private string textSearch;
        private string optionFilter;
        private bool isFilterWithText;
        public PracticionerConsult()
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

        private void BehindButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
            coordinatorMenu.Show();
            Close();
        }

        private void DisableSearch()
        {
            if (ComboBoxFilter.SelectedItem != null)
            {
                ButtonSearch.IsEnabled = true;
                optionFilter = ((ComboBoxItem)ComboBoxFilter.SelectedItem).Content.ToString();
                if (optionFilter.Equals("Todos") || optionFilter.Equals("Activos") || optionFilter.Equals("Inactivos") 
                    || optionFilter.Equals("Hombres") || optionFilter.Equals("Mujeres"))
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
                ButtonDelete.IsEnabled = false;
                ButtonModify.IsEnabled = false;
                ListViewPracticioners.Items.Clear();
                IEnumerable<Practicioner> practicioners = ConsultPracticioner();
                if (practicioners.Count() > 0)
                {
                    AddPracticionersInListView(practicioners);
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

        private void AddPracticionersInListView(IEnumerable<Practicioner> practicioners)
        {
            foreach (Practicioner practioner in practicioners)
            {
                ListViewPracticioners.Items.Add(practioner);
            }
        }

        private IEnumerable<Practicioner> ConsultPracticioner()
        {
            IEnumerable<Practicioner> practicioner = null;
            try
            {
                ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                switch (optionFilter)
                {
                    case "Todos":
                        practicioner = unitOfWork.Practicioners.GetAll();
                        break;
                    case "Activos":
                        practicioner = unitOfWork.Practicioners.Find(Practicioner => Practicioner.User.UserStatus == UserStatus.ACTIVE);
                        break;
                    case "Inactivos":
                        practicioner = unitOfWork.Practicioners.Find(Practicioner => Practicioner.User.UserStatus == UserStatus.INACTIVE);
                        break;
                    case "Nombre":
                        practicioner = unitOfWork.Practicioners.Find(Practicioner => Practicioner.User.Name.ToUpperInvariant().Contains(textSearch.ToUpperInvariant()));
                        break;
                    case "Apellido":
                        practicioner = unitOfWork.Practicioners.Find(Practicioner => Practicioner.User.LastName.ToUpperInvariant().Contains(textSearch.ToUpperInvariant()));
                        break;
                    case "Correo":
                        practicioner = unitOfWork.Practicioners.Find(Practicioner => Practicioner.User.Email.ToUpperInvariant().Contains(textSearch.ToUpperInvariant())); ;
                        break;
                    case "Periodo":
                        practicioner = unitOfWork.Practicioners.Find(Practicioner => Practicioner.Term.ToUpperInvariant().Contains(textSearch.ToUpperInvariant())); ;
                        break;
                    case "Teléfono":
                        practicioner = unitOfWork.Practicioners.Find(Practicioner => Practicioner.User.PhoneNumber.Equals(textSearch));
                        break;
                    case "CorreoAlterno":
                        practicioner = unitOfWork.Practicioners.Find(Practicioner => Practicioner.User.AlternateEmail.ToUpperInvariant().Contains(textSearch.ToUpperInvariant()));
                        break;
                    case "Matrícula":
                        practicioner = unitOfWork.Practicioners.Find(Practicioner => Practicioner.Enrollment.Equals(textSearch));
                        break;
                    case "Hombres":
                        practicioner = unitOfWork.Practicioners.Find(Practicioner => Practicioner.User.Gender == Gender.MALE);
                        break;
                    case "Mujeres":
                        practicioner = unitOfWork.Practicioners.Find(Practicioner => Practicioner.User.Gender == Gender.FEMALE);
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
            return practicioner;
        }

        private void PracticionerListViewSelectionChanged(object sender, SelectionChangedEventArgs selectionChanged)
        {
            Practicioner practicioner = ((Practicioner)ListViewPracticioners.SelectedItem);
            if (practicioner != null)
            {
                ButtonModify.IsEnabled = true;
                if (practicioner.User.UserStatus == UserStatus.ACTIVE)
                {
                    ButtonDelete.IsEnabled = true;
                }
                else
                {
                    ButtonDelete.IsEnabled = false;
                }
            }
        }

        private void DeleteButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            Practicioner practicioner = ((Practicioner)ListViewPracticioners.SelectedItem);
            try
            {
                ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                bool practicionerCantBeDeleted = unitOfWork.Practicioners.PracticionerHasActiveProject(practicioner.Enrollment);
                unitOfWork.Practicioners.PracticionerHasActiveProject(practicioner.Enrollment);
                if (practicionerCantBeDeleted)
                {
                    MessageBox.Show("El practicante tiene asignado un proyecto activo, no se puede eliminar", "No se puede eliminar", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    PracticionerDeletion practicionerDeletion = new PracticionerDeletion();
                    practicionerDeletion.InitializeDataPracticioner(practicioner);
                    practicionerDeletion.Show();
                    Close();
                }
            }
            catch (EntityException)
            {
                MessageBox.Show("No hay conexión con la base de datos. Por favor intente más tarde", "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ModifyButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {

        }
    }
}