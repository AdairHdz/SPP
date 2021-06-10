using System;
using System.Collections.Generic;
using System.IO;
using FluentValidation.Results;
using System.Windows;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using System.Data.Entity.Core;
using System.Globalization;
using Utilities;
using PresentationLayer.Validators;
using System.Windows.Media;
using System.Threading;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para ReportPartialGeneration.xaml
    /// </summary>
    public partial class PartialReportGeneration : Window
    {
        private Assignment _assignment;
        private Teacher _teacher;
        private Practicioner  _practicioner;
        private string _numberReport;
        private string _hours;
        private PartialReport _partialReport;
        private List<ActivityMade> _activityMades;
        private PartialReportTemplate _partialReportTemplate;
        private ActivityMade _activityMadeOne;
        private ActivityMade _activityMadeTwo;
        private ActivityMade _activityMadeThree;
        private ActivityMade _activityMadeFour;
        private ActivityMade _activityMadeFive;
        private bool _isValidActivityMade;
        private bool _isValidWeek;
        public List<CheckListItem> ListBoxP1S { get; set; }
        public List<CheckListItem> ListBoxP2S { get; set; }
        public List<CheckListItem> ListBoxP3S { get; set; }
        public List<CheckListItem> ListBoxP4S { get; set; }
        public List<CheckListItem> ListBoxP5S { get; set; }
        public List<CheckListItem> ListBoxR1S { get; set; }
        public List<CheckListItem> ListBoxR2S { get; set; }
        public List<CheckListItem> ListBoxR3S { get; set; }
        public List<CheckListItem> ListBoxR4S { get; set; }
        public List<CheckListItem> ListBoxR5S { get; set; }
        public PartialReportGeneration()
        {
            InitializeComponent();
            CreateCheckBoxList();
        }

        public List<CheckListItem> AddItems(string nameActivity) {
            List<CheckListItem> ListBox = new List<CheckListItem>();
            ListBox.Add(new CheckListItem { TheName = "<" + nameActivity + "1>", TheText = "S1"});
            ListBox.Add(new CheckListItem { TheName = "<" + nameActivity + "2>", TheText = "S2"});
            ListBox.Add(new CheckListItem { TheName = "<" + nameActivity + "3>", TheText = "S3"});
            ListBox.Add(new CheckListItem { TheName = "<" + nameActivity + "4>", TheText = "S4"});
            ListBox.Add(new CheckListItem { TheName = "<" + nameActivity + "5>", TheText = "S5"});
            ListBox.Add(new CheckListItem { TheName = "<" + nameActivity + "6>", TheText = "S6"});
            ListBox.Add(new CheckListItem { TheName = "<" + nameActivity + "7>", TheText = "S7"});
            ListBox.Add(new CheckListItem { TheName = "<" + nameActivity + "8>", TheText = "S8"});
            return ListBox;
        }
        public void CreateCheckBoxList()
        {
            ListBoxP1S = AddItems("P1S");
            ListBoxP2S = AddItems("P2S");
            ListBoxP3S = AddItems("P3S");
            ListBoxP4S = AddItems("P4S");
            ListBoxP5S = AddItems("P5S");

            ListBoxR1S = AddItems("R1S");
            ListBoxR2S = AddItems("R2S");
            ListBoxR3S = AddItems("R3S");
            ListBoxR4S = AddItems("R4S");
            ListBoxR5S = AddItems("R5S");
            this.DataContext = this;
        }
        public bool InitializePartialReportGeneration(string enrrollment)
        {
            try
            {
                ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                _practicioner = unitOfWork.Practicioners.FindFirstOccurence(Practicioner => Practicioner.Enrollment == enrrollment);
                if (_practicioner != null)
                {
                    _assignment = unitOfWork.Assignments.FindFirstOccurence(Assignment => Assignment.Practicioner.Enrollment == _practicioner.Enrollment);
                    if (_assignment != null)
                    {
                        _teacher = unitOfWork.Teachers.FindFirstOccurence(Teacher => Teacher.StaffNumber == _practicioner.Group.StaffNumber);
                        if (_teacher != null)
                        {
                            PartialReport partialReport = unitOfWork.PartialReports.FindFirstOccurence(PartialReport => PartialReport.Enrollment == _practicioner.Enrollment);
                            if (partialReport != null)
                            {
                                _numberReport = "SEGUNDO";
                                CreateHours(false);
                            }
                            else
                            {
                                _numberReport = "PRIMERO";
                                CreateHours(true);
                            }
                            InitializeAssignment();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (IOException)
            {
                return false;
            }
        }

        private void CreateHours(bool isReportOne)
        {
            DateTime localDate = DateTime.Now;
            int month = localDate.Month;
            String year = localDate.Year.ToString();
            if (month > 1 && month <= 7)
            {
                if (isReportOne)
                {
                    _hours = "Febrero-Marzo " + year + " 240 HRS.";
                }
                else
                {
                    _hours = "Abril-Julio " + year + " 240 HRS.";
                }
            }
            else
            {
                if (isReportOne)
                {
                    _hours = "Agosto-Octubre " + year + " 240 HRS.";
                }
                else
                {
                    _hours = "Septiembre-Enero " + year + " 240 HRS.";
                }
            }
        }

        private void InitializeAssignment()
        {
            LabelCareer.Content = "Licenciatura en Ingeniería de Software";
            LabelTeacher.Content = "M.C. Ma. " + _teacher.User.Name + " " + _teacher.User.LastName;
            LabelNRC.Content = _assignment.Practicioner.Group.Nrc;
            LabelTerm.Content = _assignment.Practicioner.Term;
            LabelPracticioner.Content = _practicioner.User.Name + " " + _practicioner.User.LastName;
            TextBlockProject.Text = _assignment.Project.NameProject;
            TextBlockLinkedOrganization.Text = _assignment.Project.LinkedOrganization.Name;
            TextBlockMethodology.Text = _assignment.Project.Methodology;
            TextBlockObjectiveGeneral.Text = _assignment.Project.ObjectiveGeneral;
            LabelNumberReport.Content = _numberReport;
            TextBlockHours.Text = _hours;
            DateTime localDate = DateTime.Now;
            string day = localDate.Day.ToString();
            int month = localDate.Month;
            string year = localDate.Year.ToString();
            DateTimeFormatInfo formatDate = CultureInfo.CurrentCulture.DateTimeFormat;
            string nameMonth = formatDate.GetMonthName(month);
            LabelDate.Content = day + " de " + nameMonth + " de " + year;
        }


        private void MyDataTemplate()
        {
            _partialReportTemplate.Career = LabelCareer.Content.ToString();
            _partialReportTemplate.Techaer = LabelTeacher.Content.ToString();
            _partialReportTemplate.NRC = LabelNRC.Content.ToString();
            _partialReportTemplate.Term = LabelTerm.Content.ToString();
            _partialReportTemplate.Practicioner = LabelPracticioner.Content.ToString();
            _partialReportTemplate.Project = TextBlockProject.Text;
            _partialReportTemplate.Date = LabelDate.Content.ToString();
            _partialReportTemplate.LinkedOrganization = TextBlockLinkedOrganization.Text;
            _partialReportTemplate.Hours = TextBlockHours.Text;
            _partialReportTemplate.Number = LabelNumberReport.Content.ToString();
            _partialReportTemplate.GeneralObjective = TextBlockObjectiveGeneral.Text;
            _partialReportTemplate.Methodology = TextBlockMethodology.Text;
            _partialReportTemplate.ActivityOne = TextBoxActivityOne.Text;
            _partialReportTemplate.ActivityTwo = TextBoxActivityTwo.Text;
            _partialReportTemplate.ActivityThree = TextBoxActivityThree.Text;
            _partialReportTemplate.ActivityFour = TextBoxActivityFour.Text;
            _partialReportTemplate.ActivityFive = TextBoxActivityFive.Text;
            _partialReportTemplate.Result = TextBoxResults.Text;
            _partialReportTemplate.Observations = TextBoxObservations.Text;
            _partialReportTemplate.WeekPlan1 = ReplaceWordWeek(ListBoxP1);
            _partialReportTemplate.WeekPlan2 = ReplaceWordWeek(ListBoxP2);
            _partialReportTemplate.WeekPlan3 = ReplaceWordWeek(ListBoxP3);
            _partialReportTemplate.WeekPlan4 = ReplaceWordWeek(ListBoxP4);
            _partialReportTemplate.WeekPlan5 = ReplaceWordWeek(ListBoxP5);
            _partialReportTemplate.WeekReal1 = ReplaceWordWeek(ListBoxR1);
            _partialReportTemplate.WeekReal2 = ReplaceWordWeek(ListBoxR2);
            _partialReportTemplate.WeekReal3 = ReplaceWordWeek(ListBoxR3);
            _partialReportTemplate.WeekReal4 = ReplaceWordWeek(ListBoxR4);
            _partialReportTemplate.WeekReal5 = ReplaceWordWeek(ListBoxR5);
        }

        private List<CheckListItem> ReplaceWordWeek(System.Windows.Controls.ListBox listBox){
            List<CheckListItem> listItems = (List < CheckListItem > )listBox.ItemsSource;
            foreach (CheckListItem item in listItems)
            {
                if (item.IsSelected == true)
                {
                    item.TheValue = "X";
                }
            }
            return listItems;
        }

        private string ObteinWeek(System.Windows.Controls.ListBox listBox)
        {
            string week = null;
            List<CheckListItem> listWeek = (List<CheckListItem>)listBox.ItemsSource;
            List<CheckListItem> listWeekSelect = listWeek.FindAll(CheckListItem => CheckListItem.IsSelected == true);
            foreach (CheckListItem item in listWeekSelect)
            {
                week = week + item.TheText + " ";
            }
            return week;
        }

        private void GenerateButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea generar el documento?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            try
            {
                if (messageBoxResult == MessageBoxResult.Yes)
                {                    
                    CreatePatialReportFromInputData();
                    CreateActivityMadesFromInputData();
                    if (ValidateDataPatialReport())
                    {
                        string routeDestination = FileExplorer.Show("Guardar reporte parcial");
                        if (!string.IsNullOrWhiteSpace(routeDestination))
                        {
                            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                            if (RegisternewPartialReport(unitOfWork))
                            {
                                _partialReportTemplate = new PartialReportTemplate();
                                MyDataTemplate();
                                PartialReportGenerator partialReportGenerator = new PartialReportGenerator();
                                partialReportGenerator.CreatePartialReportDocument($"{routeDestination}", _partialReportTemplate);
                                Thread.Sleep(3500);
                                MessageBox.Show("El documento se genero correctamente", "Documento Generado", MessageBoxButton.OK, MessageBoxImage.Information);
                                PracticionerMenu practicionerMenu = new PracticionerMenu(_practicioner.Enrollment);
                                practicionerMenu.Show();
                                Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Por favor, Ingrese el nombre del documento", "Datos Incorrectos", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Por favor, Complete todos los campos", "Datos Incorrectos", MessageBoxButton.OK, MessageBoxImage.Warning);
                        
                    }
                }
            }
            catch (EntityException)
            {
                MessageBox.Show("No se pudo generar el reporte. Intente más tarde", "Genración Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
                PracticionerMenu practicionerMenu = new PracticionerMenu(_practicioner.Enrollment);
                practicionerMenu.Show();
                Close();
            }
        }

        private bool ValidateDataPatialReport()
        {
            _isValidActivityMade = true;
            _isValidWeek = true;
            _activityMades = new List<ActivityMade>();
            PartialReportValidator partialReportValidator = new PartialReportValidator();
            ValidationResult dataValidationResult = partialReportValidator.Validate(_partialReport);
            IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
            UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
            userFeedback.ShowFeedback();
            ValidateActivityMade();
            ValidateWeek(ListBoxP1);
            ValidateWeek(ListBoxP2);
            ValidateWeek(ListBoxP3);
            ValidateWeek(ListBoxP4);
            ValidateWeek(ListBoxP5);

            ValidateWeek(ListBoxR1);
            ValidateWeek(ListBoxR2);
            ValidateWeek(ListBoxR3);
            ValidateWeek(ListBoxR4);
            ValidateWeek(ListBoxR5);

            return dataValidationResult.IsValid && _isValidActivityMade && _activityMades.Count != 0 && _isValidWeek;
        }

        private void ValidateWeek(System.Windows.Controls.ListBox listBoxWeek)
        {
            List<CheckListItem> listWeek = (List<CheckListItem>)listBoxWeek.ItemsSource;
            List<CheckListItem> listWeekSelect = listWeek.FindAll(CheckListItem => CheckListItem.IsSelected == true);
            if (listWeekSelect.Count == 0)
            {
                _isValidWeek = false;
            }
        }

        private void ValidateActivityMade()
        {
            TextBoxActivityOne.BorderBrush = Brushes.Green;
            TextBoxActivityTwo.BorderBrush = Brushes.Green;
            TextBoxActivityThree.BorderBrush = Brushes.Green;
            TextBoxActivityFour.BorderBrush = Brushes.Green;
            TextBoxActivityFive.BorderBrush = Brushes.Green;

            ActivityMadeValidator activityMadeValidator = new ActivityMadeValidator();
            ValidationResult dataValidationResultOne = activityMadeValidator.Validate(_activityMadeOne);
            if (!dataValidationResultOne.IsValid)
            {
                _isValidActivityMade = false;
                TextBoxActivityOne.BorderBrush = Brushes.Red;
            }
            ValidationResult dataValidationResultTwo = activityMadeValidator.Validate(_activityMadeTwo);
            if (!dataValidationResultTwo.IsValid)
            {
                _isValidActivityMade = false;
                TextBoxActivityTwo.BorderBrush = Brushes.Red;
            }
            ValidationResult dataValidationResultThree = activityMadeValidator.Validate(_activityMadeThree);
            if (!dataValidationResultThree.IsValid)
            {
                _isValidActivityMade = false;
                TextBoxActivityThree.BorderBrush = Brushes.Red;
            }
            ValidationResult dataValidationResultFour = activityMadeValidator.Validate(_activityMadeFour);
            if (!dataValidationResultFour.IsValid)
            {
                _isValidActivityMade = false;
                TextBoxActivityFour.BorderBrush = Brushes.Red;
            }
            ValidationResult dataValidationResultFive = activityMadeValidator.Validate(_activityMadeFive);
            if (!dataValidationResultFive.IsValid)
            {
                _isValidActivityMade = false;
                TextBoxActivityFive.BorderBrush = Brushes.Red;
            }
            _activityMades.Add(_activityMadeOne);
            _activityMades.Add(_activityMadeTwo);
            _activityMades.Add(_activityMadeThree);
            _activityMades.Add(_activityMadeFour);
            _activityMades.Add(_activityMadeFive);
        }

        private void CreatePatialReportFromInputData()
        {
            _partialReport = new PartialReport();
            DateTime localDate = DateTime.Now;
            _partialReport.NumberReport = _numberReport;
            _partialReport.ResultsObtained = TextBoxResults.Text;
            _partialReport.HoursCovered = 240;
            _partialReport.Observations = TextBoxObservations.Text;
            _partialReport.DeliveryDate = localDate;
            _partialReport.IdProject = _assignment.IdProject;
            _partialReport.Enrollment = _practicioner.Enrollment;
        }

        private void CreateActivityMadesFromInputData()
        {
            _activityMadeOne = new ActivityMade();
            _activityMadeTwo = new ActivityMade();
            _activityMadeThree = new ActivityMade();
            _activityMadeFour = new ActivityMade();
            _activityMadeFive = new ActivityMade();
            _activityMadeOne.Name = TextBoxActivityOne.Text;
            _activityMadeTwo.Name = TextBoxActivityTwo.Text;
            _activityMadeThree.Name = TextBoxActivityThree.Text;
            _activityMadeFour.Name = TextBoxActivityFour.Text;
            _activityMadeFive.Name = TextBoxActivityFive.Text;
            _activityMadeOne.PlannedWeek = ObteinWeek(ListBoxP1);
            _activityMadeTwo.PlannedWeek = ObteinWeek(ListBoxP2);
            _activityMadeThree.PlannedWeek = ObteinWeek(ListBoxP3);
            _activityMadeFour.PlannedWeek = ObteinWeek(ListBoxP4);
            _activityMadeFive.PlannedWeek = ObteinWeek(ListBoxP5);
            _activityMadeOne.RealWeek = ObteinWeek(ListBoxR1);
            _activityMadeTwo.RealWeek = ObteinWeek(ListBoxR2);
            _activityMadeThree.RealWeek = ObteinWeek(ListBoxR3);
            _activityMadeFour.RealWeek = ObteinWeek(ListBoxR4);
            _activityMadeFive.RealWeek = ObteinWeek(ListBoxR5);
        }

        private bool RegisternewPartialReport(UnitOfWork unitOfWork)
        {
            unitOfWork.PartialReports.Add(_partialReport);
            int rowsAffected = unitOfWork.Complete();
            return RegisterActivityMades(unitOfWork) && rowsAffected == 1;
        }

        private bool RegisterActivityMades(UnitOfWork unitOfWork)
        {
            PartialReport partialReport = unitOfWork.PartialReports.FindFirstOccurence(PartialReport => PartialReport.IdProject == _partialReport.IdProject
            && PartialReport.Enrollment.Equals(_partialReport.Enrollment) && PartialReport.NumberReport.Equals(_partialReport.NumberReport));
            if (partialReport!=null)
            {
                _partialReport.IdParcialReport = partialReport.IdParcialReport;
                AddIdPartialReportInActivitiesMade();
                unitOfWork.ActivityMades.AddRange(_activityMades);
                int rowsAffected = unitOfWork.Complete();
                unitOfWork.Dispose();
                return rowsAffected.Equals(_activityMades.Count);
            }
            return false;
        }

        private void AddIdPartialReportInActivitiesMade()
        {
            for (int index = 0; index < _activityMades.Count; index++)
            {
                _activityMades[index].IdPartialReport = _partialReport.IdParcialReport;
            }
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea cancelar?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                PracticionerMenu practicionerMenu = new PracticionerMenu(_practicioner.Enrollment);
                practicionerMenu.Show();
                Close();
            }
        }

        private void BehindButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            PracticionerMenu practicionerMenu = new PracticionerMenu(_practicioner.Enrollment);
            practicionerMenu.Show();
            Close();
        }
    }
}
