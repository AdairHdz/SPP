

using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Windows;


namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para PracticionerMenu.xaml
    /// </summary>
    public partial class PracticionerMenu : Window
    {
        private readonly string _practicionerEnrollment;
        public PracticionerMenu()
        {
            InitializeComponent();
        }

        public PracticionerMenu(string enrollment)
        {
            InitializeComponent();
            _practicionerEnrollment = enrollment;
        }

		private void LogOutButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			Login login = new Login();
			login.Show();
			Close();
		}

		private void AddPartialReport(object sender, RoutedEventArgs e)
		{
			ReportList reportList = new ReportList();
			if (reportList.InitializeStackPanel(ActivityType.PartialReport))
			{
				reportList.Show();
				Close();
			}
			else
			{
				MessageBox.Show("No se encontro actividades. Intente más tarde", "No se puede acceder", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void AddMonthlyReport(object sender, RoutedEventArgs e)
		{
			ReportList reportList = new ReportList();
			if (reportList.InitializeStackPanel(ActivityType.MonthlyReport))
			{
				reportList.Show();
				Close();
			}
			else
			{
				MessageBox.Show("No se encontro actividades. Intente más tarde", "No se puede acceder", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

        private void ConsultProgressButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            try
            {
                bool practicionerHaveAProject = unitOfWork.Practicioners.PracticionerHasActiveProject(_practicionerEnrollment);
                if (practicionerHaveAProject)
                {
                    ConsultProgress consultProgress = new ConsultProgress(_practicionerEnrollment);
                    consultProgress.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("No tiene un proyecto asignado. Contacte a su coordinador", "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (EntityException)
            {
                MessageBox.Show("No hay conexión a la base de datos. Intente más tarde", "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }

        private void RequestProject(object sender, RoutedEventArgs routedEventArgs)
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            try
            {
                bool practicionerHaveAProject = unitOfWork.Practicioners.PracticionerHasActiveProject(_practicionerEnrollment);
                IList<Project> projecsAvailableForThosPracticioner = unitOfWork.Projects.GetProjectsAvailableToRequest(_practicionerEnrollment);
                int requestMade = unitOfWork.RequestProjects.GetPracticionerRequest(_practicionerEnrollment);
                if (practicionerHaveAProject || projecsAvailableForThosPracticioner.Count == 0 || requestMade == 3 )
                {
                    MessageBox.Show("Ya has solocitado un proyecto o no hay proyectos disponibles", "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);      
                }
                else
                {
                    RequestProjects requestProject = new RequestProjects(_practicionerEnrollment);
                    requestProject.Show();
                    Close();
                }
            }
            catch (EntityException)
            {
                MessageBox.Show("No hay conexión a la base de datos. Intente más tarde", "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }

        private void GeneratePartialReportButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            try
            {
                Assignment assignment = unitOfWork.Assignments.FindFirstOccurence(Assignment => Assignment.Enrollment == _practicionerEnrollment);
                if (assignment!=null)
                {
                    IEnumerable<ActivityPracticioner> activities = unitOfWork.ActivityPracticioners.Find(ActivityPracticioner => ActivityPracticioner.Enrollment.Equals(_practicionerEnrollment) &&
                    ActivityPracticioner.Activity.ActivityType == ActivityType.PartialReport && ActivityPracticioner.Activity.ActivityStatus == ActivityStatus.ACTIVE);
                    if (IENumerableHasActivityPracticioners(activities)) {
                        IEnumerable<PartialReport> partialReports = unitOfWork.PartialReports.Find(PartialReport => PartialReport.Enrollment.Equals(_practicionerEnrollment));
                        if (IENumberPartialRepot(partialReports)<2)
                        {
                            PartialReportGeneration partialReportGeneration = new PartialReportGeneration();
                            if (partialReportGeneration.InitializePartialReportGeneration(_practicionerEnrollment))
                            {
                                partialReportGeneration.Show();
                                Close();
                            }
                            else
                            {
                                MessageBox.Show("No hay conexión a la base de datos. Intente más tarde", "Generación Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No tiene activas actividades de reporte parcial. Contacte a su profesor", "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No tiene activas actividades de reporte parcial. Contacte a su profesor", "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("No tiene un proyecto asignado. Contacte a su coordinador", "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (EntityException)
            {
                MessageBox.Show("No hay conexión a la base de datos. Intente más tarde", "Generación Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }
        private bool IENumerableHasActivityPracticioners(IEnumerable<ActivityPracticioner> ieNumerable)
        {
            bool isFull = false;
            foreach (ActivityPracticioner item in ieNumerable)
            {
                isFull = true;
                break;
            }
            return isFull;
        }

        private int IENumberPartialRepot(IEnumerable<PartialReport> ieNumerable)
        {
            int numberPartialReports = 0;
            foreach (PartialReport item in ieNumerable)
            {
                numberPartialReports++;
                break;
            }
            return numberPartialReports;
        }
    }
}
