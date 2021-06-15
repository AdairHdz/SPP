using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using FluentValidation.Results;
using PresentationLayer.Validators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Data.Entity.Core;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para ManageReportsAddActivity.xaml
    /// </summary>
    public partial class ManageReportsAddActivity : Window
    {
        private readonly string _staffNumber;
        public Activity Activity = new Activity();
        private readonly int _group;

        public ManageReportsAddActivity(string staffNumber, int idGroup)
        {
            _staffNumber = staffNumber;
            _group = idGroup;
            InitializeComponent();  
            this.DataContext = Activity;
            ComboBoxType.SelectedIndex = 1;
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
        }

        private void AddButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            CreateActivity();
            if (ValidateData())
            {
                ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                try
                {
                    unitOfWork.Activities.Add(Activity);
                    unitOfWork.Complete();
                    AddActivityToPracticioners();
                    MessageBox.Show("Actividad agregada exitosamente.");
                    this.Close();
                }
                catch (SqlException)
                {
                    CatchDBException();
                }
                finally
                {
                    unitOfWork.Dispose();
                }
            }
            else
            {
                MessageBox.Show("Ingrese datos válidos");
            }
        }

        private void AddActivityToPracticioners()
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            try
            {
                IList<Practicioner> practicioners = unitOfWork.Activities.GetPracticionersToActivity(_group);
                int idActivity = unitOfWork.Activities.GetId();
                ActivityPracticioner activity = new ActivityPracticioner
                {
                    Qualification = 0,
                    IdActivity = idActivity,
                    ActivityPracticionerStatus = ActivityPracticionerStatus.NOTQUALIFIED
                };
                foreach (Practicioner practicioner in practicioners)
                {
                    activity.Enrollment = practicioner.Enrollment;
                    unitOfWork.ActivityPracticioners.Add(activity);
                    unitOfWork.Complete();
                }
               
                unitOfWork.Dispose();
            }
            catch (EntityException)
            {
                CatchDBException();
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
            if (dataValidationResult.IsValid && ValidDateTime())
            {
                isValid = true;
            }
            return isValid;
        }

        private bool ValidDateTime()
        {
            bool isValid = false;
            if (DatePickerDateStart.SelectedDate == null || TimePickerTimeStart.SelectedTime == null ||
                DatePickerDateFinish.SelectedDate == null || TimePickerTimeFinish.SelectedTime == null)
            {
                return isValid;
            }
            string dateStart = DatePickerDateStart.SelectedDate.Value.ToString("yyyy-MM-dd");
            string hourStart = TimePickerTimeStart.SelectedTime.Value.ToString("HH:mm:ss");
            DateTime dateTimeStart = Convert.ToDateTime(dateStart + " " + hourStart);

            string dateFinish = DatePickerDateFinish.SelectedDate.Value.ToString("yyyy-MM-dd");
            string hourFinish = TimePickerTimeFinish.SelectedTime.Value.ToString("HH:mm:ss");
            DateTime dateTimeFinish = Convert.ToDateTime(dateFinish + " " + hourFinish);

            
            if (ActivityValidator.ValidDate(dateTimeStart) && ActivityValidator.ValidDate(dateTimeFinish)
                && ActivityValidator.ValidDateStartAndFinish(dateTimeStart, dateTimeFinish)) 
            {
                isValid = true;
                Activity.FinishDate = dateTimeFinish;
                Activity.StartDate = dateTimeStart;
            }
            return isValid;
        }

        private void CreateActivity()
        {
            Activity.StaffNumberTeacher = _staffNumber;
            Activity.IdGroup = _group;
            int selectedType = int.Parse(((System.Windows.Controls.ComboBoxItem)ComboBoxType.SelectedItem).Tag.ToString());
            if (selectedType == 0)
            {
                Activity.ActivityType = ActivityType.PartialReport;
            }
            else
            {
                Activity.ActivityType = ActivityType.MonthlyReport;
            }
             Activity.ActivityStatus = ActivityStatus.ACTIVE;
        }

    }
}

