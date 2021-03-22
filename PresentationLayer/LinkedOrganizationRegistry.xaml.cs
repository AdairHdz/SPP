using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using FluentValidation.Results;
using PresentationLayer.Validators;
using Utilities;
using System.Data.Entity.Core;
using DataPersistenceLayer.Repositories;


namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for LinkedOrganizationRegistry.xaml
    /// </summary>
    public partial class LinkedOrganizationRegistry : Window
    {
        private LinkedOrganization LinkedOrganization = new LinkedOrganization();
        private List<Phone> PhonesNumbers = new List<Phone>();

        public LinkedOrganizationRegistry()
        {
            InitializeComponent();
            this.DataContext = LinkedOrganization;

            ComboBoxCity.DisplayMemberPath = "NameCity";
            ComboBoxCity.SelectedValuePath = "IdCity";
            ComboBoxCity.ItemsSource = RecoverCities().ToList();

            ComboBoxState.DisplayMemberPath = "NameState";
            ComboBoxState.SelectedValuePath = "IdState";
            ComboBoxState.ItemsSource = RecoverStates().ToList();

            ComboBoxSector.DisplayMemberPath = "NameSector";
            ComboBoxSector.SelectedValuePath = "IdSector";
            ComboBoxSector.ItemsSource = RecoverSectors().ToList();
        }

        private IEnumerable<City> RecoverCities()
        {
            IEnumerable<City> Cities = null;
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            Cities = unitOfWork.Cities.GetAll();
            return Cities;
        }

        private IEnumerable<State> RecoverStates()
        {
            IEnumerable<State> States = null;
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            States = unitOfWork.States.GetAll();
            return States;
        }

        private IEnumerable<Sector> RecoverSectors()
        {
            IEnumerable<Sector> Sectors = null;
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            Sectors = unitOfWork.Sectors.GetAll();
            return Sectors;
        }

        private void RegisterButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateLinkedOrganizationFromInputData();
            if (IsValidData())
            {
                if (!LinkedOrganizationIsAlreadyRegistered())
                {
                    if (RegisterNewLinkedOrganization())
                    {
                        MessageBox.Show("Organización vinculada registrado exitosamente");
                    }
                }
                else
                {
                    MessageBox.Show("Esta Organización vinculada ya está registrado");
                }
            }
            else
            {
                MessageBox.Show("Datos no válidos");
            }
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea cancelar?",
                "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void TextBoxKeyDownOnlyNumber(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }

        }

        private void CreateLinkedOrganizationFromInputData()
        {
            LinkedOrganization.Name = TextBoxName.Text;
            LinkedOrganization.Address = TextBoxAddress.Text;
            LinkedOrganization.Email = TextBoxEmail.Text;

            CreatePhonesFromInputData();

            LinkedOrganization.DirectUsers = TextBoxDirectUsers.Text;
            
            LinkedOrganization.IndirectUsers = TextBoxIndirectUsers.Text;
            
            if (ComboBoxCity.SelectedValue != null)
            {
                LinkedOrganization.IdCity = (int)ComboBoxCity.SelectedValue;
            }
            if (ComboBoxState.SelectedValue != null)
            {
                LinkedOrganization.IdState = (int)ComboBoxState.SelectedValue;
            }
            if (ComboBoxSector.SelectedValue != null)
            {
                LinkedOrganization.IdSector = (int)ComboBoxSector.SelectedValue;
            }

            LinkedOrganization.LinkedOrganizationStatus = LinkedOrganizationStatus.ACTIVE;
        }

        private void CreatePhonesFromInputData()
        {
            PhonesNumbers.Clear();
            PhonesNumbers.Add(new Phone()
            {
                Extension = TextBoxExtension.Text,
                PhoneNumber = TextBoxPhoneNumber.Text,
                IdLinkedOrganization = LinkedOrganization.IdLinkedOrganization
            });

            if (TextBoxExtension2.Text.Length > 0 || TextBoxPhoneNumber2.Text.Length > 0)
            {
                PhonesNumbers.Add(new Phone()
                {
                    Extension = TextBoxExtension2.Text,
                    PhoneNumber = TextBoxPhoneNumber2.Text,
                    IdLinkedOrganization = LinkedOrganization.IdLinkedOrganization
                });
            }
            
            LinkedOrganization.PhoneNumbers = PhonesNumbers;
        }

        private bool IsValidData()
        {
            LinkedOrganizationValidator linkedOrganizationValidator = new LinkedOrganizationValidator();
            FluentValidation.Results.ValidationResult dataValidationResult = linkedOrganizationValidator.Validate(LinkedOrganization);
            IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
            UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
            userFeedback.ShowFeedback();
            return dataValidationResult.IsValid;
        }

        private bool LinkedOrganizationIsAlreadyRegistered()
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            bool linkedOrganizationIsAlreadyRegistered = unitOfWork.LinkedOrganizations.LinkedOrganizationIsAlreadyRegistered(LinkedOrganization);
            unitOfWork.Dispose();
            return linkedOrganizationIsAlreadyRegistered;
        }

        private bool RegisterNewLinkedOrganization()
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            unitOfWork.LinkedOrganizations.Add(LinkedOrganization);
            int rowsAffected = unitOfWork.Complete();
            unitOfWork.Dispose();
            return rowsAffected >= 2;
        }
    }

}