using System.Windows;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using System.Data.Entity.Core;

namespace PresentationLayer
{
	/// <summary>
	/// Lógica de interacción para PracticionerDeletion.xaml
	/// </summary>
	public partial class PracticionerDeletion : Window
	{
		private Practicioner practicioner = new Practicioner();
		public PracticionerDeletion()
		{
			InitializeComponent();
		}

		public void InitializeDataPracticioner(Practicioner practicionerSent)
		{
			practicioner = practicionerSent;
			LabelEnrollment.Content = practicionerSent.Enrollment;
			LabelName.Content = practicionerSent.User.Name;
			LabelLastName.Content = practicionerSent.User.LastName;
			LabelEmail.Content = practicionerSent.User.Email;
			LabelAlternateEmail.Content = practicionerSent.User.AlternateEmail;
			LabelTerm.Content = practicionerSent.Term;
			LabelCredits.Content = practicionerSent.Credits;
			LabelPhone.Content = practicionerSent.User.PhoneNumber;
			PrintPracticionerGender();
		}

		private void PrintPracticionerGender()
		{
			if (practicioner.User.Gender == Gender.MALE)
			{
				LabelGender.Content = "Masculino";
			}
			else
			{
				LabelGender.Content = "Femenino";
			}
		}
		private void CancelButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			PracticionerConsult practicionerConsult = new PracticionerConsult();
			practicionerConsult.Show();
			Close();
		}

		private void DeleteButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea eliminar el practicante?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (messageBoxResult == MessageBoxResult.Yes)
			{
				ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
				UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
				try
				{
					unitOfWork.Practicioners.SetPracticionerStatusAsInactive(practicioner.Enrollment);
					unitOfWork.Complete();
					MessageBox.Show("Practicante eliminado exitosamente");
				}
				catch (EntityException)
				{
					MessageBox.Show("No hay conexión a la base de datos. Por favor intente más tarde");
				}
				finally
				{
					unitOfWork.Dispose();
					PracticionerConsult practicionerConsult = new PracticionerConsult();
					practicionerConsult.Show();
					this.Close();
				}
			}
		}
	}

}
