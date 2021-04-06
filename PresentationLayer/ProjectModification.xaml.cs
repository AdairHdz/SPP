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
    /// Lógica de interacción para ProjectModification.xaml
    /// </summary>
    public partial class ProjectModification : Window
    {
        private Project _project;
        private LinkedOrganization _linkedOrganization;
        private ResponsibleProject _responsibleProject;
        private IEnumerable<SchedulingActivity> _listSchedulingActivity;

        public ProjectModification()
        {
            InitializeComponent();
        }

        public bool InitializeProject(int idProject) 
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            _project = unitOfWork.Projects.FindFirstOccurence(Project => Project.IdProject == idProject);
            if (_project!=null)
            {
                ObteinProject();
                AddSchedulingActivitiesInListView(unitOfWork);
                return true;
            }
            else
            {                
                return false;
            }
        }

        private void AddSchedulingActivitiesInListView(UnitOfWork unitOfWork)
        {
            _listSchedulingActivity = unitOfWork.SchedulingActivities.Find(schedulingActivity=> schedulingActivity.IdProject == _project.IdProject);
            foreach (SchedulingActivity schedulingActivity in _listSchedulingActivity)
            {
                ListViewSchedulingActivities.Items.Add(schedulingActivity);
            }
        }

        private void ObteinProject()
        {
            TextBoxName.Text = _project.NameProject;
            TextBoxDescriptionGeneral.Text = _project.Description;
            TextBoxObjectiveGeneral.Text = _project.ObjectiveGeneral;
            TextBoxObjectiveImmediate.Text = _project.ObjectiveImmediate;
            TextBoxObjectiveMediate.Text = _project.ObjectiveMediate;
            TextBoxMethodology.Text = _project.Methodology;
            TextBoxResources.Text = _project.Resources;
            TextBoxActivities.Text = _project.Activities;
            TextBoxResponsibilities.Text = _project.Responsibilities;
            TextBoxQuantityPracticing.Text = _project.QuantityPracticing.ToString();
            LabelTerm.Content = _project.Term;
            TextBoxDaysHours.Text = _project.DaysHours;
            TextBoxLinkedOrganization.Text = _project.LinkedOrganization.Name;
            _responsibleProject = new ResponsibleProject();
            _responsibleProject.IdResponsibleProject = _project.IdResponsibleProject;
            _linkedOrganization = new LinkedOrganization();
            _linkedOrganization.IdLinkedOrganization = _project.IdLinkedOrganization;
            TextBoxResponsibleProject.Text = _project.ResponsibleProject.Name + " " + _project.ResponsibleProject.LastName;
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea cancelar?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                ProjectConsultation projectConsultation = new ProjectConsultation();
                projectConsultation.Show();
                Close();
            }
        }

        private void BehindButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            ProjectConsultation projectConsultation = new ProjectConsultation();
            projectConsultation.Show();
            Close();
        }

        private void ChooseLinkedOrganizationButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            LinkedOrganizationChoose chooseLinkedOrganization = new LinkedOrganizationChoose();
            chooseLinkedOrganization.ShowDialog();
            LinkedOrganization linkedOrganizationReceived = LinkedOrganizationChoose.ObteinLinkedOrganization();
            if (linkedOrganizationReceived!=null)
            {
                TextBoxLinkedOrganization.Text = linkedOrganizationReceived.Name;
                _linkedOrganization.IdLinkedOrganization = linkedOrganizationReceived.IdLinkedOrganization;
            }
        }

        private void ChooseResponsibleProjectButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            ResponsibleProjectChoose chooseResponsibleProject = new ResponsibleProjectChoose();
            chooseResponsibleProject.ShowDialog();
            ResponsibleProject responsibleProjectReceived = ResponsibleProjectChoose.ObteinResponsibleProject();
            if (responsibleProjectReceived!=null)
            {
                TextBoxResponsibleProject.Text = responsibleProjectReceived.Name + " " + responsibleProjectReceived.LastName;
                _responsibleProject.IdResponsibleProject = responsibleProjectReceived.IdResponsibleProject;
            }
        }

        private void CreateProjectFromInputData()
        {
            _project.NameProject = TextBoxName.Text;
            _project.Description = TextBoxDescriptionGeneral.Text;
            _project.ObjectiveGeneral = TextBoxObjectiveGeneral.Text;
            _project.ObjectiveImmediate = TextBoxObjectiveImmediate.Text;
            _project.ObjectiveMediate = TextBoxObjectiveMediate.Text;
            _project.Methodology = TextBoxMethodology.Text;
            _project.Resources = TextBoxResources.Text;
            _project.Activities = TextBoxActivities.Text;
            _project.Responsibilities = TextBoxResponsibilities.Text;
            _project.DaysHours = TextBoxDaysHours.Text;
            if (!string.IsNullOrWhiteSpace(TextBoxQuantityPracticing.Text))
            {
                _project.QuantityPracticing = Int32.Parse(TextBoxQuantityPracticing.Text);
            }
            else
            {
                TextBoxQuantityPracticing.Text = _project.QuantityPracticing.ToString();
            }
            _project.IdResponsibleProject = _responsibleProject.IdResponsibleProject;
            _project.IdLinkedOrganization = _linkedOrganization.IdLinkedOrganization;
        }

        private bool ProjectIsAlreadyRegistered(UnitOfWork unitOfWork)
        {
            Project projectWithSameName = unitOfWork.Projects.FindFirstOccurence(Project => Project.NameProject == _project.NameProject && Project.IdProject != _project.IdProject);
            if (projectWithSameName!=null)
            {
                return true;
            }
            return false;
        }

        private void SchedulingActivityListViewSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs selectionChanged)
        {
            SchedulingActivity schedulingActivity = ((SchedulingActivity)ListViewSchedulingActivities.SelectedItem);
            if (schedulingActivity!=null)
            {
                ButtonDeleteActivity.IsEnabled = true;
                ButtonModifyActivity.IsEnabled = true;
            }
        }

        private void ModifyProjectButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            CreateProjectFromInputData();
            if (ValidateDataResponsibleProject())
            {
                if (ValidateQuantityPracticing())
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
                            if (UpdatenewProject(unitOfWork))
                            {
                                MessageBox.Show("El proyecto se modificó exitosamente", "Modificación Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                                ProjectConsultation projectConsultation = new ProjectConsultation();
                                projectConsultation.Show();
                                Close();
                            }
                        }
                    }
                    catch (EntityException)
                    {
                        MessageBox.Show("El proyecto no pudo modificarse. Intente más tarde", "Modificación Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
                        ProjectConsultation projectConsultation = new ProjectConsultation();
                        projectConsultation.Show();
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("El número de practicantes no puede ser menor al número de practicantes asignados. Por favor, Ingrese uno correcto", "Datos Incorrectos", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Por favor, Ingrese datos correctos en los campos marcados en rojo", "Datos Incorrectos", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool ValidateDataResponsibleProject()
        {
            ProjectValidator projectValidator = new ProjectValidator();
            ValidationResult dataValidationResult = projectValidator.Validate(_project);
            IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
            UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
            userFeedback.ShowFeedback();
            return dataValidationResult.IsValid;
        }

        private bool ValidateQuantityPracticing()
        {
            if(_project.QuantityPracticingAssing> _project.QuantityPracticing)
            {
                TextBoxQuantityPracticing.BorderBrush = Brushes.Red;
                return false;
            }
            return true;
        }

        private bool UpdatenewProject(UnitOfWork unitOfWork)
        {
            Project updateProject = unitOfWork.Projects.Get(_project.IdProject);
            updateProject.NameProject = _project.NameProject;
            updateProject.Description = _project.Description;
            updateProject.ObjectiveGeneral = _project.ObjectiveGeneral;
            updateProject.ObjectiveImmediate = _project.ObjectiveImmediate;
            updateProject.ObjectiveMediate = _project.ObjectiveMediate;
            updateProject.Methodology = _project.Methodology;
            updateProject.Resources = _project.Resources;
            updateProject.Activities = _project.Activities;
            updateProject.Responsibilities = _project.Responsibilities;
            updateProject.DaysHours = _project.DaysHours;
            updateProject.QuantityPracticing = _project.QuantityPracticing;
            updateProject.IdResponsibleProject = _project.IdResponsibleProject;
            updateProject.IdLinkedOrganization = _project.IdLinkedOrganization;
            int rowsAffected = unitOfWork.Complete();
            unitOfWork.Dispose();
            return rowsAffected == 1;
        }

        private void AddActivityButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            SchedulingActivityRegistration schedulingActivityRegistration = new SchedulingActivityRegistration();
            schedulingActivityRegistration.InitializeComboBoxMonth(_project);
            schedulingActivityRegistration.ShowDialog();
            UpdateListViewSchedulingActivities();
        }

        private void DeleteActivityButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            SchedulingActivity schedulingActivity = ((SchedulingActivity)ListViewSchedulingActivities.SelectedItem);
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea eliminar la actividad?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes && !object.ReferenceEquals(null, schedulingActivity))
            {
                try
                {
                    ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                    UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                    SchedulingActivity schedulingActivityDelete = unitOfWork.SchedulingActivities.Get(schedulingActivity.IdSchedulingActivity);
                    unitOfWork.SchedulingActivities.Remove(schedulingActivityDelete);
                    int rowsAffected = unitOfWork.Complete();
                    unitOfWork.Dispose();
                    if (rowsAffected == 1)
                    {
                        MessageBox.Show("La actividad se eliminó exitosamente", "Eliminación exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar la actividad. Intente más tarde.", "Eliminación Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (EntityException)
                { 
                    MessageBox.Show("No se pudo eliminar la actividad. Intente más tarde.", "Eliminación Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                UpdateListViewSchedulingActivities();
            }
        }

        private void ModifyActivityButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            SchedulingActivity schedulingActivity = ((SchedulingActivity)ListViewSchedulingActivities.SelectedItem);
            SchedulingActivityModification schedulingActivityModification = new SchedulingActivityModification();
            schedulingActivityModification.InitializeSchedulingActivity(_project.Term, schedulingActivity);
            schedulingActivityModification.ShowDialog();
            UpdateListViewSchedulingActivities();
        }

        private void UpdateListViewSchedulingActivities()
        {
            ButtonDeleteActivity.IsEnabled = false;
            ButtonModifyActivity.IsEnabled = false;
            ListViewSchedulingActivities.Items.Clear();
            try
            {
                ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                AddSchedulingActivitiesInListView(unitOfWork);
            }
            catch (EntityException)
            {
                MessageBox.Show("No se pudo actualizar la tabla", "Actualización Fallida", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
