using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;

namespace UnitTests.DataPersistenceLayerTests.ProjectTest
{
    [TestClass]
    public class LinkedOrganizationChooseTest
    {
        private UnitOfWork _unitOfWork;
        private List<LinkedOrganization> _data;

        [TestInitialize]
        public void TestInitialize()
        {
            _data = new List<LinkedOrganization>
            {
                new LinkedOrganization
                {
                    Name = "Dirección de Desarrollo Informático de Apoyo Académico",
                    DirectUsers = "Comunidad Academica",
                    IndirectUsers = "Comunidad universitaria y población en general",
                    Email = "acolunga@uv.mx",
                    IdCity = 1,
                    IdSector = 1,
                    IdState = 1,
                    Address = "Circuito Aguirre Beltrán S/N",
                    LinkedOrganizationStatus = LinkedOrganizationStatus.ACTIVE
                }
            };
            DbSet<LinkedOrganization> _mockSet = DbContextMock.GetQueryableMockDbSet(_data, x => x.Email);
            ProfessionalPracticesContext _mockContext = DbContextMock.GetContext(_mockSet);
            _unitOfWork = new UnitOfWork(_mockContext);
        }

        [TestMethod]
        public void ConsultLinkedOrganization()
        {
            IEnumerable<LinkedOrganization> linkedOrganizations = _unitOfWork.LinkedOrganizations.Find(LinkedOrganization => LinkedOrganization.LinkedOrganizationStatus == LinkedOrganizationStatus.ACTIVE);
            Assert.IsNotNull(linkedOrganizations);
        }

        [DataRow("Dirección")]
        [DataRow("Desarrollo")]
        [DataRow("Dirección de Desarrollo Informático de Apoyo Académico")]
        [TestMethod]
        public void ConsultLinkedOrganizationByName(string nameSearch)
        {
            IEnumerable<LinkedOrganization> linkedOrganizations = _unitOfWork.LinkedOrganizations.Find(LinkedOrganization => LinkedOrganization.Name.ToUpperInvariant().Contains(nameSearch.ToUpperInvariant()) && LinkedOrganization.LinkedOrganizationStatus == LinkedOrganizationStatus.ACTIVE);
            Assert.IsNotNull(linkedOrganizations);
        }

        [DataRow("X")]
        [DataRow("Xalapa")]
        [TestMethod]
        public void ConsultLinkedOrganizationByCity(string citySearch)
        {
            IEnumerable<LinkedOrganization> linkedOrganizations = _unitOfWork.LinkedOrganizations.Find(LinkedOrganization => LinkedOrganization.City.NameCity.ToUpperInvariant().Contains(citySearch.ToUpperInvariant()) && LinkedOrganization.LinkedOrganizationStatus == LinkedOrganizationStatus.ACTIVE);
            Assert.IsNotNull(linkedOrganizations);
        }

        [DataRow("V")]
        [DataRow("Veracruz")]
        [TestMethod]
        public void ConsultLinkedOrganizationByState(string stateSearch)
        {
            IEnumerable<LinkedOrganization> linkedOrganizations = _unitOfWork.LinkedOrganizations.Find(LinkedOrganization => LinkedOrganization.State.NameState.ToUpperInvariant().Contains(stateSearch.ToUpperInvariant()) && LinkedOrganization.LinkedOrganizationStatus == LinkedOrganizationStatus.ACTIVE);
            Assert.IsNotNull(linkedOrganizations);
        }

        [DataRow("E")]
        [DataRow("Educativo")]
        [TestMethod]
        public void ConsultLinkedOrganizationBySector(string sectorSearch)
        {
            IEnumerable<LinkedOrganization> linkedOrganizations = _unitOfWork.LinkedOrganizations.Find(LinkedOrganization => LinkedOrganization.Sector.NameSector.ToUpperInvariant().Contains(sectorSearch.ToUpperInvariant()) && LinkedOrganization.LinkedOrganizationStatus == LinkedOrganizationStatus.ACTIVE);
            Assert.IsNotNull(linkedOrganizations);
        }

        [TestMethod]
        public void ConsultLinkedOrganizationByEmail()
        {
            string emailSearch = "acolunga@uv.mx";
            IEnumerable<LinkedOrganization> linkedOrganizations = _unitOfWork.LinkedOrganizations.Find(LinkedOrganization => LinkedOrganization.Email.Equals(emailSearch) && LinkedOrganization.LinkedOrganizationStatus == LinkedOrganizationStatus.ACTIVE);
            Assert.IsNotNull(linkedOrganizations);
        }
    }
}
