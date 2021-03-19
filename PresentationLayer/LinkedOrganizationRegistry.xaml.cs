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
        public LinkedOrganization LinkedOrganization = new LinkedOrganization();
        public Phone PhoneOne = new Phone();
        public Phone PhoneTwo = new Phone();

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
            if (unitOfWork != null)
            {
                Cities = unitOfWork.Cities.GetAll();

            }
            return Cities;
        }

        private IEnumerable<State> RecoverStates()
        {
            IEnumerable<State> States = null;
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            if (unitOfWork != null)
            {
                States = unitOfWork.States.GetAll();
            }
            return States;
        }

        private IEnumerable<Sector> RecoverSectors()
        {
            IEnumerable<Sector> Sectors = null;
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            if (unitOfWork != null)
            {
                Sectors = unitOfWork.Sectors.GetAll();
            }
            return Sectors;
        }

        private void RegisterButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateLinkedOrganizationFromInputData();
            CreatePhoneFromInputData();
            bool validData = IsValidData() && AreValidPhoneNumbers();
            if (validData)
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
            if (TextBoxDirectUsers.Text != null && TextBoxDirectUsers.Text.Length > 0)
            {
                LinkedOrganization.DirectUsers = Int32.Parse(TextBoxDirectUsers.Text);
            }
            else
            {
                LinkedOrganization.DirectUsers = -1;
            }
            if (TextBoxIndirectUsers.Text != null && TextBoxIndirectUsers.Text.Length > 0)
            {
                LinkedOrganization.IndirectUsers = Int32.Parse(TextBoxIndirectUsers.Text);
            }
            else
            {
                LinkedOrganization.IndirectUsers = -1;
            }

            LinkedOrganization.PhoneNumber = TextBoxExtension.Text + TextBoxPhoneNumber.Text;

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

        private void CreatePhoneFromInputData()
        {
            PhoneOne.Extension = TextBoxExtension.Text;
            PhoneOne.PhoneNumber = TextBoxPhoneNumber.Text;
            PhoneTwo.Extension = TextBoxExtension2.Text;
            PhoneTwo.PhoneNumber = TextBoxPhoneNumber2.Text;
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

        public bool AreValidPhoneNumbers()
        {
            bool validPhoneNumbers;
            if (TextBoxExtension2.Text.Length <= 0 && TextBoxPhoneNumber2.Text.Length <= 0)
            {
                validPhoneNumbers = IsValidPhoneNumber(PhoneOne);
            }
            else
            {
                validPhoneNumbers = IsValidPhoneNumber(PhoneOne) && IsValidPhoneNumber(PhoneTwo);
            }
            return validPhoneNumbers;
        }

        private bool IsValidPhoneNumber(Phone phone)
        {
            PhoneValidator phoneValidator = new PhoneValidator();
            FluentValidation.Results.ValidationResult dataValidationResult = phoneValidator.Validate(phone);
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
            if (linkedOrganizationIsAlreadyRegistered)
            {
                return true;
            }
            
            return false;
        }

        private bool RegisterNewLinkedOrganization()
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            unitOfWork.LinkedOrganizations.Add(LinkedOrganization);
            PhoneOne.IdLinkedOrganization = LinkedOrganization.IdLinkedOrganization;
            unitOfWork.Phones.Add(PhoneOne);
            if (PhoneTwo.Extension.Length > 0 && PhoneTwo.PhoneNumber.Length > 0)
            {
                PhoneTwo.IdLinkedOrganization = LinkedOrganization.IdLinkedOrganization;
                unitOfWork.Phones.Add(PhoneTwo);
            }
            int rowsAffected = unitOfWork.Complete();
            unitOfWork.Dispose();

            return rowsAffected >= 2;
        }
    }

}