using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PresentationLayer
{
	/// <summary>
	/// Lógica de interacción para RequestProjecst.xaml
	/// </summary>
	public partial class RequestProjects : Window
	{
		private readonly string _enrollment;
		public RequestProject _requestProject = new RequestProject();
		private readonly ProfessionalPracticesContext _professionalPracticesContext;
		private readonly UnitOfWork _unitOfWork;

		public RequestProjects(string enrollment)
		{
			InitializeComponent();
			_professionalPracticesContext = new ProfessionalPracticesContext();
			_unitOfWork = new UnitOfWork(_professionalPracticesContext);
			_enrollment = enrollment;
			LoadInformation();
			this.DataContext = _requestProject;
		}

		private void BackButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea cancelar?",
				"Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (messageBoxResult == MessageBoxResult.Yes)
			{
				PracticionerMenu practicionerMenu = new PracticionerMenu(_enrollment);
				practicionerMenu.Show();
				Close();
			}
		}

		private void RequestButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			Project project = ((Project)ListViewProjects.SelectedItem);
			if (project != null)
			{
				MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro que deseas solicitar el proyecto?",
				"Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

				if (messageBoxResult == MessageBoxResult.Yes)
				{
					CreateRequest(project.IdProject);
					try
					{
						_unitOfWork.RequestProjects.Add(_requestProject);
						MessageBox.Show("El proyecto se solicitó exitosamente");
						PracticionerMenu practicionerMenu = new PracticionerMenu(_enrollment);
						practicionerMenu.Show();
						Close();
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
						_unitOfWork.Complete();
						_unitOfWork.Dispose();
					}
				}
			}

		}

		private void CreateRequest(int idProject)
        {
			_requestProject.Enrollment = _enrollment;
			_requestProject.RequestDate = DateTime.Now;
			_requestProject.IdProject = idProject;
			_requestProject.RequestStatus = RequestStatus.REQUESTED;
        }

		private void SelectProject(object sender, SelectionChangedEventArgs selectionChanged)
		{
			Project project = ((Project)ListViewProjects.SelectedItem);
			if (project != null)
			{
				ButtonRequest.IsEnabled = true;
			}
		}

		private void ProjectConsult(object sender, MouseButtonEventArgs e)
		{
			Project project = ((Project)ListViewProjects.SelectedItem);
			if (project != null)
			{
				RequestProjectConsult requestProjectConsult = new RequestProjectConsult(project.IdProject, _enrollment);
				requestProjectConsult.Show();
				Close();
			}
		}

		private void LoadInformation()
		{
			try
			{
				IList<Project> projectsAvailableForThisPracticioner = _unitOfWork.Projects.GetProjectsAvailableToRequest(_enrollment);
				foreach (Project projec in projectsAvailableForThisPracticioner)
                {
					ListViewProjects.Items.Add(projec);
                }
				LabelProjects.Content = _unitOfWork.RequestProjects.GetPracticionerRequest(_enrollment);
			}
			catch (EntityException)
			{
				_unitOfWork.Dispose();
				ShowException();
			}
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
