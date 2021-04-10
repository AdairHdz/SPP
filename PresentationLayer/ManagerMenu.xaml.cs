using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Windows;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para AdministratorMenu.xaml
    /// </summary>
    public partial class ManagerMenu : Window
    {
        public ManagerMenu()
        {
            InitializeComponent();
        }

        private void ConsultTeacherButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                IEnumerable<Teacher> thereAreTeachers = unitOfWork.Teachers.GetAll();
                if (!IENumerableHasTeachers(thereAreTeachers))
                {
                    MessageBox.Show("No hay ningún profesor registrado", "No se puede acceder", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    TeacherConsultation teacherConsultation = new TeacherConsultation();
                    teacherConsultation.Show();
                    Close();
                }
            }
            catch (EntityException)
            {
                MessageBox.Show("No se pudo obtener información. Intente más tarde", "No se puede acceder", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        private bool IENumerableHasTeachers(IEnumerable<Teacher> ieNumerable)
        {
            bool isFull = false;
            foreach (Teacher item in ieNumerable)
            {
                isFull = true;
                break;
            }
            return isFull;
        }

        private void LogOutButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            Login login = new Login();
            login.Show();
            Close();
        }

        private void RegisterCoordinatorButtonClicked(object sender, RoutedEventArgs e)
        {
            CoordinatorRegistry coordinatorRegistry = new CoordinatorRegistry();
            coordinatorRegistry.Show();
            Close();
        }

        private void RegisterTeacherButtonClicked(object sender, RoutedEventArgs e)
        {
            TeacherRegistry teacherRegistry = new TeacherRegistry();
            teacherRegistry.Show();
            Close();
        }

        private void ConsultCoordinatorButtonClicked(object sender, RoutedEventArgs e)
        {
            CoordinatorConsultation coordinatorConsultation = new CoordinatorConsultation();
            coordinatorConsultation.Show();
            Close();
        }
    }
}
