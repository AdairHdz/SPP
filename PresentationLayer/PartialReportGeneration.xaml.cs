using System;
using System.Collections.Generic;
using System.IO;
using FluentValidation.Results;
using System.Windows;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using System.Data.Entity.Core;
using System.Collections.ObjectModel;
using Word = Microsoft.Office.Interop.Word;
using System.Globalization;
using PresentationLayer.Utils;
using PresentationLayer.Validators;

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
        private string _routeDestination;
        private string _routeCurrent;
        private object _missing;
        private Word.Document _wordDocumentPartialReport;
        private Word.Application _wordApp;
        private ActivityMade _activityMadeOne;
        private ActivityMade _activityMadeTwo;
        private ActivityMade _activityMadeThree;
        private ActivityMade _activityMadeFour;
        private ActivityMade _activityMadeFive;
        private bool _isValidActivityMade;
        public ObservableCollection<CheckListItem> ListBoxP1S { get; set; }
        public ObservableCollection<CheckListItem> ListBoxP2S { get; set; }
        public ObservableCollection<CheckListItem> ListBoxP3S { get; set; }
        public ObservableCollection<CheckListItem> ListBoxP4S { get; set; }
        public ObservableCollection<CheckListItem> ListBoxP5S { get; set; }
        public ObservableCollection<CheckListItem> ListBoxR1S { get; set; }
        public ObservableCollection<CheckListItem> ListBoxR2S { get; set; }
        public ObservableCollection<CheckListItem> ListBoxR3S { get; set; }
        public ObservableCollection<CheckListItem> ListBoxR4S { get; set; }
        public ObservableCollection<CheckListItem> ListBoxR5S { get; set; }
        public PartialReportGeneration()
        {
            InitializeComponent();
            CreateCheckBoxList();
        }

        public ObservableCollection<CheckListItem> AddItems(string nameActivity) {
            ObservableCollection<CheckListItem> ListBox = new ObservableCollection<CheckListItem>();
            ListBox.Add(new CheckListItem { TheName = "<" + nameActivity + "1>", TheText = "S1", TheValue = 1 });
            ListBox.Add(new CheckListItem { TheName = "<" + nameActivity + "2>", TheText = "S2", TheValue = 2 });
            ListBox.Add(new CheckListItem { TheName = "<" + nameActivity + "3>", TheText = "S3", TheValue = 3 });
            ListBox.Add(new CheckListItem { TheName = "<" + nameActivity + "4>", TheText = "S4", TheValue = 4 });
            ListBox.Add(new CheckListItem { TheName = "<" + nameActivity + "5>", TheText = "S5", TheValue = 5 });
            ListBox.Add(new CheckListItem { TheName = "<" + nameActivity + "6>", TheText = "S6", TheValue = 6 });
            ListBox.Add(new CheckListItem { TheName = "<" + nameActivity + "7>", TheText = "S7", TheValue = 7 });
            ListBox.Add(new CheckListItem { TheName = "<" + nameActivity + "8>", TheText = "S8", TheValue = 8 });
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
                        _teacher = unitOfWork.Teachers.FindFirstOccurence(Teacher => Teacher.StaffNumber == _assignment.Practicioner.Group.StaffNumber);
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
            string day = localDate.ToString();
            int month = localDate.Month;
            string year = localDate.Year.ToString();
            DateTimeFormatInfo formatDate = CultureInfo.CurrentCulture.DateTimeFormat;
            string nameMonth = formatDate.GetMonthName(month);
            LabelDate.Content = day + " de " + nameMonth + " de " + year;
        }

        private void FindAndReplace(Word.Application wordApp, object ToFindText, object replaceWithText)
        {
            object matchCase = true;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundLike = false;
            object matchAllForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiactitics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;

            wordApp.Selection.Find.Execute(ref ToFindText,
                ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundLike,
                ref matchAllForms, ref forward,
                ref wrap, ref format, ref replaceWithText,
                ref replace, ref matchKashida,
                ref matchDiactitics, ref matchAlefHamza,
                ref matchControl);
        }

        private void CreateWordDocument(object filename, object saveAs)
        {
            _wordApp = new Word.Application();
            _missing = System.Reflection.Missing.Value;
            _wordDocumentPartialReport = null;
            if (File.Exists((string)filename))
            {
                object readOnly = false;
                object isVisible = false;
                _wordApp.Visible = false;
                _wordDocumentPartialReport = _wordApp.Documents.Open(ref filename, ref _missing, ref readOnly,
                    ref _missing, ref _missing, ref _missing,
                    ref _missing, ref _missing, ref _missing,
                    ref _missing, ref _missing, ref _missing,
                    ref _missing, ref _missing, ref _missing, ref _missing);
                _wordDocumentPartialReport.Activate();
                ReplaceWordPartialReport(_wordApp);
            }
            else
            {
                MessageBox.Show("No se encontro la plantilla. Intente más tarde", "No se encontro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SaveDocument(object saveAs)
        {
            int index = 1;
            bool isDownload = true;
            string routeDestination = (string)saveAs;
            while (isDownload)
            {
                if (File.Exists(routeDestination))
                {
                    int indexPrevious = index - 1;
                    if (index == 1)
                    {
                        routeDestination = routeDestination.Replace(".docx", "(" + index.ToString() + ")" + ".docx");
                    }
                    else
                    {
                        routeDestination = routeDestination.Replace("(" + indexPrevious.ToString() + ")" + ".docx", "(" + index.ToString() + ")" + ".docx");
                    }
                    index++;
                }
                else
                {
                    object saveNewAs = routeDestination;
                    _wordDocumentPartialReport.SaveAs2(ref saveNewAs, ref _missing, ref _missing, ref _missing,
                    ref _missing, ref _missing, ref _missing,
                    ref _missing, ref _missing, ref _missing,
                    ref _missing, ref _missing, ref _missing,
                    ref _missing, ref _missing, ref _missing);

                    _wordDocumentPartialReport.Close();
                    isDownload = false;
                }
            }
        }

        private void ReplaceWordPartialReport(Word.Application wordApp)
        {
            FindAndReplace(wordApp, "<Career>", LabelCareer.Content);
            FindAndReplace(wordApp, "<Teacher>", LabelTeacher.Content);
            FindAndReplace(wordApp, "<NRC>", LabelNRC.Content);
            FindAndReplace(wordApp, "<Term>", LabelTerm.Content);

            FindAndReplace(wordApp, "<Practicioner>", LabelPracticioner.Content);
            FindAndReplace(wordApp, "<Project>", TextBlockProject.Text);
            FindAndReplace(wordApp, "<Date>", LabelDate.Content);
            FindAndReplace(wordApp, "<LinkedOrganization>", TextBlockLinkedOrganization.Text);
            FindAndReplace(wordApp, "<Hours>", TextBlockHours.Text);
            FindAndReplace(wordApp, "<Number>", LabelNumberReport.Content);

            FindAndReplace(wordApp, "<GeneralObjective>", TextBlockObjectiveGeneral.Text);
            FindAndReplace(wordApp, "<Methodology>", TextBlockMethodology.Text);

            FindAndReplace(wordApp, "<ActivityOne>", TextBoxActivityOne.Text);
            FindAndReplace(wordApp, "<ActivityTwo>", TextBoxActivityTwo.Text);
            FindAndReplace(wordApp, "<ActivityThree>", TextBoxActivityThree.Text);
            FindAndReplace(wordApp, "<ActivityOne>", TextBoxActivityOne.Text);
            FindAndReplace(wordApp, "<ActivityOne>", TextBoxActivityOne.Text);
            FindAndReplace(wordApp, "<ActivityFour>", TextBoxActivityFour.Text);
            FindAndReplace(wordApp, "<ActivityFive>", TextBoxActivityFive.Text);

            _activityMadeOne.PlannedWeek = ReplaceWordWeek(wordApp, ListBoxP1);
            _activityMadeTwo.PlannedWeek = ReplaceWordWeek(wordApp, ListBoxP2);
            _activityMadeThree.PlannedWeek = ReplaceWordWeek(wordApp, ListBoxP3);
            _activityMadeFour.PlannedWeek = ReplaceWordWeek(wordApp, ListBoxP4);
            _activityMadeFive.PlannedWeek = ReplaceWordWeek(wordApp, ListBoxP5);
            _activityMadeOne.RealWeek = ReplaceWordWeek(wordApp, ListBoxR1);
            _activityMadeTwo.RealWeek = ReplaceWordWeek(wordApp, ListBoxR2);
            _activityMadeThree.RealWeek = ReplaceWordWeek(wordApp, ListBoxR3);
            _activityMadeFour.RealWeek = ReplaceWordWeek(wordApp, ListBoxR4);
            _activityMadeFive.RealWeek = ReplaceWordWeek(wordApp, ListBoxR5);

            FindAndReplace(wordApp, "<Result>", TextBoxResults.Text);
            FindAndReplace(wordApp, "<Observations>", TextBoxObservations.Text);
        }

        private string ReplaceWordWeek(Word.Application wordApp, System.Windows.Controls.ListBox listBox){
            string week = null;
            foreach (CheckListItem item in listBox.ItemsSource)
            {
                if (item.IsSelected == true)
                {
                    FindAndReplace(wordApp, item.TheName, "X");
                    week = week +item.TheText+ "";
                }
                else
                {
                    FindAndReplace(wordApp, item.TheName, null);
                }
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
                    CreatePatialReportFromInputData();
                    GererateRouteDocument();
                    CreateWordDocument(_routeCurrent, _routeDestination);
                    if (ValidateDataPatialReport())
                    {
                        ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                        UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                        if (RegisternewPartialReport(unitOfWork))
                        {
                            SaveDocument(_routeDestination);
                            _wordApp.Quit();
                            MessageBox.Show("El documento se genero correctamente en el escritorio", "Documento Generado", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Por favor, Ingrese datos correctos en los campos marcados en rojo", "Datos Incorrectos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (EntityException)
            {
                MessageBox.Show("El proyecto no pudo generar el reporte. Intente más tarde", "Genración Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
                PracticionerMenu practicionerMenu = new PracticionerMenu();
                practicionerMenu.Show();
                Close();
            }
        }

        private bool ValidateDataPatialReport()
        {
            _isValidActivityMade = true;
            _activityMades = new List<ActivityMade>();
            PartialReportValidator partialReportValidator = new PartialReportValidator();
            ValidationResult dataValidationResult = partialReportValidator.Validate(_partialReport);
            IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
            UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
            userFeedback.ShowFeedback();
            ValidateActivityMade(TextBoxActivityOne.Name, ListBoxP1.Name, ListBoxR1.Name, _activityMadeOne);
            ValidateActivityMade(TextBoxActivityTwo.Name, ListBoxP2.Name, ListBoxR2.Name, _activityMadeTwo);
            ValidateActivityMade(TextBoxActivityThree.Name, ListBoxP3.Name, ListBoxR3.Name, _activityMadeThree);
            ValidateActivityMade(TextBoxActivityFour.Name, ListBoxP4.Name, ListBoxR4.Name, _activityMadeFour);
            ValidateActivityMade(TextBoxActivityFour.Name, ListBoxP5.Name, ListBoxR5.Name, _activityMadeFive);
            return dataValidationResult.IsValid && _isValidActivityMade && _activityMades.Count != 0;
        }

        private void ValidateActivityMade(string nameTextBoxActivity , string nameListBoxPlan, string nameListBoxReal, ActivityMade activityMade)
        {
            /*TextBoxActivity.BorderBrush = Brushes.Gray;*/
            ActivityMadeValidator activityMadeValidator = new ActivityMadeValidator(nameTextBoxActivity, nameListBoxPlan, nameListBoxReal);
            ValidationResult dataValidationResult = activityMadeValidator.Validate(activityMade);
            if (!dataValidationResult.IsValid)
            {
                _isValidActivityMade = false;
            }
             _activityMades.Add(activityMade);
        }

        private void GererateRouteDocument()
        {
            _routeDestination = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/" + "reporteParcial.docx";
            _routeCurrent = AppDomain.CurrentDomain.BaseDirectory;
            _routeCurrent = _routeCurrent.Replace(@"\bin\Debug\", "/Reports/partialReport.docx");
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
            if (!object.ReferenceEquals(null, partialReport))
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
                PracticionerMenu practicionerMenu = new PracticionerMenu();
                practicionerMenu.Show();
                Close();
            }
        }

        private void BehindButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            PracticionerMenu practicionerMenu = new PracticionerMenu();
            practicionerMenu.Show();
            Close();
        }
    }
}
