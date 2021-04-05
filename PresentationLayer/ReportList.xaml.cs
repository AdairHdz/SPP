
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using System.Data.Entity.Core;
namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para PartialReportList.xaml
    /// </summary>
    public partial class ReportList : Window
    {
        private IEnumerable<ActivityPracticioner> activityPracticioners;
        public static User User { get; set; }
        private string activityTypeReport;
        public ReportList()
        {
            InitializeComponent();
        }

        public bool InitializeStackPanel(ActivityType activityType)
        {
            activityTypeReport = "Añadir Reporte Mensual";
            
            try
            {
                ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                Practicioner practicioner = unitOfWork.Practicioners.FindFirstOccurence(Practicioner => Practicioner.IdUser == User.IdUser);
                if (practicioner != null)
                {
                    if (activityType.Equals(ActivityType.PartialReport))
                    {
                        activityTypeReport = "Añadir Reporte Parcial";
                        TextBlockNameActivity.Text = "Reportes Parcial";
                        activityPracticioners = unitOfWork.ActivityPracticioners.Find(ActivityPracticioner => ActivityPracticioner.Enrollment.Equals(practicioner.Enrollment) && ActivityPracticioner.Activity.ActivityType.Equals(ActivityType.PartialReport));
                    }
                    else
                    {
                        activityPracticioners = unitOfWork.ActivityPracticioners.Find(ActivityPracticioner => ActivityPracticioner.Enrollment.Equals(practicioner.Enrollment) && ActivityPracticioner.Activity.ActivityType.Equals(ActivityType.MonthlyReport));
                    }
                    AddReportPartialInListView();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (EntityException)
            {
                return false;
            }
        }

        private void AddReportPartialInListView()
        {
            foreach (ActivityPracticioner activityPracticioner in activityPracticioners)
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
            ActivityPracticioner activityPracticioner = activityPracticioners.ElementAt(itemSelect);
            if (activityPracticioner!=null)
            {
                ReportAdd partialReportAdd = new ReportAdd();
                if (partialReportAdd.InitializeActivity(activityPracticioner, activityTypeReport))
                {
                    partialReportAdd.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("No se pudo obtener información de la actividad. Intente más tarde", "Documento", MessageBoxButton.OK, MessageBoxImage.Warning);
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
