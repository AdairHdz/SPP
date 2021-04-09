using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Windows;

namespace PresentationLayer
{
	/// <summary>
	/// Lógica de interacción para RequestProjectConsult.xaml
	/// </summary>
	public partial class RequestProjectConsult : Window
	{
		private readonly string _enrollment;
		private Project _project;
		private ProfessionalPracticesContext _professionalPracticesContext;
		private UnitOfWork _unitOfWork;
		public RequestProjectConsult (int projectId, string enrollment)
		{
			InitializeComponent();
			_enrollment = enrollment;
			_professionalPracticesContext = new ProfessionalPracticesContext();
			_unitOfWork = new UnitOfWork(_professionalPracticesContext);
			LoadProjectData(projectId);
			this.DataContext = _project;
		}

		private void LoadProjectData(int projectId)
		{
			try
			{
				_project = _unitOfWork.Projects.Get(projectId);
			}
			catch (SqlException)
			{
				ShowException();
			}
			catch (EntityException)
			{
				ShowException();
			}
			finally
            {
				_unitOfWork.Dispose();
            }

		}

		private void BehindButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			RequestProjects requestProject = new RequestProjects(_enrollment);
			requestProject.Show();
			Close();
		}

		private void ShowException()
		{
			MessageBox.Show("No hay conexión a la base de datos. Intente más tarde", "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
			PracticionerMenu practicionerMenu = new PracticionerMenu(_enrollment);
			practicionerMenu.Show();
			Close();
		}

	}
}
