
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
    /// Lógica de interacción para ActivityReportList.xaml
    /// </summary>
    public partial class ActivityReportList : Window
    {
        private IEnumerable<Activity> _activities;
        private static string _StaffNumberTeacher;
        public static User _User { get; set; }
        public ActivityReportList()
        {
            InitializeComponent();
        }

        public bool InitializeStackPanel()
        {
            try
            {
                ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                Teacher teacher = unitOfWork.Teachers.FindFirstOccurence(Teacher => Teacher.IdUser == _User.IdUser);
                if (teacher != null)
                {
                    _StaffNumberTeacher = teacher.StaffNumber;
                    _activities = unitOfWork.Activities.Find(Activity => Activity.StaffNumberTeacher.Equals(_StaffNumberTeacher) && (Activity.ActivityType.Equals(ActivityType.MonthlyReport) || Activity.ActivityType.Equals(ActivityType.PartialReport)));
                    foreach (Activity activity in _activities)
                    {
                        ListViewActivity.Items.Add(
                             new
                             {
                                 Name = activity.Name,
                                 StartDate = "Fecha de Inicio: " + activity.StartDate,
                                 FinishDate = "Fecha de finalización: " + activity.FinishDate,
                                 Value = activity.ValueActivity.ToString()
                             }
                         );
                    }
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

        private void BehindButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            TeacherMenu teacherMenu = new TeacherMenu();
            teacherMenu.Show();
            Close();
        }

        private void PartialReportItemsControlMouseDoubleClicked(object listViewReport, RoutedEventArgs routedEventArgs)
        {
            int itemSelect = ((ListView)listViewReport).SelectedIndex;
            Activity activity = _activities.ElementAt(itemSelect);
            if (!object.ReferenceEquals(null, activity))
            {
                ReportEvaluation evaluateReport = new ReportEvaluation();
                if (evaluateReport.InitializeActivity(activity))
                {
                    evaluateReport.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("No se encontro entregas. Intente más tarde", "Ingreso Faliido", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

    }
}
