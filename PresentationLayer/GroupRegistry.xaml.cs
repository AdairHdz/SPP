using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using FluentValidation.Results;
using PresentationLayer.Validators;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Windows;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para GroupRegistry.xaml
    /// </summary>
    public partial class GroupRegistry : Window
    {
		private IList<Teacher> teachersAvailables = new List<Teacher>();
		private IList<Practicioner> practicionersAvailables = new List<Practicioner>();
		public IList<Practicioner> PracticionersSelected = new List<Practicioner>();
		public Group Group = new Group();
		public GroupRegistry()
        {
            InitializeComponent();
			LoadInformation();
			CreateTerm();
        }

		private void CancelButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea cancelar?",
				"Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (messageBoxResult == MessageBoxResult.Yes)
			{
				CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
				coordinatorMenu.Show();
				this.Close();
			}
		}

		private void RegistryButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			CreateGroup();
			if (ValidateInformation() && ValidPracticioners()){
				RegistryGroup();
            }
			else {
				MessageBox.Show("Por favor ingrese información correcta ");
			}
		}

		private void RegistryGroup()
        {
			ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
			UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
			try
			{
				if (!GroupIsAlreadyRegistered(unitOfWork))
				{
					AddGroup(unitOfWork);
					unitOfWork.Complete();
					unitOfWork.Dispose();
					AddGroupToPracticioners();
					MessageBox.Show("Grupo registrado exitosamente");
					CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
					coordinatorMenu.Show();
					this.Close();
				}
				else
				{
					MessageBox.Show("El NRC-Periodo del grupo ya está registrado. Por favor ingrese uno nuevo");
				}
			}
			catch (EntityException)
			{
				ShowExceptionDB();
			}
		}

		private void AddGroupToPracticioners()
        {
			ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
			UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
			try
            {
				unitOfWork.Practicioners.AddGroup(PracticionersSelected, unitOfWork.Groups.GroupId());
				unitOfWork.Complete();
				unitOfWork.Dispose();
			}
			catch (EntityException)
			{
				ShowExceptionDB();
			}
			
		}
		public void AddGroup(UnitOfWork unitOfWork)
        {
			unitOfWork.Groups.Add(Group);;
		}

		public bool GroupIsAlreadyRegistered(UnitOfWork unitOfWork)
        {
			bool groupIsAlreadyRegistered = unitOfWork.Groups.GroupIsAlreadyRegistered(Group);
			if (groupIsAlreadyRegistered)
			{
				return true;
			}
			return false;
		}

		public bool ValidPracticioners()
        {
			bool isValid = true;
			if (PracticionersSelected.Count < 5 && PracticionersSelected.Count > 15)
            {
				isValid = false;
            }
			return isValid;
        }

		public void CreateGroup()
        {
			Group.GroupStatus = GroupStatus.ACTIVE;
			Group.Nrc = TextBoxNRC.Text;
			Group.Term = ComboBoxPeriod.Text;
			Group.StaffNumber = (string)LabelStaffNumber.Content;
        }

		public bool ValidateInformation()
        {
			bool isValid = false;
			GroupValidator groupDataValidator = new GroupValidator();
			ValidationResult dataValidationResult = groupDataValidator.Validate(Group);
			IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
			UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
			userFeedback.ShowFeedback();
			if (dataValidationResult.IsValid)
			{
				isValid = true;
			}
			return isValid;
		}

		private void SelectionTeacher(object sender, RoutedEventArgs routedEventArgs)
		{
			Teacher teacher = ((Teacher)ListViewTeacher.SelectedItem);
			if (teacher != null)
			{
				LabelNameTeacher.Content = teacher.User.Name +" "+ teacher.User.LastName;
				LabelStaffNumber.Content = teacher.StaffNumber;
			}
		}

		private void SelectionPracticioner(object sender, RoutedEventArgs routedEventArgs)
		{
			Practicioner practicioner = ((Practicioner)ListViewPracticioners.SelectedItem);
			if (practicioner != null)
			{
				PracticionersSelected.Add(practicioner);
				practicionersAvailables.Remove(practicioner);
				LoadPracticionerAvailableList();
				LoadPracticionerSelectedList();
			}
		}

		private void DiscardPracticioner(object sender, RoutedEventArgs routedEventArgs)
		{
			Practicioner practicioner = ((Practicioner)ListViewPracticionersSelected.SelectedItem);
			if (practicioner != null)
			{
				practicionersAvailables.Add(practicioner);
				PracticionersSelected.Remove(practicioner);
				LoadPracticionerAvailableList();
				LoadPracticionerSelectedList();
			}
		}

		private void LoadPracticionerAvailableList()
		{
			ListViewPracticioners.Items.Clear();
			foreach (Practicioner practicioner in practicionersAvailables)
			{
				ListViewPracticioners.Items.Add(practicioner);
			}
		}

		private void LoadPracticionerSelectedList()
		{
			ListViewPracticionersSelected.Items.Clear();
			foreach (Practicioner practicioner in PracticionersSelected)
			{
				ListViewPracticionersSelected.Items.Add(practicioner);
			}
		}

		private void LoadInformation()
        {
			ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
			UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
			try
			{
				teachersAvailables = unitOfWork.Teachers.GetActiveTeachers();
				foreach (Teacher teacher in teachersAvailables)
				{
					ListViewTeacher.Items.Add(teacher);
				}
				practicionersAvailables = unitOfWork.Practicioners.PracticionersToGroup();
				LoadPracticionerAvailableList();
			}
			catch (EntityException)
			{
				ShowExceptionDB();
			}
			finally
			{
				unitOfWork.Dispose();
			}
		}

		private void CreateTerm()
		{
			string year = DateTime.Now.ToString("yyyy");
			ComboBoxPeriod.Items.Add("FEBRERO-JULIO " + year);
			ComboBoxPeriod.Items.Add("AGOSTO " + year + " -ENERO " + (Int32.Parse(year) + 1));
		}

		private void ShowExceptionDB()
        {
			MessageBox.Show("No hay conexión a la base de datos. Por favor intente más tarde");
			CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
			coordinatorMenu.Show();
			this.Close();
		}
	}
}