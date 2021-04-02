using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using FluentValidation.Results;
using PresentationLayer.Validators;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para LinkedOrganizationRegistry.xaml
    /// </summary>
    public partial class LinkedOrganizationRegistry : Window
    {
        public LinkedOrganization LinkedOrganization = new LinkedOrganization
        {
            State = new State(),
            City = new City(),
            Sector = new Sector(),
            PhoneNumbers = new List<Phone>(),
        };

        private List<State> _statesList;
        private List<Sector> _sectorsList;
        private List<City> _citiesList;
        public LinkedOrganizationRegistry()
        {
            InitializeComponent();
            this.DataContext = LinkedOrganization;            
            LoadInitialDataForComboBoxes();
        }

        private void LoadInitialDataForComboBoxes()
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            try
            {
                LoadStates(unitOfWork);
                LoadSectors(unitOfWork);
            }
            catch (SqlException)
            {
                MessageBox.Show("No se pudo obtener la información de la base de datos");
                this.Close();
            }
            finally
            {
                unitOfWork.Complete();
                unitOfWork.Dispose();
            }
        }

        private void LoadStates(UnitOfWork unitOfWork)
        {
            _statesList = unitOfWork.States.GetStatesWithCities();
            foreach(State state in _statesList)
            {
                ComboBoxState.Items.Add(state);
            }
        }

        private void LoadSectors(UnitOfWork unitOfWork)
        {            
            _sectorsList = unitOfWork.Sectors.GetAll().ToList();
            foreach (Sector sector in _sectorsList)
            {
                ComboBoxSector.Items.Add(sector);
            }
        }

        private void ComboBoxStateSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxCity.Items.Clear();            
            State stateSelected = (State)ComboBoxState.SelectedItem;            
            _citiesList = stateSelected.Cities;             
            foreach (City city in _citiesList)
            {
                ComboBoxCity.Items.Add(city);
            }
            ComboBoxCity.SelectedIndex = 0;
        }

        private void BackButtonClicked(object sender, RoutedEventArgs e)
        {
            GoBackToCoordinatorMenu();
        }

        private void RegisterButtonClicked(object sender, RoutedEventArgs e)
        {

            CreateLinkedOrganizationFromUserInput();
            LinkedOrganizationValidator linkedOrganizationValidator = new LinkedOrganizationValidator();
            FluentValidation.Results.ValidationResult dataValidationResult = linkedOrganizationValidator.Validate(LinkedOrganization);
            IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
            UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
            userFeedback.ShowFeedback();
            if (dataValidationResult.IsValid)
            {
                ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                try
                {
                    if (ThereIsAnotherLinkedOrganizationWithSameData(unitOfWork))
                    {
                        MessageBox.Show("Existe una organización vinculada con el mismo nombre, correo o teléfono registrado");
                    }
                    else
                    {
                        unitOfWork.LinkedOrganizations.Add(LinkedOrganization);                        
                        MessageBox.Show("La organización vinculada se registró exitosamente");
                    }
                }
                catch (SqlException)
                {
                    MessageBox.Show("No se pudo obtener la información de la base de datos");
                    this.Close();
                }
                finally
                {
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
            }
        }
        
        private bool ThereIsAnotherLinkedOrganizationWithSameData(UnitOfWork unitOfWork)
        {
            LinkedOrganization retrievedLinkedOrganization =
                unitOfWork.LinkedOrganizations.FindFirstOccurence(linkedOrganization => 
                linkedOrganization.Name.Equals(LinkedOrganization.Name)
                || linkedOrganization.Email.Equals(LinkedOrganization.Email));

            Phone retrievedPhone = unitOfWork.Phones.FindFirstOccurence(phone => phone.PhoneNumber.Equals(LinkedOrganization.PhoneNumbers[0].PhoneNumber)
            || phone.PhoneNumber.Equals(LinkedOrganization.PhoneNumbers[1].PhoneNumber));

            if(retrievedLinkedOrganization != null || retrievedPhone != null)
            {
                return true;
            }

            return false;
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

        private void CreateLinkedOrganizationFromUserInput()
        {
            State selectedState = (State)ComboBoxState.SelectedItem;
            City selectedCity = (City)ComboBoxCity.SelectedItem;
            Sector selectedSector = (Sector)ComboBoxSector.SelectedItem;
            LinkedOrganization.IdState = selectedState.IdState;
            LinkedOrganization.IdCity = selectedCity.IdCity;
            LinkedOrganization.IdSector = selectedSector.IdSector;

            LinkedOrganization.State = null;
            LinkedOrganization.City = null;
            LinkedOrganization.Sector = null;

            Phone phone1 = new Phone()
            {
                Extension = TextBoxPhoneExtension1.Text,
                PhoneNumber = TextBoxPhoneNumber1.Text,
            };

            Phone phone2 = new Phone()
            {
                Extension = TextBoxPhoneExtension2.Text,
                PhoneNumber = TextBoxPhoneNumber2.Text,
            };

            LinkedOrganization.PhoneNumbers.Clear();
            LinkedOrganization.PhoneNumbers.Insert(0, phone1);
            LinkedOrganization.PhoneNumbers.Insert(1, phone2);
        }

        private void ComboBoxSectorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //string selectedSectorName = ComboBoxSector.SelectedItem.ToString();
            //Sector selectedSector = _sectorsList.Find(sector => sector.NameSector.Equals(selectedSectorName));
            //LinkedOrganization.Sector = selectedSector;
        }
    }
}
