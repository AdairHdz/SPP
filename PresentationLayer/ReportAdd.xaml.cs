
using Microsoft.Win32;
using System;
using System.Windows;
using System.IO;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using System.Data.Entity.Core;
using System.Net;


namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para PartialReportAdd.xaml
    /// </summary>
    public partial class ReportAdd : Window
    {
        private string route;
        private ActivityPracticioner activityPracticioner;
        public ReportAdd()
        {
            InitializeComponent();
        }

        public void InitializeActivity(ActivityPracticioner activityPracticionerReceived, string titleReport)
        {
            TextBlockNameReport.Text = titleReport;
            activityPracticioner = activityPracticionerReceived;
            LabelName.Content = "Actividad: " + activityPracticioner.Activity.Name + "     Valor: " + activityPracticioner.Activity.ValueActivity.ToString();
            LabelDate.Content = "Fecha de Inicio: " + activityPracticioner.Activity.StartDate + " Fecha de Finalización: " + activityPracticioner.Activity.FinishDate;
            TextBlockDescription.Text = "Instrucciones: " + activityPracticioner.Activity.Description;
            if (string.IsNullOrWhiteSpace(activityPracticioner.Activity.Description))
            {
                TextBlockDescription.Text = "Instrucciones: Ninguna";
            }
            TextBoxAnswer.Text = activityPracticioner.Answer;
            LabelNameDocument.Content = activityPracticioner.Document.Name;
            route = activityPracticioner.Document.RouteSave+"/"+ activityPracticioner.Document.Name;
            if (activityPracticioner.Document.DeliveryDate != null)
            {
                LabelDateDeliveryDate.Content = "Fecha de entrega: "+activityPracticioner.Document.DeliveryDate;
                ButtonDownloadFile.IsEnabled = true;
            }
            ValidateDateActivity();
        }

        private void ValidateDateActivity()
        {
            DateTime localDate = DateTime.Now;
            if (activityPracticioner.Activity.ActivityStatus.Equals(ActivityStatus.CANCELLED)|| activityPracticioner.ActivityPracticionerStatus.Equals(ActivityPracticionerStatus.QUALIFIED) ||
                activityPracticioner.Activity.FinishDate < localDate)
            {
                TextBoxAnswer.IsReadOnly = true;
                ButtonAddFile.Visibility = Visibility.Hidden;
                ButtonSave.Visibility = Visibility.Hidden;
                TextBlockObservation.Visibility = Visibility.Visible;
                TextBlockObservation.Text = "Observaciones por el Docente: "+activityPracticioner.Observation;
                if (activityPracticioner.Activity.FinishDate < localDate && !activityPracticioner.Activity.ActivityStatus.Equals(ActivityStatus.CANCELLED))
                {
                    activityPracticioner.Activity.ActivityStatus = ActivityStatus.FINISHED;
                }
            }
        }

        private void BehindButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            ReportList partialReportList = new ReportList();
            partialReportList.InitializeStackPanel(activityPracticioner.Activity.ActivityType);
            partialReportList.Show();
            Close();
        }

        private void AddFileButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            OpenFileDialog search = new OpenFileDialog()
            {
                Filter = "Document files (*.docx)|*.docx| Document files (*.pdf)|*.pdf"
            };
            if (search.ShowDialog() == true)
            {
                route = search.FileName;
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
                        partialReportList.InitializeStackPanel(activityPracticioner.Activity.ActivityType);
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
            string pathDirectoryDocument = activityPracticioner.Document.IdDocument.ToString();
            string pathDirectory =  Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Activity/" + pathDirectoryDocument;
            string path = pathDirectory+ "/" + LabelNameDocument.Content;
            try
            {
                if (!Directory.Exists(pathDirectory))
                {
                    Directory.CreateDirectory(pathDirectory);
                }
                if (!string.IsNullOrWhiteSpace(activityPracticioner.Document.RouteSave))
                {
                    File.Delete(activityPracticioner.Document.RouteSave+ "/" + activityPracticioner.Document.Name);
                }
                if (!route.Equals(path))
                {
                    File.Copy(route, path, true);
                }
                activityPracticioner.Document.Name = LabelNameDocument.Content.ToString();
                activityPracticioner.Document.RouteSave = pathDirectory;
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
                ActivityPracticioner activityPracticionerUpdate = unitOfWork.ActivityPracticioners.Get(activityPracticioner.IdActivityPracticioner);
                activityPracticionerUpdate.Answer = TextBoxAnswer.Text;
                activityPracticionerUpdate.Document.Name = activityPracticioner.Document.Name;
                activityPracticionerUpdate.Document.RouteSave = activityPracticioner.Document.RouteSave;
                activityPracticionerUpdate.Document.TypeDocument = "Reporte Mensual";
                if (activityPracticioner.Activity.ActivityType.Equals(ActivityType.PartialReport))
                {
                    activityPracticionerUpdate.Document.TypeDocument = "Reporte Parcial";
                }
                DateTime deliveryDate = DateTime.Now;
                activityPracticionerUpdate.Document.DeliveryDate = deliveryDate;
                activityPracticionerUpdate.Activity.ActivityStatus = activityPracticioner.Activity.ActivityStatus;

                int rowsAffected = unitOfWork.Complete();
                unitOfWork.Dispose();
                return rowsAffected >= 0 || rowsAffected <= 3;
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
