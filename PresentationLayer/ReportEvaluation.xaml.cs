using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using System.Data.Entity.Core;
using System.Net;
using System.IO;
using System;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para EvaluateReport.xaml
    /// </summary>
    public partial class ReportEvaluation : Window
    {
        private Activity activity;
        private IEnumerable<ActivityPracticioner> _activityPracticioners;
        private ActivityPracticioner _activityPracticioner;
        private IEnumerable<Document> _documents;
        private Document _document;
        public ReportEvaluation()
        {
            InitializeComponent();
        }

        public bool InitializeActivity(Activity activityReceived)
        {
            activity = activityReceived;
            LabelName.Content = activity.Name;
            LabelStartDate.Content = activity.StartDate;
            LabelFinishDate.Content = activity.FinishDate;
            LabelValueActivity.Content = activity.ValueActivity;
            TextBlockDescription.Text = activity.Description;
            if (string.IsNullOrWhiteSpace(activity.Description))
            {
                TextBlockDescription.Text = "Ninguna";
            }
            try
            {
                ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                ObteinActivitiesPracticioners(unitOfWork);
                ObteinDocument(unitOfWork);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        private void ObteinActivitiesPracticioners(UnitOfWork unitOfWork)
        {
            _activityPracticioners = unitOfWork.ActivityPracticioners.Find(ActivityPracticioner => ActivityPracticioner.IdActivity == activity.IdActivity);
            AddPracticionersInListView();
        }

        private void ObteinDocument(UnitOfWork unitOfWork)
        {
            _documents = unitOfWork.Documents.Find(Document => Document.ActivityPracticioner.IdActivity == activity.IdActivity);
        }

        private void AddPracticionersInListView()
        {
            foreach (ActivityPracticioner activityPracticioner in _activityPracticioners)
            {
               ListViewPracticioners.Items.Add(
                   new
                   {
                       Enrollment = activityPracticioner.Enrollment,
                       Name = activityPracticioner.Practicioner.User.Name +" "+ activityPracticioner.Practicioner.User.LastName
                   }
               );
            }
        }

        private void PracticionerListViewSelectionChanged(object listViewPracticioner, SelectionChangedEventArgs selectionChanged)
        {
            TextBoxObservation.Text = null;
            TextBoxQualification.Text = null;
            int itemSelected = ListViewPracticioners.SelectedIndex;
            _activityPracticioner = _activityPracticioners.ElementAt(itemSelected);
            if (_activityPracticioner!=null)
            {
                _document = _documents.FirstOrDefault(Document => Document.IdActivityPracticioner == _activityPracticioner.IdActivityPracticioner);
                LabelNameDocument.Content = _document.Name;
                TextBlockAnswer.Text = _activityPracticioner.Answer;
                if (!string.IsNullOrWhiteSpace(_document.RouteSave))
                {
                    ButtonDownloadFile.IsEnabled = true;
                    ButtonQualification.IsEnabled = true;
                    TextBoxObservation.IsReadOnly = false;
                    TextBoxQualification.IsReadOnly = false;
                    TextBoxObservation.Text = _activityPracticioner.Observation;
                    TextBoxQualification.Text = _activityPracticioner.Qualification.ToString();
                }
                else
                {
                    ButtonDownloadFile.IsEnabled = false;
                    ButtonQualification.IsEnabled = true;
                    TextBoxObservation.IsReadOnly = false;
                    TextBoxQualification.IsReadOnly = false;
                    TextBoxObservation.Text = _activityPracticioner.Observation;
                    TextBoxQualification.Text = _activityPracticioner.Qualification.ToString();
                }
            }
        }

        private void BehindButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            ActivityReportList activityReportList = new ActivityReportList();
            activityReportList.InitializeStackPanel();
            activityReportList.Show();
            Close();
        }

        private void QualificationButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            if (ValidateObservation())
            {
                if (ValidateQualification())
                {
                    try
                    {
                        ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                        UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                        if (UpdateActivityPracticioner(unitOfWork))
                        {
                            MessageBox.Show("La calificación se asigno exitosamente", "Calificación Asignada", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (EntityException)
                    {
                        MessageBox.Show("No se pudo calificar al practicante. Intente más tarde", "Calificación Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Asigne una calificación correcta", "Datos Incorrectos", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Máximo 255 caracteres en las observaciones", "Datos Incorrectos", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool UpdateActivityPracticioner(UnitOfWork unitOfWork)
        {
            ActivityPracticioner updateActivityPracticioner = unitOfWork.ActivityPracticioners.Get(_activityPracticioner.IdActivityPracticioner);
            updateActivityPracticioner.Observation = _activityPracticioner.Observation;
            updateActivityPracticioner.Qualification = _activityPracticioner.Qualification;
            updateActivityPracticioner.ActivityPracticionerStatus = ActivityPracticionerStatus.QUALIFIED;
            int rowsAffected = unitOfWork.Complete();
            unitOfWork.Dispose();
            return rowsAffected == 1;
        }

        private bool ValidateObservation()
        {
            if (!string.IsNullOrWhiteSpace(TextBoxObservation.Text))
            {
                _activityPracticioner.Observation = TextBoxObservation.Text;
            }
            if(TextBoxObservation.Text.Length <= 255)
            {
                return true;
            }
            return false;
        }

        private bool ValidateQualification()
        {
            if (!string.IsNullOrWhiteSpace(TextBoxQualification.Text))
            {
                try
                {
                    double qualification = double.Parse(TextBoxQualification.Text);
                    if(qualification>=0 && qualification <= activity.ValueActivity)
                    {
                        _activityPracticioner.Qualification = qualification;
                        return true;
                    }
                }
                catch (FormatException)
                {
                    return false;
                }
            }
            return false;
        }

        private void DownloadFileButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!string.IsNullOrWhiteSpace(LabelNameDocument.Content.ToString()))
            {
                string route = _document.RouteSave + "/" + _document.Name;
                if (File.Exists(route))
                {
                    WebClient myWebClient = new WebClient();
                    string routeDestination = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/" + LabelNameDocument.Content.ToString();
                    int index = 1;
                    bool isDownload = true;
                    while (isDownload)
                    {
                        if (File.Exists(routeDestination))
                        {
                            int indexPrevious = index - 1;
                            if (index == 1)
                            {
                                routeDestination = routeDestination.Replace(".docx", "(" + index.ToString() + ")" + ".docx");
                                routeDestination = routeDestination.Replace(".pdf", "(" + index.ToString() + ")" + ".pdf");
                            }
                            else
                            {
                                routeDestination = routeDestination.Replace("(" + indexPrevious.ToString() + ")" + ".docx", "(" + index.ToString() + ")" + ".docx");
                                routeDestination = routeDestination.Replace("(" + indexPrevious.ToString() + ")" + ".pdf", "(" + index.ToString() + ")" + ".pdf");
                            }
                            index++;
                        }
                        else
                        {
                            myWebClient.DownloadFile(route, routeDestination);
                            MessageBox.Show("El archivo se descargo la carpeta del escritorio", "Descarga", MessageBoxButton.OK, MessageBoxImage.Information);
                            isDownload = false;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("El archivo no se encontro", "Descarga Fallida", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}
