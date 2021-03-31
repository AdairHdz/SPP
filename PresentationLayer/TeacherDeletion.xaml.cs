using System.Windows;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using System.Data.Entity.Core;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para DeleteTeacher.xaml
    /// </summary>
    public partial class TeacherDeletion : Window
    {
        public TeacherDeletion()
        {
            InitializeComponent();
        }

        public void InitializeDataTeacher(Teacher teacher)
        {
            LabelName.Content = teacher.User.Name;
            LabelLastName.Content = teacher.User.LastName;
            LabelEmail.Content = teacher.User.Email;
            LabelAlternateEmail.Content = teacher.User.AlternateEmail;
            LabelStaffNumber.Content = teacher.StaffNumber;
            LabelPhoneNumber.Content = teacher.User.PhoneNumber;
            if(teacher.User.Gender.Equals(Gender.FEMALE))
            {
                LabelGender.Content = "Femenino";
            }
            else
            {
                LabelGender.Content = "Masculino";
            }
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea cancelar?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                TeacherConsultation teacherConsultation = new TeacherConsultation();
                teacherConsultation.Show();
                Close();
            }
        }

        private void BehindButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            TeacherConsultation teacherConsultation = new TeacherConsultation();
            teacherConsultation.Show();
            Close();
        }
    }
}
