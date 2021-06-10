using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Windows;

namespace PresentationLayer
{
	/// <summary>
	/// Lógica de interacción para ConsultProgress.xaml
	/// </summary>
	public partial class ConsultProgress : Window
	{
		private readonly string _enrollment;
		private int _hourCovered;
		public int ProfessionalPracticesState { get; set; }

		public ConsultProgress(string practicionerEnrollment)
		{
			InitializeComponent();
			_enrollment = practicionerEnrollment;
			LoadPracticionerInformation();
			LoadReportsInformation();
			CalculateProgress();
			DataContext = this;
		}

		private void BehindButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			PracticionerMenu practicionerMenu = new PracticionerMenu(_enrollment);
			practicionerMenu.Show();
			Close();
		}

		private void LoadPracticionerInformation()
		{
			Practicioner practicioner = new Practicioner();
			ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
			UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
			try
			{
				practicioner = unitOfWork.Practicioners.FindFirstOccurence(Practicioner => Practicioner.Enrollment.Equals(_enrollment));
				textBlockProjectName.Text = unitOfWork.Practicioners.GetPracticionerProject(_enrollment);
				labelEnrollment.Content = practicioner.Enrollment;
				labelEmail.Content = practicioner.User.Email;
				labelName.Content = practicioner.User.Name + " " + practicioner.User.LastName;
				labelPhone.Content = practicioner.User.PhoneNumber;
			}
			catch (EntityException)
			{
				ShowException();
			}
			finally
			{
				unitOfWork.Dispose();
			}
		}

		private void LoadReportsInformation()
		{
			ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
			UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
			try
			{
				IList<ActivityPracticioner> partialReport = unitOfWork.Practicioners.GetPracticionerPartialReports(_enrollment);
				foreach (ActivityPracticioner partial in partialReport)
				{
					ListViewPartialReport.Items.Add(partial);
				}

				IList<ActivityPracticioner> monthlyReport = unitOfWork.Practicioners.GetPracticionerMonthlyReports(_enrollment);
				foreach (ActivityPracticioner monthly in monthlyReport)
				{
					ListViewMonthlyReport.Items.Add(monthly);
				}
				_hourCovered = unitOfWork.Practicioners.GetAccumulatedHours(_enrollment);
			}
			catch (EntityException)
			{
				ShowException();
			}
			finally
			{
				unitOfWork.Dispose();
			}
		}

		private void CalculateProgress()
		{
			int quantity = _hourCovered * 100 / 480;
			labelPercentage.Content = quantity + " % " +_hourCovered+" horas cubiertas de 480 horas";
			ProfessionalPracticesState = quantity ;
		}

		private void ShowException()
        {
			MessageBox.Show("No hay conexión a la base de datos. Intente más tarde", "Consulta Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
			PracticionerMenu practicionerMenu = new PracticionerMenu(_enrollment);
			practicionerMenu.Show();
			this.Close();
		}
	}
}
