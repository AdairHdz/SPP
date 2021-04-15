using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Windows;

namespace PresentationLayer
{
	/// <summary>
	/// Lógica de interacción para CoordinatorMenu.xaml
	/// </summary>
	public partial class CoordinatorMenu : Window
	{
		public static User User { get; set; }
		public CoordinatorMenu()
		{
			InitializeComponent();
		}

		private void ConsultResponsibleProjectButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			try
			{
				ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
				UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
				IEnumerable<ResponsibleProject> thereAreResponsibleProjects = unitOfWork.ResponsibleProjects.GetAll();
				if (!IENumerableHasResponsibleProjects(thereAreResponsibleProjects))
				{
					MessageBox.Show("No hay ningún responsable del proyecto registrado", "No se puede acceder", MessageBoxButton.OK, MessageBoxImage.Error);
				}
				else
				{
					ResponsibleProjectConsult listResponsibleProject = new ResponsibleProjectConsult();
					listResponsibleProject.Show();
					Close();
				}
			}
			catch (EntityException)
			{
				MessageBox.Show("No se pudo obtener información. Intente más tarde", "No se puede acceder", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		private bool IENumerableHasResponsibleProjects(IEnumerable<ResponsibleProject> ieNumerable)
		{
			bool isFull = false;
			foreach (ResponsibleProject item in ieNumerable)
			{
				isFull = true;
				break;
			}
			return isFull;
		}

		private void RegisterResponsibleProjectButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			ResponsableProjectRegistry registerResponsable = new ResponsableProjectRegistry();
			registerResponsable.Show();
			Close();
		}

		private void LogOutButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			Login login = new Login();
			login.Show();
			Close();
		}

		private void ConsultPracticionerButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
			UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
			IEnumerable<Practicioner> thereArePracticioners = unitOfWork.Practicioners.GetAll();
			if (!IENumerableHasPracticioners(thereArePracticioners))
			{
				MessageBox.Show("No hay ningún practicante registrado", "No se puede acceder", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				PracticionerConsult practicionerConsult = new PracticionerConsult();
				practicionerConsult.Show();
				Close();
			}
		}
		private bool IENumerableHasPracticioners(IEnumerable<Practicioner> ieNumerable)
		{
			bool isFull = false;
			foreach (Practicioner item in ieNumerable)
			{
				isFull = true;
				break;
			}
			return isFull;
		}
		private void RegisterPracticionerButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			PracticionerRegistry practicionerRegistry = new PracticionerRegistry();
			practicionerRegistry.Show();
			Close();
		}

		private void ConsultLinkedOganizationButtonClicked(object sender, RoutedEventArgs e)
		{
			LinkedOrganizationConsultation linkedOrganizationConsultation = new LinkedOrganizationConsultation();
			linkedOrganizationConsultation.Show();
			this.Close();
		}
		
		private void RegisterLinkedOrganizationButtonClicked(object sender, RoutedEventArgs e)
		{
			LinkedOrganizationRegistry linkedOrganizationRegistry = new LinkedOrganizationRegistry();
			linkedOrganizationRegistry.Show();
			this.Close();
		}
		
		private void RegisterProjectButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			try
			{
				ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
				UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
				Coordinator coordinator = unitOfWork.Coordinators.FindFirstOccurence(Coordinator => Coordinator.IdUser == User.IdUser);
				if (!object.ReferenceEquals(null, coordinator)) {
					IEnumerable<ResponsibleProject> thereAreResponsibleProjects = unitOfWork.ResponsibleProjects.Find(ResponsibleProject => ResponsibleProject.ResponsibleProjectStatus == ResponsibleProjectStatus.ACTIVE);
					IEnumerable<LinkedOrganization> thereAreLinkedOrganizations = unitOfWork.LinkedOrganizations.Find(LinkedOrganization => LinkedOrganization.LinkedOrganizationStatus == LinkedOrganizationStatus.ACTIVE);
					if (IENumerableHasResponsibleProjects(thereAreResponsibleProjects) && IENumerableHasLinkedOrganizations(thereAreLinkedOrganizations)) {
						ProjectRegistry projectRegistry = new ProjectRegistry();
						ProjectRegistry.StaffNumber = coordinator.StaffNumber;
						projectRegistry.Show();
						Close();
                    }
                    else
                    {
						MessageBox.Show("No se encuentra un Responsable del proyecto o Organización vinculada registrado", "No se puede acceder", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
			}
			catch (EntityException)
			{
				MessageBox.Show("No se pudo obtener información. Intente más tarde", "No se puede acceder", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private bool IENumerableHasLinkedOrganizations(IEnumerable<LinkedOrganization> ieNumerable)
		{
			bool isFull = false;
			foreach (LinkedOrganization item in ieNumerable)
			{
				isFull = true;
				break;
			}
			return isFull;
		}

		private void ConsultProjectButtonClicked(object sender, RoutedEventArgs e)
		{
			ProjectConsultation projectConsultation = new ProjectConsultation();
			projectConsultation.Show();
			Close();
		}

		private void GroupRegistryButtonClicked(object sender, RoutedEventArgs routedEventArgs )
		{
			ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
			UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
			try
			{
				if (unitOfWork.Practicioners.RequiredPracticionersToGroup() && 
					unitOfWork.Teachers.ActiveTeacher())
				{
					GroupRegistry groupRegistry = new GroupRegistry();
					groupRegistry.Show();
					Close();
					}
				else
				{
					MessageBox.Show("No hay información suficiente para registrar un grupo", "Ingreso Faliido", MessageBoxButton.OK, MessageBoxImage.Error);

				}
			} 
			catch (EntityException)
			{
				MessageBox.Show("No hay conexión con la base de datos. Intente más tarde", "Ingreso Faliido", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
            {
				unitOfWork.Dispose();
            }

		}

		private void GroupModifyButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
			UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
			try
			{
				IEnumerable<Group> thereAreGroups = unitOfWork.Groups.GetAll();
				if (IENumerableHasGroups(thereAreGroups))
				{
					GroupModifyList groupModify = new GroupModifyList();
					groupModify.Show();
					Close();
				}
				else
				{
					MessageBox.Show("No hay ningún grupo registrado. Por favor registre uno", "Ingreso Faliido", MessageBoxButton.OK, MessageBoxImage.Error);

				}
			}
			catch (EntityException)
			{
				MessageBox.Show("No hay conexión con la base de datos. Intente más tarde", "Ingreso Faliido", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				unitOfWork.Dispose();
			}

		}
		private bool IENumerableHasGroups(IEnumerable<Group> ieNumerable)
		{
			bool isFull = false;
			foreach (Group item in ieNumerable)
			{
				isFull = true;
				break;
			}
			return isFull;
		}

        private void AssignProject(object sender, RoutedEventArgs e)
        {
			ProjectSelectionsList projectSelectionsList = new ProjectSelectionsList();
			projectSelectionsList.Show();
			Close();
        }
    }
}
