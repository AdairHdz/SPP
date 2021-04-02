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
        private Teacher teacher;
        public TeacherDeletion()
        {
            InitializeComponent();
        }

        public void InitializeDataTeacher(Teacher teacherReceived)
        {
            teacher = teacherReceived;
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

        private void DeleteButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea eliminar el profesor?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                try
                {
                    ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                    UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                    Teacher deleteTeacher = unitOfWork.Teachers.Get(teacher.StaffNumber);
                    deleteTeacher.User.UserStatus = UserStatus.INACTIVE;
                    int rowsAffected = unitOfWork.Complete();
                    unitOfWork.Dispose();
                    if (rowsAffected == 1)
                    {
                        MessageBox.Show("El Profesor se eliminó exitosamente", "Elimiación Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("El Profesor no pudo eliminarse", "Eliminación Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (EntityException)
                {
                    MessageBox.Show("El Profesor no pudo eliminarse", "Eliminación Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                TeacherConsultation teacherConsultation = new TeacherConsultation();
                teacherConsultation.Show();
                Close();
            }
        }
    }
}
