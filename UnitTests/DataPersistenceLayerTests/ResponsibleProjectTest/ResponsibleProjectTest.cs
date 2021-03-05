using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.Repositories;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;

namespace UnitTests.DataPersistenceLayerTests.ResponsibleProjectTest
{
    [TestClass]
    public class ResponsibleProjectTest
    {
        private List<ResponsibleProject> _data;
        private DbSet<ResponsibleProject> _mockSet;
        private ProfessionalPracticesContext _mockContext;
        private ResponsibleProjectRepository _repository;
        private UnitOfWork _unitOfWork;


        [TestInitialize]
        public void TestInitialize()
        {
            _data = new List<ResponsibleProject>
            {
                new ResponsibleProject
                {
                    Name = "Gustavo Antonio",
                    LastName = "Ruiz Zapata",
                    EmailAddress = "guruiz@uv.mx",
                    Charge = "Jefe de departamento de Tecnología Educativa"
                }
            };
            _mockSet = DbContextMock.GetQueryableMockDbSet(_data, x => x.EmailAddress);
            _mockContext = DbContextMock.GetContext(_mockSet);
            _repository = new ResponsibleProjectRepository(_mockContext);
            _unitOfWork = new UnitOfWork(_mockContext);
        }

        [TestMethod]
        public void DetermineIfResponsibleProjectAlreadyExists_Exists()
        {
            bool responsibleProjectIsAlreadyRegistered = _unitOfWork.ResponsibleProjects.ResponsibleProjectIsAlreadyRegistered("guruiz@uv.mx");

            Assert.IsTrue(responsibleProjectIsAlreadyRegistered);
        }

    }
}
