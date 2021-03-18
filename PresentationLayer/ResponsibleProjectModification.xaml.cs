using System.Windows;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer;
using DataPersistenceLayer.UnitsOfWork;
using PresentationLayer.Validators;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Data.Entity.Core;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para ModifyResponsibleProject.xaml
    /// </summary>
    public partial class ResponsibleProjectModification : Window
    {
        private ResponsibleProject responsibleProject;
        public ResponsibleProjectModification()
        {
            InitializeComponent();
        }

        public void InitializeDataResponsibleProject(ResponsibleProject responsibleProjectReceived)
        {
            TextBoxName.Text = responsibleProjectReceived.Name;
            TextBoxLastName.Text = responsibleProjectReceived.LastName;
            TextBoxEmail.Text = responsibleProjectReceived.EmailAddress;
            TextBoxCharge.Text = responsibleProjectReceived.Charge;
            responsibleProject = responsibleProjectReceived;
            if (responsibleProjectReceived.ResponsibleProjectStatus == ResponsibleProjectStatus.ACTIVE)
            {
                LabelStatus.Visibility = Visibility.Hidden;
                ButtonActive.Visibility = Visibility.Hidden;
            }
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro desea cancelar?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                ResponsibleProjectConsult listResponsibleProject = new ResponsibleProjectConsult();
                listResponsibleProject.Show();
                Close();
            }
        }

        private void ActiveButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                if (ActiveResponsibleProject())
                {
                    MessageBox.Show("El responsable del proyecto se activo con éxito", "Activación Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                    ButtonActive.Visibility = Visibility.Hidden;
                    LabelStatus.Visibility = Visibility.Hidden;
                }
                else
                {
                    MessageBox.Show("El responsable del proyecto no se pudo activar. Intente más tarde", "Activación Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (EntityException)
            {
                MessageBox.Show("El responsable del proyecto no se pudo activar. Intente más tarde", "Activación Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BehindButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            ResponsibleProjectConsult listResponsibleProject = new ResponsibleProjectConsult();
            listResponsibleProject.Show();
            Close();
        }

        private void ModifyButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            CreateResponsibleProjectFromInputData();
            if (ValidateDataResponsibleProject())
            {
                try
                {
                    if (ResponsibleProjectIsAlreadyRegistered())
                    {
                        MessageBox.Show("Existe un responsable del proyecto con el mismo correo electrónico registrado", "Datos Repetidos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        if (UpdatenewResponsibleProject())
                        {
                            MessageBox.Show("El responsable del proyecto se modificó con éxito", "Modificación Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
                            ResponsibleProjectConsult listResponsibleProject = new ResponsibleProjectConsult();
                            listResponsibleProject.Show();
                            Close();
                        }
                    }
                }
                catch (EntityException)
                {
                    MessageBox.Show("El responsable del proyecto no pudo modificarse", "Modificación Fallida", MessageBoxButton.OK, MessageBoxImage.Error);
                    ResponsibleProjectConsult listResponsibleProject = new ResponsibleProjectConsult();
                    listResponsibleProject.Show();
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Por favor, Ingrese datos correctos en los campos marcados en rojo", "Datos Incorrectos", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CreateResponsibleProjectFromInputData()
        {
            responsibleProject.Charge = TextBoxCharge.Text;
            responsibleProject.EmailAddress = TextBoxEmail.Text;
            responsibleProject.Name = TextBoxName.Text;
            responsibleProject.LastName = TextBoxLastName.Text;
        }

        private bool ValidateDataResponsibleProject()
        {
            ResponsibleProjectValidator responsibleProjectValidator = new ResponsibleProjectValidator();
            ValidationResult dataValidationResult = responsibleProjectValidator.Validate(responsibleProject);
            IList<ValidationFailure> validationFailures = dataValidationResult.Errors;
            UserFeedback userFeedback = new UserFeedback(FormGrid, validationFailures);
            userFeedback.ShowFeedback();
            return dataValidationResult.IsValid;
        }

        private bool UpdatenewResponsibleProject()
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            unitOfWork.ResponsibleProjects.UpdateResponsibleProject(responsibleProject);
            int rowsAffected = unitOfWork.Complete();
            unitOfWork.Dispose();
            return rowsAffected == 1;
        }
        
        private bool ResponsibleProjectIsAlreadyRegistered()
        {
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            ResponsibleProject responsibleProjectWithSameEmailAddress = unitOfWork.ResponsibleProjects.FindFirstOccurence(ResponsibleProject => ResponsibleProject.EmailAddress == responsibleProject.EmailAddress && ResponsibleProject.IdResponsibleProject != responsibleProject.IdResponsibleProject);
            if (!object.ReferenceEquals(null, responsibleProjectWithSameEmailAddress))
            {
                return true;
            }
            return false;
        }

        private bool ActiveResponsibleProject()
        {
            responsibleProject.ResponsibleProjectStatus = ResponsibleProjectStatus.ACTIVE;
            ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
            UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
            unitOfWork.ResponsibleProjects.ActiveResponsibleProject(responsibleProject);
            int rowsAffected = unitOfWork.Complete();
            unitOfWork.Dispose();
            return rowsAffected == 1;
        }
    }
}
