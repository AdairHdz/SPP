using DataPersistenceLayer.Entities;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PresentationLayer.Validators;

namespace UnitTests.ValidatorsTests
{
    [TestClass]
    public class ResponsibleProjectValidatorTest
    {
        private ResponsibleProjectValidator _responsibleValidator;

        [TestInitialize]
        public void TestInitialize()
        {
            _responsibleValidator = new ResponsibleProjectValidator();
        }

        [TestMethod]
        public void ValidateResponsibleProjectData_Success()
        {
            ResponsibleProject responsibleProject = new ResponsibleProject
            {
                Name = "Gustavo Antonio",
                LastName = "Ruiz Zapata",
                EmailAddress = "guruiz@uv.mx",
                Charge = "Jefe de departamento de Tecnología Educativa"
            };

            var result = _responsibleValidator.TestValidate(responsibleProject);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [DataRow("")]
        [DataRow("                               ")]
        [DataRow("&&&& 325435435")]
        [DataRow("Gustavo Antonio Luis Alberto Francisco de Jesus Mariano")]
        [DataRow("Gustavo Antonio Luis Alberto Francisco de Jesus Mariano Julio Franco")]
        [TestMethod]
        public void ValidateResponsibleProjectData_Error_InvalidName(string name)
        {
            ResponsibleProject responsibleProject = new ResponsibleProject
            {
                Name = name,
                LastName = "Ruiz Zapata",
                EmailAddress = "guruiz@uv.mx",
                Charge = "Jefe de departamento de Tecnología Educativa"
            };

            var result = _responsibleValidator.TestValidate(responsibleProject);
            result.ShouldHaveValidationErrorFor(validatedResponsibleProject => validatedResponsibleProject.Name);
        }

        [DataRow("")]
        [DataRow("                                ")]
        [DataRow("&&&   343435")]
        [DataRow("Ruiz Zapata Herrera López Ortiz Ruiz Benítez Luna Toy Díaz Ferrer Juarez")]
        [DataRow("Ruiz Zapata Herrera López Ortiz Ruiz Benítez Luna Toy Díaz")]
        [TestMethod]
        public void ValidateResponsibleProjectData_Error_InvalidLastName(string lastName)
        {
            ResponsibleProject responsibleProject = new ResponsibleProject
            {
                Name = "Gustavo Antonio",
                LastName = lastName,
                EmailAddress = "guruiz@uv.mx",
                Charge = "Jefe de departamento de Tecnología Educativa"
            };

            var result = _responsibleValidator.TestValidate(responsibleProject);
            result.ShouldHaveValidationErrorFor(validatedResponsibleProject => validatedResponsibleProject.LastName);
        }

        [DataRow("")]
        [DataRow("                                ")]
        [DataRow("&&&   343435")]
        [DataRow("martha_15_7gmail.com")]
        [DataRow("martha_5_7@.com")]
        [DataRow("martha_5_7@gmail")]
        [DataRow("martha_5_7@gmailcom")]
        [TestMethod]
        public void ValidateResponsibleProjectData_Error_InvalidEmail(string email)
        {
            ResponsibleProject responsibleProject = new ResponsibleProject
            {
                Name = "Gustavo Antonio",
                LastName = "Ruiz Zapata",
                EmailAddress = email,
                Charge = "Jefe de departamento de Tecnología Educativa"
            };

            var result = _responsibleValidator.TestValidate(responsibleProject);
            result.ShouldHaveValidationErrorFor(validatedResponsibleProject => validatedResponsibleProject.EmailAddress);
        }

        [DataRow("")]
        [DataRow("                                ")]
        [DataRow("&&&   343435")]
        [DataRow("Jefe de departamento de Tecnología Educativa de la Universidad Veracruzana")]
        [DataRow("Encargado de Finanzas dl departamento de Tecnología Educativa")]
        [TestMethod]
        public void ValidateResponsibleProjectData_Error_InvalidCharge(string charge)
        {
            ResponsibleProject responsibleProject = new ResponsibleProject
            {
                Name = "Gustavo Antonio",
                LastName = "Ruiz Zapata",
                EmailAddress = "guruiz@uv.mx",
                Charge = charge
            };

            var result = _responsibleValidator.TestValidate(responsibleProject);
            result.ShouldHaveValidationErrorFor(validatedResponsibleProject => validatedResponsibleProject.Charge);
        }
    }
}
