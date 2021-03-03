using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;

namespace PresentationLayer
{

    public partial class RegisterResponsableProject : Window
    {
		public ResponsibleProject ResponsibleProject= new ResponsibleProject();
		public RegisterResponsableProject()
        {
            InitializeComponent();
			this.DataContext = ResponsibleProject;
		}

		private void CancelButtonClicked(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("¿Seguro desea cancelar?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
		}

		private void RegisterButtonClicked(object sender, RoutedEventArgs e)
		{
			bool isValidDataResponsibleProject = ValidateDataResponsibleProject();
			if (isValidDataResponsibleProject)
			{
				CreateResponsabileProjectFromInputData();
				bool isValidEmail = ResponsibleProjectIsAlreadyRegistered();

				if (isValidEmail)
				{
					MessageBox.Show("Existe un responsable del proyecto con el mismo correo electrónico registrado", "Dato Repetido", MessageBoxButton.OK, MessageBoxImage.Error);
				}
				else
				{
					if (RegisternewResponsibleProject())
					{
						MessageBox.Show("El responsable del proyecto se registró exitosamente", "Registro Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
						MessageBox.Show("El responsable del proyecto no pudo registrarse. Intente más tarde", "Registro Fallido", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
			}
			else
			{
				MessageBox.Show("Por favor, Ingrese datos correctos en los campos marcados en rojo", "Datos Incorrectos", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}
		
		private void CreateResponsabileProjectFromInputData()
		{
			ResponsibleProject.Charge = ChargeTextBox.Text;
			ResponsibleProject.EmailAddress = EmailTextBox.Text;
			ResponsibleProject.Name = NameTextBox.Text;
			ResponsibleProject.LastName = LastNameTextBox.Text;
			
		}

		private bool ResponsibleProjectIsAlreadyRegistered()
		{
			ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
			UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
			bool responsibleProjectIsAlreadyRegistered = unitOfWork.ResponsibleProjects.ResponsibleProjectIsAlreadyRegistered(ResponsibleProject.EmailAddress);
			unitOfWork.Dispose();
			if (responsibleProjectIsAlreadyRegistered)
			{
				return true;
			}
			return false;
		}

		private bool ValidateDataResponsibleProject()
		{
			NameTextBox.BorderBrush = Brushes.Transparent;
			LastNameTextBox.BorderBrush = Brushes.Transparent;
			ChargeTextBox.BorderBrush = Brushes.Transparent;
			EmailTextBox.BorderBrush = Brushes.Transparent;

			bool isValidName = ValidateName();
			bool isValidLastName = ValidateLastName();
			bool isValidCharge = ValidateCharge();
			bool isValidEmail = ValidateEmail();

			bool isValidDataResponsibleProject = false;
			if (isValidName && isValidLastName && isValidCharge && isValidEmail)
			{
				isValidDataResponsibleProject = true;

			}
			return isValidDataResponsibleProject;
		}

		private bool ValidateName()
		{
			bool isValidName = ValidationData.ValidateNameComplete(NameTextBox.Text);
			if (isValidName)
			{
				NameTextBox.BorderBrush = Brushes.Green;
			}
			else
			{
				NameTextBox.BorderBrush = Brushes.Red;
			}
			return isValidName;
		}

		private bool ValidateLastName()
		{
			bool isValidLastName = ValidationData.ValidateNameComplete(LastNameTextBox.Text);
			if (isValidLastName)
			{
				LastNameTextBox.BorderBrush = Brushes.Green;
			}
			else
			{
				LastNameTextBox.BorderBrush = Brushes.Red;
			}
			return isValidLastName;
		}

		private bool ValidateEmail()
		{
			bool isValidEmail = ValidationData.ValidateEmail(EmailTextBox.Text);
			if (isValidEmail)
			{
				EmailTextBox.BorderBrush = Brushes.Green;
			}
			else
			{
				EmailTextBox.BorderBrush = Brushes.Red;
			}
			return isValidEmail;
		}

		private bool ValidateCharge()
		{
			bool isValidCharge = ValidationData.ValidateCharge(ChargeTextBox.Text);
			if (isValidCharge)
			{
				ChargeTextBox.BorderBrush = Brushes.Green;
			}
			else
			{
				ChargeTextBox.BorderBrush = Brushes.Red;
			}
			return isValidCharge;
		}

		private bool RegisternewResponsibleProject()
		{
			ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
			UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
			unitOfWork.ResponsibleProjects.Add(ResponsibleProject);
			int rowsAffected = unitOfWork.Complete();
			unitOfWork.Dispose();
			return rowsAffected == 1;
		}

		private void ProhibitSpace(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Space)
				e.Handled = true;
		}
	}
}
