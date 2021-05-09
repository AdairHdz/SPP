using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.Repositories;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using UnitTests.DataPersistenceLayerTests;

namespace UnitTests.CoordinatorModificationTestCase
{
    [TestClass]
    public class CoordinatorModificationTest
    {
        private List<Coordinator> _data;
        private DbSet<Coordinator> _mockSet;
        private ProfessionalPracticesContext _mockContext;
        private UnitOfWork _unitOfWork;

        [TestInitialize]
        public void TestInitialize()
        {
            _data = new List<Coordinator>
            {
                new Coordinator
                {
                    StaffNumber = "XGC16215",
                    RegistrationDate = DateTime.Now,
                    DischargeDate = null,
                    User = new User
                    {
                        IdUser = 1,
                        Name = "Eduardo Aldair",
                        LastName = "Hernández Solís",
                        Gender = Gender.MALE,
                        UserStatus = UserStatus.ACTIVE,
                        Email = "eduardo@hotmail.com",
                        AlternateEmail = "aldair@hotmail.com",
                        PhoneNumber = "2298046218",
                        Account = new Account
                        {
                            Username = "AldairHS",
                            Password = "salf,lsflfgs",
                            FirstLogin = true
                        }
                    }
                }
            };

            _mockSet = DbContextMock.GetQueryableMockDbSet(_data, x => x.StaffNumber);
            _mockContext = DbContextMock.GetContext(_mockSet);
            _unitOfWork = new UnitOfWork(_mockContext);
        }

        [TestMethod]
        public void UpdateName_Success()
        {
            Coordinator coordinator = _unitOfWork.Coordinators.Get("XGC16215");
            coordinator.User.Name = "Pedro";
            _unitOfWork.Complete();
            _unitOfWork.Dispose();
            string expected = "Pedro";
            string actual = coordinator.User.Name;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateLastName_Success()
        {
            Coordinator coordinator = _unitOfWork.Coordinators.Get("XGC16215");
            string expected = "Perez Jimene";
            coordinator.User.Name = expected;
            _unitOfWork.Complete();
            _unitOfWork.Dispose();            
            string actual = coordinator.User.Name;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateGenre_Success()
        {
            Coordinator coordinator = _unitOfWork.Coordinators.Get("XGC16215");
            Gender expected = Gender.FEMALE;
            coordinator.User.Gender = expected;
            _unitOfWork.Complete();
            _unitOfWork.Dispose();
            Gender actual = coordinator.User.Gender;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateEmail_Success()
        {
            Coordinator coordinator = _unitOfWork.Coordinators.Get("XGC16215");
            string expected = "correo_prueba27@hotmail.com";
            coordinator.User.Email = expected;
            _unitOfWork.Complete();
            _unitOfWork.Dispose();
            string actual = coordinator.User.Email;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateAlternateEmail_Success()
        {
            Coordinator coordinator = _unitOfWork.Coordinators.Get("XGC16215");
            string expected = "correo_prueba27@hotmail.com";
            coordinator.User.AlternateEmail = expected;
            _unitOfWork.Complete();
            _unitOfWork.Dispose();
            string actual = coordinator.User.AlternateEmail;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdatePhone_Success()
        {
            Coordinator coordinator = _unitOfWork.Coordinators.Get("XGC16215");
            string expected = "2297894621";
            coordinator.User.PhoneNumber = expected;
            _unitOfWork.Complete();
            _unitOfWork.Dispose();
            string actual = coordinator.User.PhoneNumber;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateStatus_Success()
        {
            Coordinator coordinator = _unitOfWork.Coordinators.Get("XGC16215");
            UserStatus expected = UserStatus.INACTIVE;
            coordinator.User.UserStatus = expected;
            _unitOfWork.Complete();
            _unitOfWork.Dispose();
            UserStatus actual = coordinator.User.UserStatus;
            Assert.AreEqual(expected, actual);
        }
    }
}
