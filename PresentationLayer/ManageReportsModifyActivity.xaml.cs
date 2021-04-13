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
    /// Lógica de interacción para ManageReportsModifyActivity.xaml
    /// </summary>
    public partial class ManageReportsModifyActivity : Window
    {
        private string _staffNumber;
        public Activity Activity = new Activity();
        private readonly ProfessionalPracticesContext _professionalPracticesContext;
        private readonly UnitOfWork _unitOfWork;
        public ManageReportsModifyActivity(string staffNumber, int idActivity)
        {
            _staffNumber = staffNumber;
            _professionalPracticesContext = new ProfessionalPracticesContext();
            _unitOfWork = new UnitOfWork(_professionalPracticesContext);
            InitializeComponent();
            LoadActivity(idActivity);
            this.DataContext = Activity;
        }

        private void LoadActivity(int idActivity)
        {
            try
            {
                Activity = _unitOfWork.Activities.Get(idActivity);
                
            }
            catch (SqlException)
            {
                _unitOfWork.Dispose();
                CatchDBException();
            }
        }
        private void CancelButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
        }

        private void ModifyButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            CreateActivity();
            if (ValidateData())
            {
                try
                {
                    _unitOfWork.Complete();
                    MessageBox.Show("Actividad modificada exitosamente.");
                    this.Close();
                }
                catch (SqlException)
                {
                    CatchDBException();
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
            }
        }

        private void CatchDBException()
        {
            MessageBox.Show("No hay conexión con la base de datos. Intente más tarde.");
            TeacherMenu teacherMenu = new TeacherMenu(_staffNumber);
            teacherMenu.Show();
            this.Close();
        }

        private bool ValidateData()
        {
            bool isValid = false;
            ActivityValidator activityValidator = new ActivityValidator();
            ValidationResult dataValidationResult = activityValidator.Validate(Activity);

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

        private void CreateActivity()
        {
            string date = DatePickerDate.SelectedDate.ToString();
            string hour = TimePickerHour.SelectedTime.ToString();
            Activity.FinishDate = Convert.ToDateTime(date + " " + hour);

            int selectedType = int.Parse(((System.Windows.Controls.ComboBoxItem)ComboBoxType.SelectedItem).Tag.ToString());
            if (selectedType == 0)
            {
                Activity.ActivityType = ActivityType.MonthlyReport;
            }
            else
            {
                Activity.ActivityType = ActivityType.PartialReport;
            }

            int selectedStatus = int.Parse(((System.Windows.Controls.ComboBoxItem)ComboBoxStatus.SelectedItem).Tag.ToString());
            if (selectedStatus == 0)
            {
                Activity.ActivityStatus = ActivityStatus.CANCELLED;
            }
            else
            {
                if (selectedStatus == 1)
                {
                    Activity.ActivityStatus = ActivityStatus.ACTIVE;
                }
                else
                {
                    Activity.ActivityStatus = ActivityStatus.FINISHED;
                }
            }

        }
    }
}
