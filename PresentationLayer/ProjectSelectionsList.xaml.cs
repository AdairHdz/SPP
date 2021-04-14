using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Utilities;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para ProjectSelectionsList.xaml
    /// </summary>
    public partial class ProjectSelectionsList : Window
    {
        private bool handle = true;
        private string textSearch;
        private string optionFilter;
        private bool isFilterWithText;
		private IList<Practicioner> _practicionersList = new List<Practicioner>();
		private IList<Project> _projects = new List<Project>();
		private Project _selectedProject;
		private Practicioner _selectedPracticioner;
		private Assignment _assignment;

		public ProjectSelectionsList()
        {
            InitializeComponent();			
			LoadSelectedProjects();
			AddRequestedProjectsInListView();
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

		private void BackButtonClicked(object sender, RoutedEventArgs e)
        {
			CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
			coordinatorMenu.Show();
			Close();
        }

		private void SearchButtonClicked(object sender, RoutedEventArgs routedEvent)
		{
			ListViewPracticioners.Items.Clear();
			textSearch = TextBoxSearch.Text;
			optionFilter = ((ComboBoxItem)ComboBoxFilter.SelectedItem).Content.ToString();
			if (ValidateIsTextSearch())
			{								
				ConsultPracticioner();
				AddPracticionersInListView();
				if (_practicionersList.Count() == 0)
				{
					MessageBox.Show("No se encontraron registros", "No hay registros", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
		}

		private void AddPracticionersInListView()
		{			
			foreach (Practicioner practioner in _practicionersList)
			{
				ListViewPracticioners.Items.Add(practioner);
			}
		}

		private Practicioner GetSelectedPracticioner()
        {
			Practicioner practicioner = ((Practicioner)ListViewPracticioners.SelectedItem);
			return practicioner;
		}

		private Project GetSelectedProject()
        {
			Project requestedProject = ((Project)ListViewProject.SelectedItem);			
			return requestedProject;
		}

		private void ConsultPracticioner()
		{
			IList<Practicioner> practicioner = new List<Practicioner>();
			_practicionersList = practicioner;
			ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
			UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
			string optionFilter = ((ComboBoxItem)ComboBoxFilter.SelectedItem).Content.ToString();
			try
			{
				IEnumerable<Assignment> assignments = unitOfWork.Assignments.GetAll();
				switch (optionFilter)
				{
					case "Todos":
						practicioner = unitOfWork.Practicioners.GetPracticionersWithUserData(Practicioner => true && !HasProjectAssigned(assignments, Practicioner.Enrollment));
						break;
					case "Activos":
						practicioner = unitOfWork.Practicioners.GetPracticionersWithUserData(Practicioner => Practicioner.User.UserStatus == UserStatus.ACTIVE && !HasProjectAssigned(assignments, Practicioner.Enrollment));
						break;
					case "Nombre":
						practicioner = unitOfWork.Practicioners.GetPracticionersWithUserData(Practicioner => Practicioner.User.Name.ToUpperInvariant().Contains(textSearch.ToUpperInvariant()) && !HasProjectAssigned(assignments, Practicioner.Enrollment));
						break;
					case "Apellido":
						practicioner = unitOfWork.Practicioners.GetPracticionersWithUserData(Practicioner => Practicioner.User.LastName.ToUpperInvariant().Contains(textSearch.ToUpperInvariant()) && !HasProjectAssigned(assignments, Practicioner.Enrollment));
						break;
					case "Correo":
						practicioner = unitOfWork.Practicioners.GetPracticionersWithUserData(Practicioner => Practicioner.User.Email.ToUpperInvariant().Contains(textSearch.ToUpperInvariant()) && !HasProjectAssigned(assignments, Practicioner.Enrollment));
						break;
					case "Periodo":
						practicioner = unitOfWork.Practicioners.GetPracticionersWithUserData(Practicioner => Practicioner.Term.ToUpperInvariant().Contains(textSearch.ToUpperInvariant()) && !HasProjectAssigned(assignments, Practicioner.Enrollment));
						break;
					case "Teléfono":
						practicioner = unitOfWork.Practicioners.GetPracticionersWithUserData(Practicioner => Practicioner.User.PhoneNumber.Equals(textSearch) && !HasProjectAssigned(assignments, Practicioner.Enrollment));
						break;
					case "Correo alterno":
						practicioner = unitOfWork.Practicioners.GetPracticionersWithUserData(Practicioner => Practicioner.User.AlternateEmail.ToUpperInvariant().Contains(textSearch.ToUpperInvariant()) && !HasProjectAssigned(assignments, Practicioner.Enrollment));
						break;
					case "Matrícula":
						practicioner = unitOfWork.Practicioners.GetPracticionersWithUserData(Practicioner => Practicioner.Enrollment.Equals(textSearch) && !HasProjectAssigned(assignments, Practicioner.Enrollment));
						break;
					case "Hombres":
						practicioner = unitOfWork.Practicioners.GetPracticionersWithUserData(Practicioner => Practicioner.User.Gender == Gender.MALE && !HasProjectAssigned(assignments, Practicioner.Enrollment));
						break;
					case "Mujeres":
						practicioner = unitOfWork.Practicioners.GetPracticionersWithUserData(Practicioner => Practicioner.User.Gender == Gender.FEMALE && !HasProjectAssigned(assignments, Practicioner.Enrollment));
						break;
				}
				if (practicioner.Count > 0)
				{
					_practicionersList = practicioner.ToList();
				}
			}
			catch (EntityException)
			{
				MessageBox.Show("Ha ocurrido un error en la conexión con la base de datos", "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
				GoBackToCoordinatorMenu();
            }
            catch (SqlException)
            {
				MessageBox.Show("Ha ocurrido un error en la conexión con la base de datos", "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
				GoBackToCoordinatorMenu();
			}
			finally
			{
				unitOfWork.Dispose();
			}		
        }

		private bool HasProjectAssigned(IEnumerable<Assignment> assignments, string enrollment)
        {
			foreach(Assignment assignment in assignments)
            {
                if (assignment.Enrollment.Equals(enrollment))
                {
					return true;
                }
            }
			return false;
        }

		private bool ValidateIsTextSearch()
		{
			if (String.IsNullOrWhiteSpace(textSearch) && isFilterWithText)
			{
				return false;
			}
			return true;
		}

		private void AssignProjectButtonClicked(object sender, RoutedEventArgs e)
        {
			_selectedPracticioner = GetSelectedPracticioner();
			_selectedProject = GetSelectedProject();
			if(ProjectAndPracticionerHaveBeenSelected())
            {
				SaveAssignmentOffice();
			}
            else
            {
				MessageBox.Show("Por favor seleccione a un practicante y a un proyecto para poder realizar la asignación");
			}
        }

		private bool ProjectCanStillReceivePracticioners()
        {
			return _selectedProject.QuantityPracticingAssing < _selectedProject.QuantityPracticing;
		}

		private bool ProjectAndPracticionerHaveBeenSelected()
        {
			return _selectedPracticioner != null && _selectedProject != null;

		}

		private void SaveAssignmentOffice()
        {
			ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
			UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);			
			try
            {
				string assignmentOfficeFilePath = FileExplorer.Show("Guardar oficio de asignación");
				string acceptanceOfficeFilePath = FileExplorer.Show("Guardar oficio de aceptación");
				Coordinator coordinator = unitOfWork.Coordinators.FindFirstOccurence(c => c.User.UserStatus == UserStatus.ACTIVE);
				SaveAssignmentToDatabase(unitOfWork, assignmentOfficeFilePath, acceptanceOfficeFilePath);

				AcceptanceOfficeTemplate acceptanceOfficeTemplate = new AcceptanceOfficeTemplate();
				acceptanceOfficeTemplate.MapData(_selectedProject, _selectedPracticioner, _assignment, coordinator);

				AssignmentOfficeTemplate assignmentOfficeTemplate = new AssignmentOfficeTemplate();
				assignmentOfficeTemplate.MapData(_selectedProject, _selectedPracticioner, _assignment, coordinator);

				DocumentGenerator documentGenerator = new DocumentGenerator();
				documentGenerator.CreateAcceptanceOfficeDocument($"{_assignment.RouteSave}", acceptanceOfficeTemplate);
				documentGenerator.CreateAssignmentOfficeTemplate($"{_assignment.OfficeOfAcceptance.RouteSave}", assignmentOfficeTemplate);
				
				MessageBox.Show("Proyecto asignado de forma exitosa");				
			}
            catch (EntityException)
            {
				MessageBox.Show("Ha ocurrido un error en la conexión con la base de datos", "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
			}
            catch (SqlException)
            {
				MessageBox.Show("Ha ocurrido un error en la conexión con la base de datos", "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
			}
            catch (NullReferenceException)
            {
				MessageBox.Show("Ocurrió un error al intentar generar el oficio de asignación; por favor, intente más tarde", "Guardado Fallido", MessageBoxButton.OK, MessageBoxImage.Error);
			}
            catch (COMException)
            {
				MessageBox.Show("Ocurrió un error al intentar generar el oficio de asignación, debido a que la ruta o el nombre del archivo que especificó no es válido; por favor, intente más tarde", "Guardado Fallido", MessageBoxButton.OK, MessageBoxImage.Error);				
			}
            finally
            {
				unitOfWork.Dispose();
				GoBackToCoordinatorMenu();
			}
			
		}

		private void SaveAssignmentToDatabase(UnitOfWork unitOfWork, string assignmentOfficeFilePath, string acceptanceOfficeFilePath)
        {			
			_selectedProject = unitOfWork.Projects.Get(_selectedProject.IdProject);
			_selectedPracticioner = unitOfWork.Practicioners.Get(_selectedPracticioner.Enrollment);

			_selectedProject.QuantityPracticingAssing += 1;
			if (_selectedProject.QuantityPracticingAssing == _selectedProject.QuantityPracticing)
			{
				_selectedProject.Status = ProjectStatus.FILLED;
			}

			List<RequestProject> requestedProjects = _selectedPracticioner.Requests;
			foreach(RequestProject requestedProject in requestedProjects)
            {
				if(requestedProject.IdProject == _selectedProject.IdProject)
                {
					requestedProject.RequestStatus = RequestStatus.APROVED;
                }
                else
                {
					requestedProject.RequestStatus = RequestStatus.DENIED;
				}
            }

			OfficeOfAcceptance officeOfAcceptance = new OfficeOfAcceptance
			{
				DateOfAcceptance = DateTime.Now,
				RouteSave = acceptanceOfficeFilePath
			};

			_assignment = new Assignment
			{
				CompletionTerm = "",
				DateAssignment = DateTime.Now,
				RouteSave = assignmentOfficeFilePath,
				StartTerm = _selectedProject.Term,
				Status = AssignmentStatus.Assigned,
				OfficeOfAcceptance = officeOfAcceptance,
                IdProject = _selectedProject.IdProject,
                Enrollment = _selectedPracticioner.Enrollment
            };

			unitOfWork.Assignments.Add(_assignment);

            unitOfWork.Complete();
        }

		private void LoadSelectedProjects()
        {			
			ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
			UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            try
            {
				_projects = unitOfWork.Projects.Find(project => project.QuantityPracticing > project.QuantityPracticingAssing).ToList();
			}
			catch (EntityException)
			{
				NotifyErrorAndGoBack();
			}
			catch (SqlException)
			{
				NotifyErrorAndGoBack();
			}
			finally
			{
                unitOfWork.Dispose();
            }			
		}

		private void NotifyErrorAndGoBack()
        {
			MessageBox.Show("Ha ocurrido un error en la conexión con la base de datos", "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
			GoBackToCoordinatorMenu();
		}

		private void GoBackToCoordinatorMenu()
        {
			CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
			coordinatorMenu.Show();
			Close();
        }
		
		private void AddRequestedProjectsInListView()
		{
			foreach (Project project in _projects)
			{
				ListViewProject.Items.Add(project);
			}
		}
    }
}
