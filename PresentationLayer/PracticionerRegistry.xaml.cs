using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using FluentValidation.Results;
using PresentationLayer.Validators;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Windows;
using Utilities;
using PresentationLayer.Utils;


namespace PresentationLayer
{
	/// <summary>
	/// Lógica de interacción para PracticionerRegistry.xaml
	/// </summary>
	public partial class PracticionerRegistry : Window
	{
		public Practicioner Practicioner = new Practicioner
		{
			User = new User
			{
				Account = new Account()
			}
		};

		public PracticionerRegistry()
		{
			InitializeComponent();
			CreatePeriod();
			this.DataContext = Practicioner;
		}

		private void CancelButtonClicked(object sender, RoutedEventArgs e)
		{
			MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea cancelar?",
				"Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (messageBoxResult == MessageBoxResult.Yes)
			{
				this.Close();
			}

		}

		private void LogOutButtonClicked(object sender, RoutedEventArgs e)
		{
			MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea cerrar sesión?",
				"Cerrar Sesión", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (messageBoxResult == MessageBoxResult.Yes)
			{
				this.Close();
			}

		}

		private void RegisterButtonClicked(object sender, RoutedEventArgs e)
		{
			CreatePracticioner();
			if (IsValidData())
			{
				RegisterNewPracticioner();
			}
		}

		private void RegisterNewPracticioner()
		{
			ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
			UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
			try
			{
				if (!PracticionerIsAlreadyRegistered(unitOfWork))
				{
					HashAccountPassword();
					RegisterNewPracticioner(unitOfWork);
					MessageBox.Show("Practicante registrado exitosamente");
				}
				else
				{
					MessageBox.Show("Este practicante ya está registrado");
				}
			}
			catch (EntityException)
			{
				MessageBox.Show("No hay conexión con la base de datos. Intente más tarde");
			}
			finally
			{
				unitOfWork.Complete();
				unitOfWork.Dispose();
			}
		}

		private void CreatePracticioner()
		{
			Practicioner.User.UserType = UserType.Practicioner;
			Practicioner.User.Account.Password = PasswordBoxPassword.Password;
			Practicioner.User.Account.Username = TextBoxUsername.Text;
			GenderParser.ParseFromRadioButtonsToObject(ManRadioButton);
		}

		private bool IsValidData()
		{
			bool isValid = false;
			PracticionerValidator practicionerDataValidator = new PracticionerValidator();
			ValidationResult dataValidationResult = practicionerDataValidator.Validate(Practicioner);
			IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
			UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
			userFeedback.ShowFeedback();
			if (dataValidationResult.IsValid)
			{
				isValid = true;
			} 
			return isValid;
		}

		private bool PracticionerIsAlreadyRegistered(UnitOfWork unitOfWork)
		{
			bool practicionerIsAlreadyRegistered = unitOfWork.Practicioners.PracticionerIsAlreadyRegistered(Practicioner);
			bool userIsAlreadyRegistered = unitOfWork.Users.UserIsAlreadyRegistered(Practicioner.User);
			if (practicionerIsAlreadyRegistered || userIsAlreadyRegistered)
			{
				return true;
			}
			return false;
		}


		private void HashAccountPassword()
		{
			BCryptHashGenerator bCryptHashGenerator = new BCryptHashGenerator();
			string salt = bCryptHashGenerator.GenerateSalt();
			string hashedPassword = bCryptHashGenerator.GenerateHashedString(Practicioner.User.Account.Password, salt);
			Practicioner.User.Account.Password = hashedPassword;
		}

		private void RegisterNewPracticioner(UnitOfWork unitOfWork)
		{
			unitOfWork.Accounts.Add(Practicioner.User.Account);
			unitOfWork.Users.Add(Practicioner.User);
			unitOfWork.Practicioners.Add(Practicioner);
		}

		private void CreatePeriod()
		{
			String year = DateTime.Now.ToString("yyyy");
			ComboBoxPeriod.Items.Add("FEBRERO-JULIO " + year);
			ComboBoxPeriod.Items.Add("AGOSTO " + year + " -ENERO " + (Int32.Parse(year) + 1));
		}
	}
}
