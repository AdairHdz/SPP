using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer
{
	/// <summary>
	/// Lógica de interacción para GroupModifyList.xaml
	/// </summary>
	public partial class GroupModifyList : Window
	{
		public GroupModifyList()
		{
			InitializeComponent();
			LoadInformation();
		}

		private void BackButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
			coordinatorMenu.Show();
			this.Close();
		}

		private void ModifyButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			Group group = ((Group)ListViewGroups.SelectedItem);
			GroupModify modifyGroup = new GroupModify(group.IdGroup);
			modifyGroup.Show();
			this.Close();
		}

		private void GroupListViewSelectionChanged(object sender, SelectionChangedEventArgs selectionChanged)
		{
			Group group = ((Group)ListViewGroups.SelectedItem);
			if (group != null)
			{
				ButtonModify.IsEnabled = true;
			}
		}

		private void LoadInformation()
		{
			ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
			UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
			try
			{
				IEnumerable<Group> groups = unitOfWork.Groups.GetAll();
				foreach (Group group in groups)
				{
					ListViewGroups.Items.Add(group);
				}
	 
			}
			catch (EntityException)
			{
				MessageBox.Show("No hay conexión con la base de datos. Intente más tarde");
				CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
				coordinatorMenu.Show();
				this.Close();
			}
			finally
			{
				unitOfWork.Dispose();
			}
		}
	}
}
