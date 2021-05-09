using System;
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
    public class LinkedOrganizationModificationTest
    {
        private List<LinkedOrganization> _data;
        private DbSet<LinkedOrganization> _mockSet;
        private ProfessionalPracticesContext _mockContext;
        private LinkedOrganizationRepository _repository;
        private UnitOfWork _unitOfWork;

        [TestInitialize]
        public void TestInitialize()
        {
            _data = new List<LinkedOrganization>
            {
                new LinkedOrganization
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
                            PhoneNumber = "2281244285"
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
                },
                 new LinkedOrganization
                {
                    IdLinkedOrganization = 2,
                    Name = "Sony",
                    DirectUsers = "Comunidad estudiantil",
                    IndirectUsers = "Comunidad tecnológica",
                    Email = "business@sony.com",
                    PhoneNumbers = new List<Phone>
                    {
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "521",
                            PhoneNumber = "2281244290"
                        },
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "521",
                            PhoneNumber = "2281240021"
                        },
                    },
                    Address = "Obrero Campesino",
                    IdCity = 1,
                    IdState = 1,
                    IdSector = 1,
                    LinkedOrganizationStatus = LinkedOrganizationStatus.ACTIVE,
                }
            };
            _mockSet = DbContextMock.GetQueryableMockDbSet(_data, x => x.IdLinkedOrganization);
            _mockContext = DbContextMock.GetContext(_mockSet);
            _repository = new LinkedOrganizationRepository(_mockContext);
            _unitOfWork = new UnitOfWork(_mockContext);
        }

        [TestMethod]
        public void ModifyLinkedOrganization_Success()
        {
            LinkedOrganization linkedOrganization = _unitOfWork.LinkedOrganizations.Get(1);
            linkedOrganization.Name = "Dell";
            _unitOfWork.Complete();
            _unitOfWork.Dispose();
            Assert.AreEqual("Dell", _data[0].Name);
        }

        [TestMethod]
        public void ModifyLinkedOrganization_ErrorDueToRepeatedName()
        {
            LinkedOrganization linkedOrganization = _unitOfWork.LinkedOrganizations.Get(1);
            linkedOrganization.Name = "Sony";
            bool isAlreadyRegistered =
                _unitOfWork.LinkedOrganizations.ThereIsAnotherLinkedOrganizationWithSameData(linkedOrganization);
            Assert.IsTrue(isAlreadyRegistered);
        }

        [TestMethod]
        public void ModifyLinkedOrganization_ErrorDueToRepeatedEmail()
        {
            LinkedOrganization linkedOrganization = _unitOfWork.LinkedOrganizations.Get(1);
            linkedOrganization.Email = "business@sony.com";
            bool isAlreadyRegistered =
                _unitOfWork.LinkedOrganizations.ThereIsAnotherLinkedOrganizationWithSameData(linkedOrganization);
            Assert.IsTrue(isAlreadyRegistered);
        }

        
    }
}
