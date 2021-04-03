using System.Windows;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using PresentationLayer.Validators;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Windows.Media;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para SchedulingActivityRegistration.xaml
    /// </summary>
    public partial class SchedulingActivityRegistration : Window
    {
        private SchedulingActivity _schedulingActivity;
        private Project _project;
        public SchedulingActivityRegistration()
        {
            InitializeComponent();
        }
        public void InitializeComboBoxMonth(Project projectReceived)
        {
            _project = projectReceived;
            string[] valuesTerm = _project.Term.Split(' ');
            if (valuesTerm[0].Equals("FEBRERO-JULIO"))
            {
                ComboBoxMonth.Items.Add("Febrero");
                ComboBoxMonth.Items.Add("Marzo");
                ComboBoxMonth.Items.Add("Abril");
                ComboBoxMonth.Items.Add("Mayo");
            }
            else
            {
                ComboBoxMonth.Items.Add("Agosto");
                ComboBoxMonth.Items.Add("Septiembre");
                ComboBoxMonth.Items.Add("Octubre");
                ComboBoxMonth.Items.Add("Noviembre");
            }
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
        }

        private void AddButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            _schedulingActivity = new SchedulingActivity();
            CreateSchedulingActivityFromInputData();
            if (ValidateDataSchedulingActivity())
            {
                try
                {
                    ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                    UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                    if (SchedulingActivityIsAlreadyRegistered(unitOfWork))
                    {
                        MessageBox.Show("Existe una actividad con el mismo mes registrado", "Dato Repetido", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        if (AddnewSchedulingActivity(unitOfWork))
                        {
                            MessageBox.Show("La atividad se agrego exitosamente", "Agregación Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("La actividad no pudo agregarse. Intente más tarde", "Agregación Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        Close();
                    }
                }
                catch (EntityException)
                {
                    MessageBox.Show("La actividad no pudo agregarse. Intente más tarde", "Agregación Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Por favor, Ingrese datos correctos en los campos marcados en rojo", "Datos Incorrectos", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CreateSchedulingActivityFromInputData()
        {
            if (ComboBoxMonth.SelectedItem != null)
            {
                _schedulingActivity.Month = ComboBoxMonth.SelectedItem.ToString();
                ComboBoxMonth.BorderBrush = Brushes.Green;
            }
            else
            {
                ComboBoxMonth.BorderBrush = Brushes.Red;
            }
            _schedulingActivity.Activity = TextBoxActivity.Text;
            _schedulingActivity.IdProject = _project.IdProject;
        }

        private bool ValidateDataSchedulingActivity()
        {
            SchedulingActivityValidator schedulingActivityValidator = new SchedulingActivityValidator();
            ValidationResult dataValidationResult = schedulingActivityValidator.Validate(_schedulingActivity);
            IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
            UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
            userFeedback.ShowFeedback();
            return dataValidationResult.IsValid;
        }

        private bool SchedulingActivityIsAlreadyRegistered(UnitOfWork unitOfWork)
        {
            SchedulingActivity schedulingActivityWithSameName = unitOfWork.SchedulingActivities.FindFirstOccurence(SchedulingActivity => SchedulingActivity.IdProject == _project.IdProject && SchedulingActivity.Month.Equals(_schedulingActivity.Month));
            if (schedulingActivityWithSameName!=null)
            {
                return true;
            }
            return false;
        }

        private bool AddnewSchedulingActivity(UnitOfWork unitOfWork)
        {
            unitOfWork.SchedulingActivities.Add(_schedulingActivity);
            int rowsAffected = unitOfWork.Complete();
            unitOfWork.Dispose();
            return rowsAffected == 1;
        }
    }
}
