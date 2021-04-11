using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using FluentValidation.Results;
using PresentationLayer.Validators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace PresentationLayer
{
	/// <summary>
	/// Lógica de interacción para TeacherModification.xaml
	/// </summary>
	public partial class TeacherModification : Window
	{

		private readonly ProfessionalPracticesContext _professionalPracticesContext;
		private readonly UnitOfWork _unitOfWork;
		private Teacher _teacher = new Teacher
		{
			User = new User
			{
				Account = new Account()
			}
		};
		public TeacherModification(string staffNumber)
		{
			_professionalPracticesContext = new ProfessionalPracticesContext();
			_unitOfWork = new UnitOfWork(_professionalPracticesContext);
			InitializeComponent();
			ColocateTeacher(staffNumber);
			this.DataContext = _teacher;
		}

		private void ColocateTeacher(string staffNumber)
		{
			try
			{
				_teacher = _unitOfWork.Teachers.GetTeacherWithAllInformation(staffNumber);
				RadioButtonMen.IsChecked = _teacher.User.Gender == Gender.MALE;
				RadioButtonWomen.IsChecked = !RadioButtonMen.IsChecked;
				if (_teacher.User.UserStatus == UserStatus.ACTIVE)
				{
					ComboBoxPracticionerStatus.SelectedIndex = 0;
				}
				else
				{
					ComboBoxPracticionerStatus.SelectedIndex = 1;
				}
			}
			catch (SqlException)
			{
				_unitOfWork.Dispose();
				ShowException();
			}
		}

		private void ModifyButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			CreateTeacherFromInputData();
			Console.WriteLine(ValidateData());
			if (ValidateData())
			{
				try
				{
					bool teacherrWithSameData = _unitOfWork.Teachers.TeacherIsAlreadyRegistered(_teacher, true);
					if (teacherrWithSameData)
					{
						MessageBox.Show("El correo, correo alternativo o número de teléfono ingresado ya está ocupado por otro usuario");
					}
					else
					{
						bool userConfirmedAction = AskForConfirmation();
						if (userConfirmedAction)
						{
							_unitOfWork.Complete();
							_unitOfWork.Dispose();
							MessageBox.Show("Profesor modificado exitosamente.");
							TeacherConsultation teacherConsult = new TeacherConsultation();
							teacherConsult.Show();
							Close();
						}
					}
				}
				catch (SqlException)
				{
					ShowException();
				}
			}
		}

		private void ShowException()
		{
			MessageBox.Show("No hay conexión con la base de datos. Intente más tarde.");
			ShowMenu();
		}

		private bool ValidateData()
		{
			bool isValid = false;
			TeacherValidator teacherDataValidator = new TeacherValidator(true);
			ValidationResult dataValidationResult = teacherDataValidator.Validate(_teacher);
			IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
			foreach (ValidationFailure v in validationFailures)
            {
				Console.WriteLine(v);
            }
			UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
			userFeedback.ShowFeedback();
			if (dataValidationResult.IsValid)
			{
				isValid = true;
			}
			return isValid;
		}

		private void CreateTeacherFromInputData()
		{
			if (RadioButtonMen.IsChecked == true)
			{
				_teacher.User.Gender = Gender.MALE;
			}
			else if (RadioButtonWomen.IsChecked == true)
			{
				_teacher.User.Gender = Gender.FEMALE;
			}

			int selectedTag = int.Parse(((System.Windows.Controls.ComboBoxItem)ComboBoxPracticionerStatus.SelectedItem).Tag.ToString());
			if (selectedTag == 0)
			{
				_teacher.User.UserStatus = UserStatus.INACTIVE;
				_teacher.DischargeDate = DateTime.Now;
			}
			else
			{
				_teacher.User.UserStatus = UserStatus.ACTIVE;
				_teacher.DischargeDate = null;
			}
		}

		private bool AskForConfirmation()
		{
			MessageBoxResult messageBoxResult =
				MessageBox.Show("¿Seguro desea modificar al profesor?", "Confirmación", MessageBoxButton.YesNo);
			return messageBoxResult == MessageBoxResult.Yes;
		}

		private void CancelButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			TeacherConsultation teacherConsult = new TeacherConsultation();
			teacherConsult.Show();
			Close();
		}

		private void ShowMenu()
        {
			ManagerMenu managerMenu = new ManagerMenu();
			managerMenu.Show();
			this.Close();
		}
	}
}
