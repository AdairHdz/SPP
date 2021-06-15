using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using FluentValidation.Results;
using PresentationLayer.Validators;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Threading;
using System.Windows;
using Utilities;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para MonthlyReportGeneration.xaml
    /// </summary>
    public partial class MonthlyReportGeneration : Window
    {
        public static string Enrollment { get; set; }
        private Project _project;
        private readonly MonthlyReport _monthlyReport = new MonthlyReport();
        private int _idMonthlyReport;
        private List<AdvanceQuestion> _advanceQuestions;
        private MonthlyReportTemplate _monthlyReportTemplate;
        public MonthlyReportGeneration()
        {
            InitializeComponent();
            LoadPracticionerInformation();
        }

        private void LoadPracticionerInformation()
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            try
            {
                Practicioner practicioner = unitOfWork.Practicioners.FindFirstOccurence(Practicioner => Practicioner.Enrollment == Enrollment);
                LabelPracticioner.Content = practicioner.User.Name + " " + practicioner.User.LastName;
                _project = unitOfWork.Practicioners.GetPracticionerInformationProject(Enrollment);
                LabelProject.Content = _project.NameProject;
                LabelOrganization.Content = _project.ResponsibleProject.Name + " " + _project.ResponsibleProject.LastName;
                TextBoxCumulativeHours.Text = unitOfWork.Practicioners.GetAccumulatedHours(Enrollment).ToString();
            }
            catch (EntityException)
            {
                ShowExceptionDB();
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }

        private void ShowExceptionDB()
        {
            MessageBox.Show("No hay conexión a la base de datos. Intente más tarde");
            PracticionerMenu practicionerMenu = new PracticionerMenu(Enrollment);
            practicionerMenu.Show();
            Close();
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea cancelar?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                PracticionerMenu practicionerMenu = new PracticionerMenu(Enrollment);
                practicionerMenu.Show();
                Close();
            }
        }

        private void AcceptButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            CreateMonthlyReportFromInputData();
            if (IsValidData())
            {
                string routeDestination = FileExplorer.Show("Guardar reporte mensual");
                if (!string.IsNullOrWhiteSpace(routeDestination))
                {
                    GenerateDocument(routeDestination);
                    ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                    UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                    try
                    {
                        unitOfWork.MonthlyReports.Add(_monthlyReport);
                        unitOfWork.Complete();
                    }
                    catch (EntityException)
                    {
                        ShowExceptionDB();
                    }
                    finally
                    {
                        unitOfWork.Dispose();
                    }
                    AddAnwers();
                    MessageBox.Show("El reporte mensual fue descargado exitosamente");
                    PracticionerMenu practicionerMenu = new PracticionerMenu(Enrollment);
                    practicionerMenu.Show();
                    Close();
                }
            } else
            {
                MessageBox.Show("Por favor ingrese datos correctos");
            }
        }

        private void GenerateDocument(string routeDestination)
        {
            _monthlyReportTemplate = new MonthlyReportTemplate();
            MyDataTemplate();
            MonthlyReportGenerator monthlyReportGenerator = new MonthlyReportGenerator();
            monthlyReportGenerator.CreateMonthlyReportDocument($"{routeDestination}", _monthlyReportTemplate);
            Thread.Sleep(3500);
        }

        private void MyDataTemplate()
        {
            _monthlyReportTemplate.Practicioner = LabelPracticioner.Content.ToString();
            _monthlyReportTemplate.LinkedOrganization = LabelOrganization.Content.ToString();
            _monthlyReportTemplate.Project = LabelProject.Content.ToString();
            DateTime dateTime = (DateTime)DatePickerDate.SelectedDate;
            _monthlyReportTemplate.MonthYear = dateTime.ToString("MM-yyyy");

            _monthlyReportTemplate.Activities = TextBoxActivities.Text;
            _monthlyReportTemplate.Results = TextBoxResults.Text;

            _monthlyReportTemplate.DateTime = _monthlyReport.DeliveryDate.ToString();

            _monthlyReportTemplate.ReasonOne = TextBoxReasonOne.Text;
            _monthlyReportTemplate.ReasonTwo = TextBoxReasonTwo.Text;
            _monthlyReportTemplate.ReasonThree = TextBoxReasonThree.Text;

            AddAnwersToTemplate();
        }

        private void AddAnwersToTemplate()
        {
            if ((bool)RadioButtonYesOne.IsChecked)
            {
                _monthlyReportTemplate.YesOne = "X";
                _monthlyReportTemplate.NoOne = " ";
            }
            else
            {
                _monthlyReportTemplate.YesOne = " ";
                _monthlyReportTemplate.NoOne = "X";
            }
            if ((bool)RadioButtonYesTwo.IsChecked)
            {
                _monthlyReportTemplate.YesTwo = "X";
                _monthlyReportTemplate.NoTwo = " ";
            }
            else
            {
                _monthlyReportTemplate.YesTwo = " ";
                _monthlyReportTemplate.NoTwo = "X";
            }
            if ((bool)RadioButtonYesThree.IsChecked)
            {
                _monthlyReportTemplate.YesThree = "X";
                _monthlyReportTemplate.NoThree = " ";
            }
            else
            {
                _monthlyReportTemplate.YesThree = " ";
                _monthlyReportTemplate.NoThree = "X";
            }
        }

        private void AddAnwers()
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            try
            {
                _idMonthlyReport = unitOfWork.MonthlyReports.GetId();
                CreateAnwersFromInputData();
                foreach (AdvanceQuestion advanceQuestion in _advanceQuestions)
                {
                    unitOfWork.AdvanceQuestions.Add(advanceQuestion);
                    unitOfWork.Complete();
                }

                unitOfWork.Dispose();
            }
            catch (EntityException)
            {
                ShowExceptionDB();
            }
        }

        private void CreateAnwersFromInputData()
        {
            _advanceQuestions = new List<AdvanceQuestion>();
            AdvanceQuestion advanceQuestionOne = new AdvanceQuestion
            {
                IdMonthlyReport = _idMonthlyReport,
                Reasons = TextBoxReasonOne.Text,
                Question = (string)LabelQuestionOne.Content
            };
            if ((bool)RadioButtonYesOne.IsChecked)
            {
                advanceQuestionOne.Reply = true;
            } 
            else
            {
                advanceQuestionOne.Reply = false;
            }

            AdvanceQuestion advanceQuestionTwo = new AdvanceQuestion
            {
                IdMonthlyReport = _idMonthlyReport,
                Reasons = TextBoxReasonTwo.Text,
                Question = (string)LabelQuestionTwo.Content
            };
            if ((bool)RadioButtonYesTwo.IsChecked)
            {
                advanceQuestionTwo.Reply = true;
            }
            else
            {
                advanceQuestionTwo.Reply = false;
            }

            AdvanceQuestion advanceQuestionThree = new AdvanceQuestion
            {
                IdMonthlyReport = _idMonthlyReport,
                Reasons = TextBoxReasonThree.Text,
                Question = (string)LabelQuestionThree.Content
            };
            if ((bool)RadioButtonYesThree.IsChecked)
            {
                advanceQuestionThree.Reply = true;
            }
            else
            {
                advanceQuestionThree.Reply = false;
            }

            _advanceQuestions.Add(advanceQuestionTwo);
            _advanceQuestions.Add(advanceQuestionThree);
            _advanceQuestions.Add(advanceQuestionOne);
           
        }

        private bool IsValidData()
        {
            bool isValid = false;
            MonthlyReportValidator monthlyReportDataValidator = new MonthlyReportValidator();
            ValidationResult dataValidationResult = monthlyReportDataValidator.Validate(_monthlyReport);
            IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
            UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
            userFeedback.ShowFeedback();
            if (ValidateQuestions() && dataValidationResult.IsValid && ValidateMonthYear() && ValidateHours())
            {
                foreach(ValidationFailure v in validationFailures)
                {
                    Console.WriteLine(v);
                }
                isValid = true;
            }
            return isValid;
        }

        private bool ValidateMonthYear()
        {
            bool valid = false;
            if (DatePickerDate.SelectedDate != null)
            {
                if (MonthlyReportValidator.ValidDate(DatePickerDate.SelectedDate))
                {
                    valid = true;
                }
            }
            return valid;
        }

        private bool ValidateHours()
        {
            bool valid = true;
            int hours = Convert.ToInt32(TextBoxReportedHours.Text) + Convert.ToInt32(TextBoxCumulativeHours.Text);
            if (hours > 480)
            {
                valid = false;
            }
            return valid;
        }

        private bool ValidateQuestions()
        {
            bool valid = false;
            if ((bool)RadioButtonNoOne.IsChecked || (bool)RadioButtonYesOne.IsChecked)
            {
                if ((bool)RadioButtonNoTwo.IsChecked || (bool)RadioButtonYesTwo.IsChecked)
                {
                    if((bool)RadioButtonNoThree.IsChecked || (bool)RadioButtonYesThree.IsChecked)
                    {
                        valid = true;
                    }
                }
            }
            return valid;
        }

        private void CreateMonthlyReportFromInputData()
        {
            _monthlyReport.IdProject = _project.IdProject;
            _monthlyReport.DeliveryDate = DateTime.Now;
            _monthlyReport.PerformedActivities = TextBoxActivities.Text;
            _monthlyReport.ResultsObtained = TextBoxResults.Text;
            if (!string.IsNullOrWhiteSpace(TextBoxReportedHours.Text))
            {
                _monthlyReport.HoursReported = Convert.ToInt32(TextBoxReportedHours.Text);
            }
            if (!string.IsNullOrWhiteSpace(TextBoxReportedHours.Text))
            {
                _monthlyReport.HoursCumulative = Convert.ToInt32(TextBoxCumulativeHours.Text) + Convert.ToInt32(TextBoxReportedHours.Text);
            }
            _monthlyReport.Enrollment = Enrollment;
        }
    }
}
