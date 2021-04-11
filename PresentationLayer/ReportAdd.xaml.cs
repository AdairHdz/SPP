
using Microsoft.Win32;
using System;
using System.Windows;
using System.IO;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Net;


namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para PartialReportAdd.xaml
    /// </summary>
    public partial class ReportAdd : Window
    {
        private string _route;
        private ActivityPracticioner _activityPracticioner;
        private Document _document;
        public ReportAdd()
        {
            InitializeComponent();
        }

        public bool InitializeActivity(ActivityPracticioner activityPracticionerReceived, string titleReport)
        {
            TextBlockNameReport.Text = titleReport;
            _activityPracticioner = activityPracticionerReceived;
            LabelName.Content = "Actividad: " + _activityPracticioner.Activity.Name + "     Valor: " + _activityPracticioner.Activity.ValueActivity.ToString();
            LabelDate.Content = "Fecha de Inicio: " + _activityPracticioner.Activity.StartDate + " Fecha de Finalización: " + _activityPracticioner.Activity.FinishDate;
            TextBlockDescription.Text = "Instrucciones: " + _activityPracticioner.Activity.Description;
            if (string.IsNullOrWhiteSpace(_activityPracticioner.Activity.Description))
            {
                TextBlockDescription.Text = "Instrucciones: Ninguna";
            }
            return ObteinDocument();
        }

        private bool ObteinDocument() {
            try
            {
                ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                _document = unitOfWork.Documents.FindFirstOccurence(Document => Document.IdActivityPracticioner == _activityPracticioner.IdActivityPracticioner);
                if (_document != null)
                {
                    InitializeDocument();
                    return true;
                }
                else
                {
                    if (AddDocumentPracticioner(unitOfWork))
                    {
                        _document = unitOfWork.Documents.FindFirstOccurence(Document => Document.IdActivityPracticioner == _activityPracticioner.IdActivityPracticioner);
                        if (_document != null)
                        {
                            InitializeDocument();
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
            }
            catch (SqlException)
            {
                return false;
            }
            catch (EntityException)
            {
                return false;
            }
        }

        private bool AddDocumentPracticioner(UnitOfWork unitOfWork)
        {
            Document addDocument = new Document();
            addDocument.TypeDocument = "Reporte Mensual";
            if (_activityPracticioner.Activity.ActivityType.Equals(ActivityType.PartialReport))
            {
                addDocument.TypeDocument = "Reporte Parcial";
            }
            addDocument.IdActivityPracticioner = _activityPracticioner.IdActivityPracticioner;
            unitOfWork.Documents.Add(addDocument);
            int rowsAffected = unitOfWork.Complete();
            return rowsAffected == 1;
        }

        private void InitializeDocument()
        {
            TextBoxAnswer.Text = _activityPracticioner.Answer;
            LabelNameDocument.Content = _document.Name;
            _route = _document.RouteSave + "/" + _document.Name;
            if (_document.DeliveryDate != null)
            {
                LabelDateDeliveryDate.Content = "Fecha de entrega: " + _document.DeliveryDate;
                ButtonDownloadFile.IsEnabled = true;
            }
            ValidateDateActivity();
        }

        private void ValidateDateActivity()
        {
            DateTime localDate = DateTime.Now;
            if (_activityPracticioner.Activity.ActivityStatus.Equals(ActivityStatus.CANCELLED)|| _activityPracticioner.ActivityPracticionerStatus.Equals(ActivityPracticionerStatus.QUALIFIED) ||
                _activityPracticioner.Activity.FinishDate < localDate)
            {
                TextBoxAnswer.IsReadOnly = true;
                ButtonAddFile.Visibility = Visibility.Hidden;
                ButtonSave.Visibility = Visibility.Hidden;
                TextBlockObservation.Visibility = Visibility.Visible;
                TextBlockObservation.Text = "Observaciones por el Docente: "+_activityPracticioner.Observation;
                if (_activityPracticioner.Activity.FinishDate < localDate && !_activityPracticioner.Activity.ActivityStatus.Equals(ActivityStatus.CANCELLED))
                {
                    _activityPracticioner.Activity.ActivityStatus = ActivityStatus.FINISHED;
                }
            }
        }

        private void BehindButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            ReportList partialReportList = new ReportList();
            partialReportList.InitializeStackPanel(_activityPracticioner.Activity.ActivityType);
            partialReportList.Show();
            Close();
        }

        private void AddFileButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            OpenFileDialog search = new OpenFileDialog()
            {
                Filter = "Document files (*.pdf)|*.pdf"
            };
            if (search.ShowDialog() == true)
            {
                _route = search.FileName;
                LabelNameDocument.Content = search.SafeFileName;
                ButtonDownloadFile.IsEnabled = true;
            }   
        }

        private void SaveFileButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            if (LabelNameDocument.Content!=null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea guardar el archivo?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    if (ValidateAnswer())
                    {
                        if (CreateDirectoryDocument() && AddDocument())
                        {
                            MessageBox.Show("El archivo se guardo exitosamente", "Guardado Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("El archivo no se pudo guardar. Intente más tarde", "Guardado Fallido", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        ReportList partialReportList = new ReportList();
                        partialReportList.InitializeStackPanel(_activityPracticioner.Activity.ActivityType);
                        partialReportList.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Solo se permite 254 en la respuesta", "Longitud de caracteres", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }
        
        private bool CreateDirectoryDocument()
        {
            string pathDirectoryDocument = _document.IdDocument.ToString();
            string pathDirectory =  Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Activity/" + pathDirectoryDocument;
            string path = pathDirectory+ "/" + LabelNameDocument.Content;
            try
            {
                if (!Directory.Exists(pathDirectory))
                {
                    Directory.CreateDirectory(pathDirectory);
                }
                if (!string.IsNullOrWhiteSpace(_document.RouteSave))
                {
                    File.Delete(_document.RouteSave+ "/" + _document.Name);
                }
                if (!_route.Equals(path))
                {
                    File.Copy(_route, path, true);
                }
                _document.Name = LabelNameDocument.Content.ToString();
                _document.RouteSave = pathDirectory;
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        private bool ValidateAnswer()
        {
            if (TextBoxAnswer.Text.Length < 256)
            {
                return true;
            }
            return false;
        }


        private bool AddDocument()
        {
            try
            {
                ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                ActivityPracticioner activityPracticionerUpdate = unitOfWork.ActivityPracticioners.Get(_activityPracticioner.IdActivityPracticioner);
                activityPracticionerUpdate.Answer = TextBoxAnswer.Text;
                Document documentUpdate = unitOfWork.Documents.Get(_document.IdDocument);
                documentUpdate.Name = _document.Name;
                documentUpdate.RouteSave = _document.RouteSave;
                DateTime deliveryDate = DateTime.Now;
                documentUpdate.DeliveryDate = deliveryDate;
                activityPracticionerUpdate.Activity.ActivityStatus = _activityPracticioner.Activity.ActivityStatus;

                int rowsAffected = unitOfWork.Complete();
                unitOfWork.Dispose();
                return rowsAffected >= 0 || rowsAffected <= 3;
            }
            catch (SqlException)
            {
                return false;
            }
            catch (EntityException)
            {
                return false;
            }
        }

        private void DownloadFileButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!string.IsNullOrWhiteSpace(LabelNameDocument.Content.ToString()))
            {
                if (File.Exists(_route))
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
                                routeDestination = routeDestination.Replace(".pdf", "(" + index.ToString() + ")" + ".pdf");
                            }
                            else
                            {
                                routeDestination = routeDestination.Replace("(" + indexPrevious.ToString() + ")" + ".pdf", "(" + index.ToString() + ")" + ".pdf");
                            }
                            index++;
                        }
                        else
                        {
                            myWebClient.DownloadFile(_route, routeDestination);
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
