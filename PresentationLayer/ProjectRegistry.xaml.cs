using System.Windows;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using PresentationLayer.Validators;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System;
using System.Windows.Media;

namespace PresentationLayer
{
	/// <summary>
	/// Lógica de interacción para RegisterProject.xaml
	/// </summary> 
	public partial class ProjectRegistry : Window
	{
		private Project project;
		private LinkedOrganization linkedOrganization;
		private ResponsibleProject responsibleProject;
		private List<SchedulingActivity> listSchedulingActivity;
		private bool isValidSchedulingActivity;
		public static string StaffNumber { get; set; }

		public ProjectRegistry()
		{
			InitializeComponent();
			this.DataContext = project;
			CreateTerm();
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

		private void ChooseLinkedOrganizationButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			LinkedOrganizationChoose chooseLinkedOrganization = new LinkedOrganizationChoose();
			chooseLinkedOrganization.ShowDialog();
			linkedOrganization = LinkedOrganizationChoose.ObteinLinkedOrganization();
			if (!object.ReferenceEquals(null, linkedOrganization))
			{
				TextBoxLinkedOrganization.Text = linkedOrganization.Name;
			}
		}

		private void ChooseResponsibleProjectButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			ResponsibleProjectChoose chooseResponsibleProject = new ResponsibleProjectChoose();
			chooseResponsibleProject.ShowDialog();
			responsibleProject = ResponsibleProjectChoose.ObteinResponsibleProject();
			if (!object.ReferenceEquals(null, responsibleProject))
			{
				TextBoxResponsibleProject.Text = responsibleProject.Name + " " + responsibleProject.LastName;
			}
		}

		private void RegisterButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			project = new Project();
			CreateProjectFromInputData();
			if (ValidateDataProject())
			{
				try
				{
					ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
					UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
					if (ProjectIsAlreadyRegistered(unitOfWork))
					{
						MessageBox.Show("Existe un proyecto con el mismo nombre registrado", "Dato Repetido", MessageBoxButton.OK, MessageBoxImage.Warning);
					}
					else
					{
						if (RegisternewProject(unitOfWork))
						{
							MessageBox.Show("El proyecto se registró exitosamente", "Registro Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
						}
						else
						{
							MessageBox.Show("El proyecto no pudo registrarse. Intente más tarde", "Registro Fallido", MessageBoxButton.OK, MessageBoxImage.Error);
						}
						CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
						coordinatorMenu.Show();
						Close();
					}
				}
				catch (EntityException)
				{
					MessageBox.Show("El proyecto no pudo registrarse. Intente más tarde", "Registro Fallido", MessageBoxButton.OK, MessageBoxImage.Error);
					CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
					coordinatorMenu.Show();
					Close();
				}
			}
			else
			{
				if (listSchedulingActivity.Count == 0)
				{
					TextBoxScheduleActivityOne.BorderBrush = Brushes.Red;
				}
				MessageBox.Show("Por favor, Ingrese datos correctos en los campos marcados en rojo", "Datos Incorrectos", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

		private void CreateTerm()
		{
			DateTime localDate = DateTime.Now;
			int month = localDate.Month;
			String year = localDate.Year.ToString();
			if (month > 1 && month <= 7)
			{
				LabelTerm.Content = "FEBRERO-JULIO " + year;
			}
			else
			{
				LabelTerm.Content = "AGOSTO-ENERO " + year;
				LabelMonth1.Content = "Agosto";
				LabelMonth2.Content = "Septiembre";
				LabelMonth3.Content = "Octubre";
				LabelMonth4.Content = "Noviembre";
			}
		}

		private void CreateProjectFromInputData()
		{
			project = new Project();
			project.NameProject = TextBoxName.Text;
			project.Description = TextBoxDescriptionGeneral.Text;
			project.ObjectiveGeneral = TextBoxObjectiveGeneral.Text;
			project.ObjectiveImmediate = TextBoxObjectiveImmediate.Text;
			project.ObjectiveMediate = TextBoxObjectiveMediate.Text;
			project.Methodology = TextBoxMethodology.Text;
			project.Resources = TextBoxResources.Text;
			project.Activities = TextBoxActivities.Text;
			project.Responsibilities = TextBoxResponsibilities.Text;
			project.Duration = 480;
			if (!string.IsNullOrWhiteSpace(TextBoxQuantityPracticing.Text))
			{
				project.QuantityPracticing = Int32.Parse(TextBoxQuantityPracticing.Text);
			}
			project.Term = LabelTerm.Content.ToString();
			project.StaffNumberCoordinator = StaffNumber;
			if (!object.ReferenceEquals(null, responsibleProject)) { 
				project.IdResponsibleProject = responsibleProject.IdResponsibleProject;
			}
			if (!object.ReferenceEquals(null, linkedOrganization)){
				project.IdLinkedOrganization = linkedOrganization.IdLinkedOrganization;
			}
		}

		private bool ProjectIsAlreadyRegistered(UnitOfWork unitOfWork)
		{
			Project projectWithSameName = unitOfWork.Projects.FindFirstOccurence(Project => Project.NameProject == project.NameProject);
			if (!object.ReferenceEquals(null, projectWithSameName))
			{
				return true;
			}
			return false;
		}

		private bool ValidateDataProject()
		{
			isValidSchedulingActivity = true;
			listSchedulingActivity = new List<SchedulingActivity>();
			ProjectValidator projectValidator = new ProjectValidator();
			ValidationResult dataValidationResult = projectValidator.Validate(project);
			IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
			UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
			userFeedback.ShowFeedback();
			ValidateScheduleActivity(TextBoxScheduleActivityOne, LabelMonth1.Content.ToString());
			ValidateScheduleActivity(TextBoxScheduleActivityTwo, LabelMonth2.Content.ToString());
			ValidateScheduleActivity(TextBoxScheduleActivityThree, LabelMonth3.Content.ToString());
			ValidateScheduleActivity(TextBoxScheduleActivityFour, LabelMonth4.Content.ToString());
			return dataValidationResult.IsValid && isValidSchedulingActivity && listSchedulingActivity.Count!=0;
		}

		private void ValidateScheduleActivity(System.Windows.Controls.TextBox TextBoxActivity,string month)
        {
			TextBoxActivity.BorderBrush = Brushes.Gray;
			if (!string.IsNullOrWhiteSpace(TextBoxActivity.Text))
			{
				SchedulingActivity schedulingActivity = new SchedulingActivity();
				schedulingActivity.Month = month;
				schedulingActivity.Activity = TextBoxActivity.Text;
				SchedulingActivityValidator schedulingActivityValidator = new SchedulingActivityValidator(TextBoxActivity.Name);
				ValidationResult dataValidationResult = schedulingActivityValidator.Validate(schedulingActivity);
                if (!dataValidationResult.IsValid)
                {
					isValidSchedulingActivity = false;
					TextBoxActivity.BorderBrush = Brushes.Red;
				}
                else
                {
					TextBoxActivity.BorderBrush = Brushes.Green;
				}
				listSchedulingActivity.Add(schedulingActivity);
			}        
		}

		private bool RegisternewProject(UnitOfWork unitOfWork)
		{			
			unitOfWork.Projects.Add(project);
			int rowsAffected = unitOfWork.Complete();
			return RegisterSchedulingActivity(unitOfWork) && rowsAffected ==1;
		}

		private void AddIdProjectInSchedulingActivity()
        {
			for (int index = 0; index < listSchedulingActivity.Count; index++)
			{
				listSchedulingActivity[index].IdProject = project.IdProject;
            }
        }

		private bool RegisterSchedulingActivity(UnitOfWork unitOfWork)
        {
			Project projectRegistry = unitOfWork.Projects.FindFirstOccurence(Project => Project.NameProject == project.NameProject);
			if (!object.ReferenceEquals(null, projectRegistry))
			{
				project.IdProject = projectRegistry.IdProject;
				AddIdProjectInSchedulingActivity();
				unitOfWork.SchedulingActivities.AddRange(listSchedulingActivity);
				int rowsAffected = unitOfWork.Complete();
				unitOfWork.Dispose();
				return rowsAffected.Equals(listSchedulingActivity.Count);
			}
			return false; 
		}
	}
}
