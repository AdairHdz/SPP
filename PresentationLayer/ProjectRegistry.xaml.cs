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
		private Project _project;
		private LinkedOrganization _linkedOrganization;
		private ResponsibleProject _responsibleProject;
		private List<SchedulingActivity> _listSchedulingActivity;
		private bool _isValidSchedulingActivity;
		public static string StaffNumber { get; set; }

		public ProjectRegistry()
		{
			InitializeComponent();
			this.DataContext = _project;
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
			_linkedOrganization = LinkedOrganizationChoose.ObteinLinkedOrganization();
			if (!object.ReferenceEquals(null, _linkedOrganization))
			{
				TextBoxLinkedOrganization.Text = _linkedOrganization.Name;
			}
		}

		private void ChooseResponsibleProjectButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			ResponsibleProjectChoose chooseResponsibleProject = new ResponsibleProjectChoose();
			chooseResponsibleProject.ShowDialog();
			_responsibleProject = ResponsibleProjectChoose.ObteinResponsibleProject();
			if (!object.ReferenceEquals(null, _responsibleProject))
			{
				TextBoxResponsibleProject.Text = _responsibleProject.Name + " " + _responsibleProject.LastName;
			}
		}

		private void RegisterButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			_project = new Project();
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
				if (_listSchedulingActivity.Count == 0)
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
			_project = new Project();
			_project.NameProject = TextBoxName.Text;
			_project.Description = TextBoxDescriptionGeneral.Text;
			_project.ObjectiveGeneral = TextBoxObjectiveGeneral.Text;
			_project.ObjectiveImmediate = TextBoxObjectiveImmediate.Text;
			_project.ObjectiveMediate = TextBoxObjectiveMediate.Text;
			_project.Methodology = TextBoxMethodology.Text;
			_project.Resources = TextBoxResources.Text;
			_project.Activities = TextBoxActivities.Text;
			_project.Responsibilities = TextBoxResponsibilities.Text;
			_project.Duration = 480;
			if (!string.IsNullOrWhiteSpace(TextBoxQuantityPracticing.Text))
			{
				_project.QuantityPracticing = Int32.Parse(TextBoxQuantityPracticing.Text);
			}
			_project.Term = LabelTerm.Content.ToString();
			_project.StaffNumberCoordinator = StaffNumber;
			_project.DaysHours = "A acordar con el estudiante (en horario de oficina)";
			if (!object.ReferenceEquals(null, _responsibleProject)) { 
				_project.IdResponsibleProject = _responsibleProject.IdResponsibleProject;
			}
			if (!object.ReferenceEquals(null, _linkedOrganization)){
				_project.IdLinkedOrganization = _linkedOrganization.IdLinkedOrganization;
			}
		}

		private bool ProjectIsAlreadyRegistered(UnitOfWork unitOfWork)
		{
			Project projectWithSameName = unitOfWork.Projects.FindFirstOccurence(Project => Project.NameProject == _project.NameProject);
			if (!object.ReferenceEquals(null, projectWithSameName))
			{
				return true;
			}
			return false;
		}

		private bool ValidateDataProject()
		{
			_isValidSchedulingActivity = true;
			_listSchedulingActivity = new List<SchedulingActivity>();
			ProjectValidator projectValidator = new ProjectValidator();
			ValidationResult dataValidationResult = projectValidator.Validate(_project);
			IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
			UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
			userFeedback.ShowFeedback();
			ValidateScheduleActivity(TextBoxScheduleActivityOne, LabelMonth1.Content.ToString());
			ValidateScheduleActivity(TextBoxScheduleActivityTwo, LabelMonth2.Content.ToString());
			ValidateScheduleActivity(TextBoxScheduleActivityThree, LabelMonth3.Content.ToString());
			ValidateScheduleActivity(TextBoxScheduleActivityFour, LabelMonth4.Content.ToString());
			return dataValidationResult.IsValid && _isValidSchedulingActivity && _listSchedulingActivity.Count!=0;
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
					_isValidSchedulingActivity = false;
					TextBoxActivity.BorderBrush = Brushes.Red;
				}
                else
                {
					TextBoxActivity.BorderBrush = Brushes.Green;
				}
				_listSchedulingActivity.Add(schedulingActivity);
			}        
		}

		private bool RegisternewProject(UnitOfWork unitOfWork)
		{			
			unitOfWork.Projects.Add(_project);
			int rowsAffected = unitOfWork.Complete();
			return RegisterSchedulingActivity(unitOfWork) && rowsAffected ==1;
		}

		private void AddIdProjectInSchedulingActivity()
        {
			for (int index = 0; index < _listSchedulingActivity.Count; index++)
			{
				_listSchedulingActivity[index].IdProject = _project.IdProject;
            }
        }

		private bool RegisterSchedulingActivity(UnitOfWork unitOfWork)
        {
			Project projectRegistry = unitOfWork.Projects.FindFirstOccurence(Project => Project.NameProject == _project.NameProject);
			if (!object.ReferenceEquals(null, projectRegistry))
			{
				_project.IdProject = projectRegistry.IdProject;
				AddIdProjectInSchedulingActivity();
				unitOfWork.SchedulingActivities.AddRange(_listSchedulingActivity);
				int rowsAffected = unitOfWork.Complete();
				unitOfWork.Dispose();
				return rowsAffected.Equals(_listSchedulingActivity.Count);
			}
			return false; 
		}
	}
}
