using System;
using System.Collections.Generic;
using System.Linq;
using DataPersistenceLayer.Entities;
using System.Windows;
using System.Windows.Controls;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using System.Data.Entity.Core;

namespace PresentationLayer
{
	/// <summary>
	/// Lógica de interacción para ListTeacher.xaml
	/// </summary>
	public partial class TeacherConsultation : Window
	{
		private bool handle = true;
		private string textSearch;
		private string optionFilter;
		private bool isFilterWithText;
		public TeacherConsultation()
		{
			InitializeComponent();
		}

		private void FilterComboBoxDropDownClosed(object sender, EventArgs eventArgs)
		{
			if (handle)
			{
				DisableSearch();
			}
			handle = true;
		}

		private void FilterComboBoxSelectionChanged(object sender, SelectionChangedEventArgs selectionChanged)
		{
			ComboBox FilterSelectComboBox = sender as ComboBox;
			handle = !FilterSelectComboBox.IsDropDownOpen;
			DisableSearch();
		}

		private void BehindButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			ManagerMenu managerMenu = new ManagerMenu();
			managerMenu.Show();
			Close();
		}

		private void DisableSearch()
		{
			if (ComboBoxFilter.SelectedItem != null)
			{
				ButtonSearch.IsEnabled = true;
				optionFilter = ((ComboBoxItem)ComboBoxFilter.SelectedItem).Content.ToString();
				if (optionFilter.Equals("Todos") || optionFilter.Equals("Activos") || optionFilter.Equals("Inactivos"))
				{
					TextBoxSearch.IsEnabled = false;
					isFilterWithText = false;
				}
				else
				{
					TextBoxSearch.IsEnabled = true;
					isFilterWithText = true;
				}
			}
		}

		private void SearchButtonClicked(object sender, RoutedEventArgs routedEvent)
		{
			textSearch = TextBoxSearch.Text;
			optionFilter = ((ComboBoxItem)ComboBoxFilter.SelectedItem).Content.ToString();
			if (ValidateIsTextSearch())
			{
				ButtonDelete.IsEnabled = false;
				ButtonModify.IsEnabled = false;
				ListViewTeacher.Items.Clear();
				IEnumerable<Teacher> teachers = ConsultTeacher();
				if (teachers.Count() > 0)
				{
					AddTeachersInListView(teachers);
				}
				else
				{
					MessageBox.Show("No se encontraron registros", "No hay registros", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
		}

		private bool ValidateIsTextSearch()
		{
			if (String.IsNullOrWhiteSpace(textSearch) && isFilterWithText)
			{
				return false;
			}
			return true;
		}

		private void AddTeachersInListView(IEnumerable<Teacher> teachers)
		{
			foreach (Teacher teacher in teachers)
			{
				ListViewTeacher.Items.Add(teacher);
			}
		}

		private IEnumerable<Teacher> ConsultTeacher()
		{
			IEnumerable<Teacher> teachers = null;
			try
			{
				ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
				UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
				switch (optionFilter)
				{
					case "Todos":
						teachers = unitOfWork.Teachers.GetAll();
						break;
					case "Activos":
						teachers = unitOfWork.Teachers.Find(Teacher => Teacher.User.UserStatus == UserStatus.ACTIVE);
						break;
					case "Inactivos":
						teachers = unitOfWork.Teachers.Find(Teacher => Teacher.User.UserStatus == UserStatus.INACTIVE);
						break;
					case "Nombre":
						teachers = unitOfWork.Teachers.Find(Teacher => Teacher.User.Name.ToUpperInvariant().Contains(textSearch.ToUpperInvariant()));
						break;
					case "Apellido":
						teachers = unitOfWork.Teachers.Find(Teacher => Teacher.User.LastName.ToUpperInvariant().Contains(textSearch.ToUpperInvariant()));
						break;
					case "Correo":
						teachers = unitOfWork.Teachers.Find(Teacher => Teacher.User.Email.Equals(textSearch));
						break;
					case "Número de personal":
						teachers = unitOfWork.Teachers.Find(Teacher => Teacher.StaffNumber.Equals(textSearch));
						break;
					default:
						MessageBox.Show("Ingrese un filtro valido", "Filtro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
						break;
				}
			}
			catch (EntityException)
			{
				MessageBox.Show("No se pudo obtener información de la base de datos. Intente más tarde", "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			return teachers;
		}

		private void TeacherListViewSelectionChanged(object sender, SelectionChangedEventArgs selectionChanged)
		{
			Teacher teacher = ((Teacher)ListViewTeacher.SelectedItem);
			if (teacher != null)
			{
				ButtonModify.IsEnabled = true;
				if (teacher.User.UserStatus == UserStatus.ACTIVE)
				{
					ButtonDelete.IsEnabled = true;
				}
				else
				{
					ButtonDelete.IsEnabled = false;
				}
			}
		}

		private void DeleteButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			Teacher teacher = ((Teacher)ListViewTeacher.SelectedItem);
			TeacherDeletion teacherDeletion = new TeacherDeletion();
			teacherDeletion.InitializeDataTeacher(teacher);
			teacherDeletion.Show();
			Close();
		}
		private void ModifyButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			Teacher teacher = (Teacher)ListViewTeacher.SelectedItem;
			TeacherModification teacherModify = new TeacherModification(teacher.StaffNumber);
			teacherModify.Show();
			Close();
		}
	}
}
