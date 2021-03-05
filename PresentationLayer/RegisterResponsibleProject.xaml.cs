﻿using System.Windows;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using PresentationLayer.Validators;
using FluentValidation.Results;
using System.Collections.Generic;

namespace PresentationLayer
{
	/// <summary>
	/// Lógica de interacción para CoordinatorRegistry.xaml
	/// </summary>
	public partial class RegisterResponsableProject : Window
    {
		private ResponsibleProject responsibleProject= new ResponsibleProject();
		public RegisterResponsableProject()
        {
            InitializeComponent();
			this.DataContext = responsibleProject;
		}

		private void CancelButtonClicked(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("¿Seguro desea cancelar?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
		}

		private void RegisterButtonClicked(object sender, RoutedEventArgs e)
		{
			CreateResponsabileProjectFromInputData();
			bool isValidDataResponsibleProject = ValidateDataResponsibleProject();
			if (isValidDataResponsibleProject)
			{
				bool isValidEmail = ResponsibleProjectIsAlreadyRegistered();

				if (isValidEmail)
				{
					MessageBox.Show("Existe un responsable del proyecto con el mismo correo electrónico registrado", "Dato Repetido", MessageBoxButton.OK, MessageBoxImage.Warning);
				}
				else
				{
					bool isRegisterResponsableProject = RegisternewResponsibleProject();
					if (isRegisterResponsableProject)
					{
						MessageBox.Show("El responsable del proyecto se registró exitosamente", "Registro Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
						MessageBox.Show("El responsable del proyecto no pudo registrarse. Intente más tarde", "Registro Fallido", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
			}
			else
			{
				MessageBox.Show("Por favor, Ingrese datos correctos en los campos marcados en rojo", "Datos Incorrectos", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}
		
		private void CreateResponsabileProjectFromInputData()
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
			bool responsibleProjectIsAlreadyRegistered = unitOfWork.ResponsibleProjects.ResponsibleProjectIsAlreadyRegistered(responsibleProject.EmailAddress);
			unitOfWork.Dispose();
			if (responsibleProjectIsAlreadyRegistered)
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
