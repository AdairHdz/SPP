
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using System.Data.Entity.Core;
using System.Data.SqlClient;
namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para PartialReportList.xaml
    /// </summary>
    public partial class ReportList : Window
    {
        private IEnumerable<ActivityPracticioner> _activityPracticioners;
        public static User _User { get; set; }
        private string _activityTypeReport;
        public ReportList()
        {
            InitializeComponent();
        }

        public bool InitializeStackPanel(ActivityType activityType)
        {
            _activityTypeReport = "Añadir Reporte Mensual";
            
            try
            {
                ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                Practicioner practicioner = unitOfWork.Practicioners.FindFirstOccurence(Practicioner => Practicioner.IdUser == _User.IdUser);
                if (practicioner != null)
                {
                    if (activityType.Equals(ActivityType.PartialReport))
                    {
                        _activityTypeReport = "Añadir Reporte Parcial";
                        TextBlockNameActivity.Text = "Reportes Parcial";
                        _activityPracticioners = unitOfWork.ActivityPracticioners.Find(ActivityPracticioner => ActivityPracticioner.Enrollment.Equals(practicioner.Enrollment) && ActivityPracticioner.Activity.ActivityType.Equals(ActivityType.PartialReport));
                    }
                    else
                    {
                        _activityPracticioners = unitOfWork.ActivityPracticioners.Find(ActivityPracticioner => ActivityPracticioner.Enrollment.Equals(practicioner.Enrollment) && ActivityPracticioner.Activity.ActivityType.Equals(ActivityType.MonthlyReport));
                    }
                    if (IENumerableHasActivityPracticioners(_activityPracticioners))
                    {
                        AddReportPartialInListView();
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
            catch (SqlException)
            {
                return false;
            }
            catch (EntityException)
            {
                return false;
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

        private void AddReportPartialInListView()
        {
            foreach (ActivityPracticioner activityPracticioner in _activityPracticioners)
            {
                ListViewActivity.Items.Add(
                     new
                     {
                         Name = activityPracticioner.Activity.Name,
                         Date = activityPracticioner.Activity.StartDate + " - " + activityPracticioner.Activity.FinishDate,
                         Value = "Valor: " + activityPracticioner.Activity.ValueActivity.ToString(),
                         Qualification = activityPracticioner.Qualification.ToString()
                     }
                 );
            }
        }

        private void PartialReportItemsControlMouseDoubleClicked(object listViewReport, RoutedEventArgs routedEventArgs)
        {
            int itemSelect = ((ListView)listViewReport).SelectedIndex;
            ActivityPracticioner activityPracticioner = _activityPracticioners.ElementAt(itemSelect);
            if (activityPracticioner!=null)
            {
                ReportAdd partialReportAdd = new ReportAdd();
                if (partialReportAdd.InitializeActivity(activityPracticioner, _activityTypeReport))
                {
                    partialReportAdd.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("No se pudo obtener información de la actividad. Intente más tarde", "No se puede acceder", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
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
