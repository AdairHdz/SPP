using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;


namespace UnitTests.DataPersistenceLayerTests.ResponsibleProjectTest
{
    [TestClass]
    public class ResponsibleProjectTest
    {
        private UnitOfWork _unitOfWork;

        [TestInitialize]
        public void TestInitialize()
        {
            List<ResponsibleProject> _data = new List<ResponsibleProject>
            {
                new ResponsibleProject
                {
                    Name = "Gustavo Antonio",
                    LastName = "Ruiz Zapata",
                    EmailAddress = "ruizZapata@uv.mx",
                    Charge = "Jefe de departamento de Tecnología Educativa"
                }
            };
            DbSet<ResponsibleProject> _mockSet = DbContextMock.GetQueryableMockDbSet(_data, x => x.EmailAddress);
            ProfessionalPracticesContext _mockContext = DbContextMock.GetContext(_mockSet);
            _unitOfWork = new UnitOfWork(_mockContext);
        }

        [TestMethod]
        public void DetermineIfResponsibleProjectAlreadyExists_Exists()
        {
            ResponsibleProject responsibleProjectWithSameEmailAddress = _unitOfWork.ResponsibleProjects.FindFirstOccurence(ResponsibleProject => ResponsibleProject.EmailAddress == "guztavo@uv.mx");

            Assert.IsNull(responsibleProjectWithSameEmailAddress);
        }

        [TestMethod]
        public void RegisterResponsibleProject_Exists()
        {
            List<ResponsibleProject> responsiblesProject = new List<ResponsibleProject>();
            DbSet<ResponsibleProject> mockSet = DbContextMock.GetQueryableMockDbSet(responsiblesProject, r => r.EmailAddress);
            ProfessionalPracticesContext mockContext = DbContextMock.GetContext(mockSet);
            UnitOfWork unitOfWork = DbContextMock.GetUnitOfWork(mockContext);
            ResponsibleProject newResponsibleProject = new ResponsibleProject
            {
                Name = "Gustavo Antonio",
                LastName = "Ruiz Zapata",
                EmailAddress = "guztavo@uv.mx",
                Charge = "Jefe de departamento de Tecnología Educativa"
            };
            unitOfWork.ResponsibleProjects.Add(newResponsibleProject);
            int expected = 1;
            int actual = responsiblesProject.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}
