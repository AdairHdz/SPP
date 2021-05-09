using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.Repositories;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using UnitTests.DataPersistenceLayerTests;

namespace UnitTests.CoordinatorDeletionTestCase
{
    [TestClass]
    public class CoordinatorDeletionTest
    {
        private List<Coordinator> _data;
        private DbSet<Coordinator> _mockSet;
        private ProfessionalPracticesContext _mockContext;
        private CoordinatorRepository _repository;
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
            _repository = new CoordinatorRepository(_mockContext);
            _unitOfWork = new UnitOfWork(_mockContext);
        }

        [TestMethod]
        public void CoordinatorDeletedTest()
        {
            _unitOfWork.Coordinators.SetCoordinatorStatusAsInactive("XGC16215");
            Coordinator retrievedCoordinator = _unitOfWork.Coordinators.Get("XGC16215");
            Assert.AreEqual(UserStatus.INACTIVE, retrievedCoordinator.User.UserStatus);
        }

        [TestMethod]
        public void NoActiveCoordinatorTest()
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
                            UserStatus = UserStatus.INACTIVE,
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
            Coordinator retrievedCoordinator = _unitOfWork.Coordinators.GetActiveCoordinator();
            Assert.IsNull(retrievedCoordinator);
        }
    }
}
