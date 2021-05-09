using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;


namespace UnitTests.DataPersistenceLayerTests.ResponsibleProjectTest
{
    [TestClass]
    public class ResponsibleProjectTestConsult
    {
        private UnitOfWork _unitOfWork;
        private List<ResponsibleProject> _data;

        [TestInitialize]
        public void TestInitialize()
        {
            _data = new List<ResponsibleProject>
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
        public void ConsultResponsibleProject()
        {
            IEnumerable<ResponsibleProject> responsiblesProjects = _unitOfWork.ResponsibleProjects.GetAll();
            Assert.IsNotNull(responsiblesProjects);
        }

        [TestMethod]
        public void ConsultResponsibleProjectActive()
        {
            IEnumerable<ResponsibleProject> responsiblesProjects = _unitOfWork.ResponsibleProjects.Find(ResponsibleProject => ResponsibleProject.ResponsibleProjectStatus == ResponsibleProjectStatus.ACTIVE);
            Assert.IsNotNull(responsiblesProjects);
        }

        [TestMethod]
        public void ConsultResponsibleProjectInactive()
        {
            IEnumerable<ResponsibleProject> responsiblesProjects = _unitOfWork.ResponsibleProjects.Find(ResponsibleProject => ResponsibleProject.ResponsibleProjectStatus == ResponsibleProjectStatus.INACTIVE);
            Assert.IsNotNull(responsiblesProjects);
        }

        [DataRow("Gustavo")]
        [DataRow("Antonio")]
        [DataRow("Gustavo Antonio")]
        [TestMethod]
        public void ConsultResponsibleProjectByName(string nameSeach)
        {
            IEnumerable<ResponsibleProject> responsiblesProjects = _unitOfWork.ResponsibleProjects.Find(ResponsibleProject => ResponsibleProject.Name.ToUpper().Contains(nameSeach.ToUpper()));
            Assert.IsNotNull(responsiblesProjects);
        }

        [DataRow("Ruiz")]
        [DataRow("Zapata")]
        [DataRow("Ruiz Zapata")]
        [TestMethod]
        public void ConsultResponsibleProjectByLastName(string lastNameSeach)
        {
            IEnumerable<ResponsibleProject> responsiblesProjects = _unitOfWork.ResponsibleProjects.Find(ResponsibleProject => ResponsibleProject.LastName.ToUpper().Contains(lastNameSeach.ToUpper()));
            Assert.IsNotNull(responsiblesProjects);
        }

        [TestMethod]
        public void ConsultResponsibleProjectByEmail()
        {
            string emailSeach = "guruiz@uv.mx";
            IEnumerable<ResponsibleProject> responsiblesProjects = _unitOfWork.ResponsibleProjects.Find(ResponsibleProject => ResponsibleProject.EmailAddress.Equals(emailSeach));
            Assert.IsNotNull(responsiblesProjects);
        }

        [DataRow("Departamento")]
        [DataRow("Jefe")]
        [DataRow("Jefe de departamento de Tecnología Educativa")]
        [TestMethod]
        public void ConsultResponsibleProjectByCharge(string chargeSeach)
        {
            IEnumerable<ResponsibleProject> responsiblesProjects = _unitOfWork.ResponsibleProjects.Find(ResponsibleProject => ResponsibleProject.Charge.ToUpper().Contains(chargeSeach.ToUpper()));
            Assert.IsNotNull(responsiblesProjects);
        }
    }
}
