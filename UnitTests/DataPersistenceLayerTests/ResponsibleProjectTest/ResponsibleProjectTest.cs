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
                    EmailAddress = "guruiz@uv.mx",
                    Charge = "Jefe de departamento de Tecnología Educativa"
                }
            };
            DbSet<ResponsibleProject>  _mockSet = DbContextMock.GetQueryableMockDbSet(_data, x => x.EmailAddress);
            ProfessionalPracticesContext _mockContext = DbContextMock.GetContext(_mockSet);
            ResponsibleProjectRepository _repository = new ResponsibleProjectRepository(_mockContext);
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
