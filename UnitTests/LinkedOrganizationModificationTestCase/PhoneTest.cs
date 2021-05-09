using System.Collections.Generic;
using System.Data.Entity;
using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.Repositories;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.DataPersistenceLayerTests;

namespace UnitTests.LinkedOrganizationModificationTestCase
{
    [TestClass]
    public class PhoneTest
    {
        private List<Phone> _data;
        private DbSet<Phone> _mockSet;
        private ProfessionalPracticesContext _mockContext;
        private PhoneRepository _repository;
        private UnitOfWork _unitOfWork;

        [TestInitialize]
        public void TestInitialize()
        {
            _data = new List<Phone>
            {
                new Phone
                {
                    IdPhoneNumber = 1,
                    IdLinkedOrganization = 1,
                    Extension = "521",
                    PhoneNumber = "2281244285"
                },
                new Phone
                {
                    IdPhoneNumber = 2,
                    IdLinkedOrganization = 1,
                    Extension = "521",
                    PhoneNumber = "2281172455"
                },
            };
            _mockSet = DbContextMock.GetQueryableMockDbSet(_data, x => x.IdPhoneNumber);
            _mockContext = DbContextMock.GetContext(_mockSet);
            _repository = new PhoneRepository(_mockContext);
            _unitOfWork = new UnitOfWork(_mockContext);
        }

        [TestMethod]
        public void ModifyLinkedOrganization_ErrorDueToRepeatedPhoneNumberOne()
        {
            LinkedOrganization linkedOrganization = new LinkedOrganization
            {
                IdLinkedOrganization = 1,
                Name = "Intel",
                DirectUsers = "Comunidad estudiantil",
                IndirectUsers = "Comunidad tecnológica",
                Email = "intel@gmail.com",
                PhoneNumbers = new List<Phone>
                    {
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "521",
                            PhoneNumber = "2281244230"
                        },
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "521",
                            PhoneNumber = "2281115489"
                        },
                    },
                Address = "Enrique Segoviano",
                IdCity = 1,
                IdState = 1,
                IdSector = 1,
                LinkedOrganizationStatus = LinkedOrganizationStatus.ACTIVE,
            };
            linkedOrganization.PhoneNumbers[0].PhoneNumber = "2281244285";
            linkedOrganization.PhoneNumbers[1].PhoneNumber = "2281244908";
            bool phoneIsAlreadyRegistered =
                _unitOfWork.Phones.PhoneIsAlreadyRegistered(linkedOrganization);
            Assert.IsFalse(phoneIsAlreadyRegistered);
        }
    }
}
