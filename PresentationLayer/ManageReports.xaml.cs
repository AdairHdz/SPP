using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PresentationLayer
{
	/// <summary>
	/// Lógica de interacción para ManageReports.xaml
	/// </summary>
	public partial class ManageReports : Window
	{
		private readonly string _staffNumber;
		public ManageReports(string staffNumber)
		{
			InitializeComponent();
			_staffNumber = staffNumber;
			LoadInformationGroups();
		}

		private void CancelButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			BackToMenu();
		}

		private void AddButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			ManageReportsAddActivity manageReportsAddActivity = new ManageReportsAddActivity(_staffNumber);
			manageReportsAddActivity.Show();
			ListViewActivities.Items.Clear();
		}

		private void ModifyButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			Group group = ((Group)ListViewGroups.SelectedItem);
			if (group != null)
			{
				ManageReportsModifyActivity modifyActivity = new ManageReportsModifyActivity(_staffNumber, group.IdGroup);
				modifyActivity.Show();
				ListViewActivities.Items.Clear();
			}
		}

		private void SelectionGroup(object sender, RoutedEventArgs routedEventArgs)
		{
			Group group = ((Group)ListViewGroups.SelectedItem);
			if (group != null)
			{
				ButtonAddActivity.IsEnabled = true;
			}
		}

		private void SelectionGroupToSearchActivity(object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{
			Group group = ((Group)ListViewGroups.SelectedItem);
			if (group != null)
			{
				ListViewActivities.Items.Clear();
				ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
				UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
				try
				{
					IList<Activity> activities = unitOfWork.Groups.GetActivitiesForThisGroup(group.IdGroup);
					if (activities.Count != 0)
					{
						foreach (Activity activity in activities)
                        {
							ListViewActivities.Items.Add(activity);
                        }
					}
					else
					{
						MessageBox.Show("Este grupo aún no tiene actividades registradas");
					}
				}
				catch (EntityException)
				{
					MessageBox.Show("No hay conexión con la base de datos. Intente más tarde");
					BackToMenu();
				}
				finally
				{
					unitOfWork.Dispose();
				}
			}
		}

		private void ModifyActivity(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
		{
			Activity activity = ((Activity)ListViewActivities.SelectedItem);
			if (activity != null)
			{
				ButtonModifyActivity.IsEnabled = true;
			}
		}

		private void BackToMenu()
        {
			TeacherMenu teacherMenu = new TeacherMenu(_staffNumber);
			teacherMenu.Show();
			this.Close();
		}

		private void LoadInformationGroups()
        {
			ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
			UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
			try
			{
				IList<Group> groups = unitOfWork.Groups.GetActiveGroupsForThisTeacher(_staffNumber);
				foreach (Group groupFound in groups)
                {
					ListViewGroups.Items.Add(groupFound);
                }
			}
			catch (EntityException)
			{
				MessageBox.Show("No hay conexión con la base de datos. Intente más tarde");
				BackToMenu();
			}
			finally
			{
				unitOfWork.Dispose();
			}
		}
	}
}
