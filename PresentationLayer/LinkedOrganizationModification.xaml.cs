using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using FluentValidation.Results;
using PresentationLayer.Validators;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para LinkedOrganizationModification.xaml
    /// </summary>
    public partial class LinkedOrganizationModification : Window
    {
        private List<State> _registeredStates;
        private List<Sector> _sectorsList;
        private UnitOfWork _unitOfWork;
        private ProfessionalPracticesContext _professionalPracticesContext;

        public LinkedOrganization LinkedOrganizationToBeModified = new LinkedOrganization
        {
            State = new State(),
            City = new City(),
            Sector = new Sector(),
            PhoneNumbers = new List<Phone>(),
        };

        public LinkedOrganizationModification(int linkedOrganizationId)
        {
            InitializeComponent();
            _professionalPracticesContext = new ProfessionalPracticesContext();
            _unitOfWork = new UnitOfWork(_professionalPracticesContext);
            LoadData(_unitOfWork, linkedOrganizationId);
            this.DataContext = LinkedOrganizationToBeModified;
        }

        private void LoadData(UnitOfWork unitOfWork, int linkedOrganizationId)
        {            
            try
            {
                LoadLinkedOrganizationData(unitOfWork, linkedOrganizationId);
                LoadStatesWithCities(unitOfWork);
                LoadSectors(unitOfWork);
            }
            catch (SqlException)
            {
                MessageBox.Show("No se pudo obtener la información de la base de datos");
                unitOfWork.Dispose();
                GoBackToCoordinatorMenu();
            }                                 
        }

        private void LoadStatesWithCities(UnitOfWork unitOfWork)
        {            
            _registeredStates = unitOfWork.States.GetStatesWithCities();

            foreach (State state in _registeredStates)
            {
                ComboBoxState.Items.Add(state);
            }

            ComboBoxState.SelectedItem = LinkedOrganizationToBeModified.State;

            State stateSelected = _registeredStates.Find(st => st.NameState.Equals(LinkedOrganizationToBeModified.State.NameState));

            foreach (City city in stateSelected.Cities)
            {
                ComboBoxCity.Items.Add(city);
            }

            ComboBoxCity.SelectedItem = LinkedOrganizationToBeModified.City;
        }

        private void LoadLinkedOrganizationData(UnitOfWork unitOfWork, int linkedOrganizationId)
        {
            LinkedOrganizationToBeModified = unitOfWork.LinkedOrganizations.Get(linkedOrganizationId);
        }

        private void LoadSectors(UnitOfWork unitOfWork)
        {            
            _sectorsList = unitOfWork.Sectors.GetAll().ToList();
            foreach (Sector sector in _sectorsList)
            {
                ComboBoxSector.Items.Add(sector);
            }

            ComboBoxSector.SelectedItem = LinkedOrganizationToBeModified.Sector;
        }

        private void BackButtonClicked(object sender, RoutedEventArgs e)
        {
            GoBackToCoordinatorMenu();
        }

        private void ComboBoxSectorSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void ComboBoxStateSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ComboBoxCity.Items.Clear();
            State stateSelected = (State)ComboBoxState.SelectedItem;
            List<City> citiesList = stateSelected.Cities;
            foreach (City city in citiesList)
            {
                ComboBoxCity.Items.Add(city);
            }
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea cancelar?",
                "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                GoBackToCoordinatorMenu();
            }
        }

        private void GoBackToCoordinatorMenu()
        {
            CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
            coordinatorMenu.Show();
            this.Close();
        }

        private void ModifyButtonClicked(object sender, RoutedEventArgs e)
        {
            LinkedOrganizationValidator linkedOrganizationValidator = new LinkedOrganizationValidator();
            FluentValidation.Results.ValidationResult dataValidationResult = linkedOrganizationValidator.Validate(LinkedOrganizationToBeModified);
            IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
            UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
            userFeedback.ShowFeedback();
            if (dataValidationResult.IsValid)
            {
                ModifyLinkedOrganization();
            }
        }

        private void ModifyLinkedOrganization()
        {
            try
            {
                if (ThereIsAnotherLinkedOrganizationWithSameData(_unitOfWork))
                {
                    MessageBox.Show("Existe una organización vinculada con el mismo nombre, correo o teléfono registrado");
                }
                else
                {
                    State selectedState = (State)ComboBoxState.SelectedItem;
                    City selectedCity = (City)ComboBoxCity.SelectedItem;
                    Sector selectedSector = (Sector)ComboBoxSector.SelectedItem;
                    LinkedOrganizationToBeModified.IdState = selectedState.IdState;
                    LinkedOrganizationToBeModified.IdCity = selectedCity.IdCity;
                    LinkedOrganizationToBeModified.IdSector = selectedSector.IdSector;

                    LinkedOrganizationToBeModified.State = selectedState;
                    LinkedOrganizationToBeModified.City = selectedCity;
                    LinkedOrganizationToBeModified.Sector = selectedSector;
                    _unitOfWork.Complete();
                    _unitOfWork.Dispose();
                    MessageBox.Show("La organización vinculada se modificó con éxito");
                    GoBackToCoordinatorMenu();
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("No se pudo obtener la información de la base de datos");
                GoBackToCoordinatorMenu();
            }
            
        }

        private bool ThereIsAnotherLinkedOrganizationWithSameData(UnitOfWork unitOfWork)
        {
            LinkedOrganization retrievedLinkedOrganization =
                unitOfWork.LinkedOrganizations.FindFirstOccurence(linkedOrganization =>
                (linkedOrganization.Name.Equals(LinkedOrganizationToBeModified.Name)
                || linkedOrganization.Email.Equals(LinkedOrganizationToBeModified.Email))
                && linkedOrganization.IdLinkedOrganization != LinkedOrganizationToBeModified.IdLinkedOrganization);

            Phone retrievedPhone = unitOfWork.Phones.FindFirstOccurence(phone => (phone.PhoneNumber.Equals(LinkedOrganizationToBeModified.PhoneNumbers[0].PhoneNumber)
            || phone.PhoneNumber.Equals(LinkedOrganizationToBeModified.PhoneNumbers[1].PhoneNumber))
            && phone.IdLinkedOrganization != LinkedOrganizationToBeModified.IdLinkedOrganization);

            if (retrievedLinkedOrganization != null)
            {
                return true;                
            }

            if(retrievedPhone != null)
            {
                if (retrievedPhone.IdPhoneNumber != LinkedOrganizationToBeModified.PhoneNumbers[0].IdPhoneNumber
                    || retrievedPhone.IdPhoneNumber != LinkedOrganizationToBeModified.PhoneNumbers[1].IdPhoneNumber)
                {
                    return true;
                }
            }

            return false;

            
        }
    }
}
