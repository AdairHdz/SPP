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
	/// Lógica de interacción para PractiiconerModification.xaml
	/// </summary>
	public partial class PracticionerModification : Window
	{

		private readonly ProfessionalPracticesContext _professionalPracticesContext;
        private readonly UnitOfWork _unitOfWork;
		public Practicioner Practicioner = new Practicioner
		{
			User = new User
			{
				Account = new Account()
			}
		};
		public PracticionerModification(string enrollment)
		{
			_professionalPracticesContext = new ProfessionalPracticesContext();
			_unitOfWork = new UnitOfWork(_professionalPracticesContext);
			InitializeComponent();
			ColocatePracticioner(enrollment);
			this.DataContext = Practicioner;
		}

		private void ColocatePracticioner(string enrollment)
		{
			try
			{
				Practicioner = _unitOfWork.Practicioners.GetAllInformationPracticioner(enrollment);

				RadioButtonMen.IsChecked = Practicioner.User.Gender == Gender.MALE;
				RadioButtonWomen.IsChecked = !RadioButtonMen.IsChecked;
				if (Practicioner.User.UserStatus == UserStatus.ACTIVE)
				{
					ComboBoxPracticionerStatus.SelectedIndex = 0;
				}
				else
				{
					ComboBoxPracticionerStatus.SelectedIndex = 1;
				}
				CreatePeriod(Practicioner.Term);
			}
			catch (SqlException)
			{
				MessageBox.Show("No hay conexión a la base de datos. Intente más tarde.");
				_unitOfWork.Dispose();
				CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
				coordinatorMenu.Show();
				this.Close();
			}
		}

		private void ModifyButtonClicked(object sender, RoutedEventArgs e)
		{
			CreatePracticionerFromInputData();
			if (ValidateData())
			{
				try
				{
					bool thereIsAnotherCoordinatorWithTheSameInformation =
					_unitOfWork.Practicioners.PracticionerIsAlreadyRegistered(Practicioner, true);
					if (thereIsAnotherCoordinatorWithTheSameInformation)
					{
						MessageBox.Show("Este practicante ya está registrado");
					}
					else
					{
						bool userConfirmedAction = AskForConfirmation();
						if (userConfirmedAction)
						{
							_unitOfWork.Complete();
							_unitOfWork.Dispose();
							MessageBox.Show("Practicante modificado exitosamente.");
							CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
							coordinatorMenu.Show();
							this.Close();
						}
					}
				}
				catch (SqlException)
				{
					MessageBox.Show("No hay conexión con la base de datos. Intente más tarde.");
					_unitOfWork.Dispose();
					this.Close();
				}
			}
		}

		private bool ValidateData()
		{
			bool isValid = false;
			PracticionerValidator practicionerDataValidator = new PracticionerValidator(true);
			
			ValidationResult dataValidationResult = practicionerDataValidator.Validate(Practicioner);
	
			IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
			UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
			userFeedback.ShowFeedback();

			foreach (ValidationFailure v in validationFailures)
			{
				Console.WriteLine(v);
			}

			if (dataValidationResult.IsValid)
			{
				isValid = true;
			}
			return isValid;
		}


		private void CreatePracticionerFromInputData()
		{
			if(RadioButtonMen.IsChecked == true)
			{
				Practicioner.User.Gender = Gender.MALE;
			}

			else if (RadioButtonWomen.IsChecked == true)
			{
				Practicioner.User.Gender = Gender.FEMALE;
			}

			int selectedTag = int.Parse(((System.Windows.Controls.ComboBoxItem)ComboBoxPracticionerStatus.SelectedItem).Tag.ToString());
			if (selectedTag == 0)
			{
				Practicioner.User.UserStatus = UserStatus.INACTIVE;
			}
			else
			{
				Practicioner.User.UserStatus = UserStatus.ACTIVE;
			}
		}

		private bool AskForConfirmation()
		{
			MessageBoxResult messageBoxResult =
				MessageBox.Show("¿Seguro desea modificar al practicante?", "Confirmación", MessageBoxButton.YesNo);
			return messageBoxResult == MessageBoxResult.Yes;
		}

		private void CancelButtonClicked(object sender, RoutedEventArgs e)
		{
			PracticionerConsult practicionerConsult = new PracticionerConsult();
			practicionerConsult.Show();
			this.Close();
		}

		private void CreatePeriod(String term)
		{
			String year = DateTime.Now.ToString("yyyy");
			ComboBoxPeriod.Items.Insert(0, term);
			ComboBoxPeriod.Items.Insert(1, "FEBRERO-JULIO " + year);
			ComboBoxPeriod.Items.Insert(2, "AGOSTO " + year + " -ENERO " + (Int32.Parse(year) + 1));
			ComboBoxPeriod.SelectedIndex = 1;
		}
	}
}
