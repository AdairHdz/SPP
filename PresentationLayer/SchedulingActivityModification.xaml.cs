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
    /// Lógica de interacción para SchedulingActivityModification.xaml
    /// </summary>
    public partial class SchedulingActivityModification : Window
    {
        private SchedulingActivity _schedulingActivity;
        public SchedulingActivityModification()
        {
            InitializeComponent();
        }
        public void InitializeSchedulingActivity(string term, SchedulingActivity schedulingActivityReceived)
        {
            _schedulingActivity = schedulingActivityReceived;
            string[] valuesTerm = term.Split(' ');
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
            ComboBoxMonth.SelectedItem = schedulingActivityReceived.Month;
            TextBoxActivity.Text = schedulingActivityReceived.Activity;
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
        }

        private void ModifyButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
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
                        if (UpdatenewSchedulingActivity(unitOfWork))
                        {
                            MessageBox.Show("La atividad se modifico exitosamente", "Modificación Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                            Close();
                        }
                    }
                }
                catch (EntityException)
                {
                    MessageBox.Show("La actividad no se pudo modificar. Intente más tarde", "Modificación Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
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
            _schedulingActivity.Month = ComboBoxMonth.SelectedItem.ToString();
            ComboBoxMonth.BorderBrush = Brushes.Green;
            _schedulingActivity.Activity = TextBoxActivity.Text;
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
            SchedulingActivity schedulingActivityWithSameName = unitOfWork.SchedulingActivities.FindFirstOccurence(SchedulingActivity => SchedulingActivity.IdProject == _schedulingActivity.IdProject && SchedulingActivity.Month.Equals(_schedulingActivity.Month) && SchedulingActivity.IdSchedulingActivity != _schedulingActivity.IdSchedulingActivity);
            if (schedulingActivityWithSameName!=null)
            {
                return true;
            }
            return false;
        }

        private bool UpdatenewSchedulingActivity(UnitOfWork unitOfWork)
        {
            SchedulingActivity updateSchedulingActivity = unitOfWork.SchedulingActivities.Get(_schedulingActivity.IdSchedulingActivity);
            updateSchedulingActivity.Month = _schedulingActivity.Month;
            updateSchedulingActivity.Activity = _schedulingActivity.Activity;
            int rowsAffected = unitOfWork.Complete();
            unitOfWork.Dispose();
            return rowsAffected == 1;
        }
    }
}
