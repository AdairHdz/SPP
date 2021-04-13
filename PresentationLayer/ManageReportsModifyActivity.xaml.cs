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
                ColocateActivity();
                
            }
            catch (SqlException)
            {
                _unitOfWork.Dispose();
                CatchDBException();
            }
        }

        private void ColocateActivity()
        {
            TextBoxValue.Text = Activity.ValueActivity.ToString();
          
            if (Activity.ActivityStatus == ActivityStatus.ACTIVE)
            {
                ComboBoxStatus.SelectedIndex = 0;
            }
            else
            {
                if (Activity.ActivityStatus == ActivityStatus.FINISHED)
                {
                    ComboBoxStatus.SelectedIndex = 2;
                } 
                else
                {
                    ComboBoxStatus.SelectedIndex = 1;
                }
            }

            if (Activity.ActivityType == ActivityType.MonthlyReport)
            {
                ComboBoxType.SelectedIndex = 1;
            }
            else
            {
                ComboBoxType.SelectedIndex = 0;
            }

            DatePickerDate.SelectedDate = Activity.FinishDate;
            TimePickerHour.SelectedTime = Activity.FinishDate;

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
            } else
            {
                MessageBox.Show("Ingrese datos válidos");
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
            ActivityValidator activityValidator = new ActivityValidator(true);
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
            string date = DatePickerDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            string hour = TimePickerHour.SelectedTime.Value.ToString("HH:mm:ss");
            DateTime dateFinish = Convert.ToDateTime(date + " " + hour);
            bool isValid = true;
            if (dateFinish != Activity.FinishDate)
            {
                isValid = ActivityValidator.ValidDate(dateFinish);
                if (isValid)
                {
                    Activity.FinishDate = Convert.ToDateTime(dateFinish);
                } 
                else
                {
                    isValid = false;
                }
            }
            return isValid;
        }

        private void CreateActivity()
        {
            int selectedType = int.Parse(((System.Windows.Controls.ComboBoxItem)ComboBoxType.SelectedItem).Tag.ToString());
            if (selectedType == 0)
            {
                Activity.ActivityType = ActivityType.PartialReport;
            }
            else
            {
                Activity.ActivityType = ActivityType.MonthlyReport;
            }

            int selectedStatus = int.Parse(((System.Windows.Controls.ComboBoxItem)ComboBoxStatus.SelectedItem).Tag.ToString());
            if (selectedStatus == 0)
            {
                Activity.ActivityStatus = ActivityStatus.ACTIVE;
            }
            else
            {
                if (selectedStatus == 1)
                {
                    Activity.ActivityStatus = ActivityStatus.CANCELLED;
                }
                else
                {
                    Activity.ActivityStatus = ActivityStatus.FINISHED;
                }
            }

        }

    }
}
