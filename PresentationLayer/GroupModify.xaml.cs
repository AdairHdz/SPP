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
	/// Lógica de interacción para GroupModify.xaml
	/// </summary>
	public partial class GroupModify : Window
	{
		private IList<Teacher> _teachersAvailables = new List<Teacher>();
		private IList<Practicioner> _practicionersInThisGroup = new List<Practicioner>();
		private readonly ProfessionalPracticesContext _professionalPracticesContext;
		private readonly UnitOfWork _unitOfWork;
		public Group Group = new Group();
		public IList<Practicioner> _practicionersSelectedToAdd = new List<Practicioner>();
		public IList<Practicioner> PracticionerRemove = new List<Practicioner>();
		public IList<Practicioner> PracticionersInThisGroupOriginal = new List<Practicioner>();

		public GroupModify(int idGroup)
		{
			_professionalPracticesContext = new ProfessionalPracticesContext();
			_unitOfWork = new UnitOfWork(_professionalPracticesContext);
			InitializeComponent();
			LoadInformation(idGroup);
			this.DataContext = Group;
			CreateTerm();
			CreateGroupStatus();
		}

		private void CancelButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			GroupModifyList groupModifyList = new GroupModifyList();
			groupModifyList.Show();
			this.Close();
		}

		private void ModifyButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			CreateGroup();
			if (ValidateInformation() && ValidPracticioners())
			{
				ModifyGroup();
			}
			else
			{
				MessageBox.Show("Por favor ingrese información correcta ");
			}
		}

		private void ModifyGroup()
		{
			try
			{
				if (_unitOfWork.Groups.GroupCanBeModify(Group))
				{
					ChangeGroupToPracticioners();
					MessageBox.Show("Grupo modificado exitosamente");
					CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
					coordinatorMenu.Show();
					this.Close();
				}
				else
				{
					MessageBox.Show("El NRC-Periodo del grupo ya está registrado. Por favor ingrese uno nuevo"+ Group.IdGroup);
				}
			}
			catch (EntityException)
			{
				ShowExceptionDB();
			}
			finally
			{
				_unitOfWork.Complete();
				_unitOfWork.Dispose();
			}
		}

		private void ChangeGroupToPracticioners()
		{
			IList<Practicioner> practicionersAdd = new List<Practicioner>();
			foreach (Practicioner practicioner in ListViewPracticioners.Items)
            {
				practicionersAdd.Add(practicioner);
            }
			_unitOfWork.Practicioners.RemoveGroup(PracticionerRemove);  
			_unitOfWork.Practicioners.AddGroup(practicionersAdd, Group.IdGroup);
		}

		public bool ValidPracticioners()
		{
			bool isValid = true;
			if (ListViewPracticioners.Items.Count > 15)
			{
				isValid = false;
			}
			return isValid;
		}

		public void CreateGroup()
		{
			switch (ComboBoxStatus.Text)
			{
				case "Activo":
					Group.GroupStatus = GroupStatus.ACTIVE;
					break;
				case "Cerrado":
					Group.GroupStatus = GroupStatus.CLOSED;
						break;
				case "Concluido":
					Group.GroupStatus = GroupStatus.CONCLUDED;
					break;
			}
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
				LabelNameTeacher.Content = teacher.User.Name + " " + teacher.User.LastName;
				LabelStaffNumber.Content = teacher.StaffNumber;
			}
		}

		private void SelectionPracticioner(object sender, RoutedEventArgs routedEventArgs)
		{
			Practicioner practicioner = ((Practicioner)ListViewPracticionersToAdd.SelectedItem);
			if (practicioner != null)
			{
				_practicionersInThisGroup.Add(practicioner);
				_practicionersSelectedToAdd.Remove(practicioner);
				LoadPracticionerInThisGroup();
				LoadPracticionerSelectedList();
			}
		}

		private void DiscardPracticioner(object sender, RoutedEventArgs routedEventArgs)
		{
			Practicioner practicioner = ((Practicioner)ListViewPracticioners.SelectedItem);
			if (practicioner != null)
			{
				foreach (Practicioner practicionerAdd in PracticionersInThisGroupOriginal)
				{
					if (practicioner.Enrollment == practicionerAdd.Enrollment)
					{
						PracticionerRemove.Add(practicioner);

					}
				}
				_practicionersSelectedToAdd.Add(practicioner);
				_practicionersInThisGroup.Remove(practicioner);
				LoadPracticionerInThisGroup();
				LoadPracticionerSelectedList();
			}
		}

		private void LoadPracticionerInThisGroup()
		{
			ListViewPracticioners.Items.Clear();
			foreach (Practicioner practicioner in _practicionersInThisGroup)
			{
				ListViewPracticioners.Items.Add(practicioner);
			}
		}

		private void LoadPracticionerSelectedList()
		{
			ListViewPracticionersToAdd.Items.Clear();
			foreach (Practicioner practicioner in _practicionersSelectedToAdd)
			{
				ListViewPracticionersToAdd.Items.Add(practicioner);
			}
		}

		private void LoadInformation(int idGroup)
		{
			try
			{
				_teachersAvailables = _unitOfWork.Teachers.GetActiveTeachers();
				foreach (Teacher teacherAvailable in _teachersAvailables)
				{
					ListViewTeacher.Items.Add(teacherAvailable);
				}
				Group = _unitOfWork.Groups.Get(idGroup);
				Teacher teacher = _unitOfWork.Groups.GetTeacherAssigned(Group.StaffNumber);
				LabelNameTeacher.Content = teacher.User.Name + " " + teacher.User.LastName;
				IList<Practicioner> practicionersGroup = _unitOfWork.Practicioners.GetPracticionersInThisGroup(idGroup);
				_practicionersInThisGroup = practicionersGroup;
				PracticionersInThisGroupOriginal = practicionersGroup;
				IList<Practicioner> practicionersAdd = _unitOfWork.Practicioners.GetPracticionerActiveNotInThisGroup(idGroup);
				_practicionersSelectedToAdd = practicionersAdd;

				LoadPracticionerInThisGroup();
				LoadPracticionerSelectedList();
			}
			catch (EntityException)
			{
				ShowExceptionDB();
				_unitOfWork.Dispose();
			}
		}

		private void CreateTerm()
		{
			string year = DateTime.Now.ToString("yyyy");
			ComboBoxPeriod.Items.Insert(0, Group.Term);
			ComboBoxPeriod.Items.Insert(1, "FEBRERO-JULIO " + year);
			ComboBoxPeriod.Items.Insert(2, "AGOSTO " + year + " -ENERO " + (Int32.Parse(year) + 1));
			ComboBoxPeriod.SelectedIndex = 0;
		}

		private void CreateGroupStatus()
		{
			switch (Group.GroupStatus)
			{
				case GroupStatus.ACTIVE:
					ComboBoxStatus.Items.Insert(0, "Activo");
					ComboBoxStatus.Items.Insert(1, "Concluido");
					ComboBoxStatus.Items.Insert(2, "Cerrado");
					break;
				case GroupStatus.CONCLUDED:
					ComboBoxStatus.Items.Insert(0, "Concluido");
					ComboBoxStatus.Items.Insert(1, "Activo");
					ComboBoxStatus.Items.Insert(2, "Cerrado");
					break;
				case GroupStatus.CLOSED:
					ComboBoxStatus.Items.Insert(0, "Cerrado");
					ComboBoxStatus.Items.Insert(1, "Concluido");
					ComboBoxStatus.Items.Insert(2, "Activo");
					break;
			}
			ComboBoxStatus.SelectedIndex = 0;
		}

		private void ShowExceptionDB()
		{
			MessageBox.Show("No hay conexión a la base de datos. Intente más tarde");
			CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
			coordinatorMenu.Show();
			this.Close();
		}
	}
}