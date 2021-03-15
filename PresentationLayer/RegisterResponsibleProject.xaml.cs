using System.Windows;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using PresentationLayer.Validators;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Data.Entity.Core;


namespace PresentationLayer
{
	/// <summary>
	/// Lógica de interacción para CoordinatorRegistry.xaml
	/// </summary>
	public partial class RegisterResponsableProject : Window
    {
		private ResponsibleProject responsibleProject;
		public RegisterResponsableProject()
        {
            InitializeComponent();
			this.DataContext = responsibleProject;
		}

		private void CancelButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea cancelar?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (messageBoxResult == MessageBoxResult.Yes)
			{
				CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
				coordinatorMenu.Show();
				Close();
			}
		}

		private void BehindButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
			coordinatorMenu.Show();
			Close();
		}

		private void RegisterButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			responsibleProject = new ResponsibleProject();
			CreateResponsibleProjectFromInputData();
			if (ValidateDataResponsibleProject())
			{
				try
				{
					if (ResponsibleProjectIsAlreadyRegistered())
					{
						MessageBox.Show("Existe un responsable del proyecto con el mismo correo electrónico registrado", "Dato Repetido", MessageBoxButton.OK, MessageBoxImage.Warning);
					}
					else
					{
						if (RegisternewResponsibleProject())
						{
							MessageBox.Show("El responsable del proyecto se registró exitosamente", "Registro Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
						}
						else
						{
							MessageBox.Show("El responsable del proyecto no pudo registrarse. Intente más tarde", "Registro Fallido", MessageBoxButton.OK, MessageBoxImage.Error);
						}
					}
                }
                catch (EntityException)
				{
					MessageBox.Show("El responsable del proyecto no pudo registrarse. Intente más tarde", "Registro Fallido", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			else
			{
				MessageBox.Show("Por favor, Ingrese datos correctos en los campos marcados en rojo", "Datos Incorrectos", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}
		
		private void CreateResponsibleProjectFromInputData()
		{
			responsibleProject.Charge = TextBoxCharge.Text;
			responsibleProject.EmailAddress = TextBoxEmail.Text;
			responsibleProject.Name = TextBoxName.Text;
			responsibleProject.LastName = TextBoxLastName.Text;
			
		}

		private bool ResponsibleProjectIsAlreadyRegistered()
		{
			ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
			UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
			ResponsibleProject responsibleProjectWithSameEmailAddress = unitOfWork.ResponsibleProjects.FindFirstOccurence(ResponsibleProject => ResponsibleProject.EmailAddress == responsibleProject.EmailAddress);
			if (responsibleProjectWithSameEmailAddress != null)
			{
				return true;
			}
			return false;
		}

		private bool ValidateDataResponsibleProject()
		{
			ResponsibleProjectValidator responsibleProjectValidator = new ResponsibleProjectValidator();
			ValidationResult dataValidationResult = responsibleProjectValidator.Validate(responsibleProject);
			IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
			UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
			userFeedback.ShowFeedback();
			return dataValidationResult.IsValid;
		}

		private bool RegisternewResponsibleProject()
		{
			ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
			UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
			unitOfWork.ResponsibleProjects.Add(responsibleProject);
			int rowsAffected = unitOfWork.Complete();
			unitOfWork.Dispose();
			return rowsAffected == 1;
		}
	}
}
