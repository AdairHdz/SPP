using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.Repositories;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace UnitTests.DataPersistenceLayerTests.CoordinatorTest
{
    [TestClass]
    public class CoordinatorTest
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
        public void DetermineIfCoordinatorAlreadyExists_Exists()
        {
            Coordinator newlyCreatedCoordinator = new Coordinator
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
            };

            bool coordinatorIsAlreadyRegistered = _unitOfWork.Coordinators.CoordinatorIsAlreadyRegistered(newlyCreatedCoordinator, false);
                        
            Assert.IsTrue(coordinatorIsAlreadyRegistered);

        }

        [TestMethod]
        public void DetermineIfCoordinatorStaffNumberAlreadyExists_Exists()
        {

            Coordinator newlyCreatedCoordinator = new Coordinator
            {
                StaffNumber = "XGC16215",
                RegistrationDate = DateTime.Now,
                DischargeDate = null,
                User = new User
                {
                    IdUser = 2,
                    Name = "Irving",
                    LastName = "Lozada Rodríguez",
                    Gender = Gender.MALE,
                    UserStatus = UserStatus.ACTIVE,
                    Email = "irving@hotmail.com",
                    AlternateEmail = "",
                    PhoneNumber = "2298040941",
                    Account = new Account
                    {
                        Username = "Irving",
                        Password = "salf,lsflfgs",
                        FirstLogin = true
                    }
                }
            };

            bool coordinatorIsAlreadyRegistered = _unitOfWork.Coordinators.CoordinatorIsAlreadyRegistered(newlyCreatedCoordinator, false);
            Assert.IsTrue(coordinatorIsAlreadyRegistered);
        }

        [TestMethod]
        public void DetermineIfEmailAlreadyExists_Exists()
        {
            Coordinator newlyCreatedCoordinator = new Coordinator
            {
                StaffNumber = "XGC16097",
                RegistrationDate = DateTime.Now,
                DischargeDate = null,
                User = new User
                {
                    IdUser = 2,
                    Name = "Irving",
                    LastName = "Lozada Rodríguez",
                    Gender = Gender.MALE,
                    UserStatus = UserStatus.ACTIVE,
                    Email = "eduardo@hotmail.com",
                    AlternateEmail = "irving2@hotmail.com",
                    PhoneNumber = "2298040941",
                    Account = new Account
                    {
                        Username = "Irving",
                        Password = "salf,lsflfgs",
                        FirstLogin = true
                    }
                }
            };

            bool coordinatorIsAlreadyRegistered = _unitOfWork.Coordinators.CoordinatorIsAlreadyRegistered(newlyCreatedCoordinator, false);
            Assert.IsTrue(coordinatorIsAlreadyRegistered);
        }


        [TestMethod]
        public void DetermineIfAlternateEmailAlreadyExists_Exists()
        {
            Coordinator newlyCreatedCoordinator = new Coordinator
            {
                StaffNumber = "XGC16097",
                RegistrationDate = DateTime.Now,
                DischargeDate = null,
                User = new User
                {
                    IdUser = 2,
                    Name = "Irving",
                    LastName = "Lozada Rodríguez",
                    Gender = Gender.MALE,
                    UserStatus = UserStatus.ACTIVE,
                    Email = "irving2@hotmail.com",
                    AlternateEmail = "eduardo@hotmail.com",
                    PhoneNumber = "2298040941",
                    Account = new Account
                    {
                        Username = "Irving",
                        Password = "salf,lsflfgs",
                        FirstLogin = true
                    }
                }
            };

            bool coordinatorIsAlreadyRegistered = _unitOfWork.Coordinators.CoordinatorIsAlreadyRegistered(newlyCreatedCoordinator, false);
            Assert.IsTrue(coordinatorIsAlreadyRegistered);
        }

        [TestMethod]
        public void DetermineIfPhoneNumberAlreadyExists_Exists()
        {
            Coordinator newlyCreatedCoordinator = new Coordinator
            {
                StaffNumber = "XGC16097",
                RegistrationDate = DateTime.Now,
                DischargeDate = null,
                User = new User
                {
                    IdUser = 2,
                    Name = "Irving",
                    LastName = "Lozada Rodríguez",
                    Gender = Gender.MALE,
                    UserStatus = UserStatus.ACTIVE,
                    Email = "irving@hotmail.com",
                    AlternateEmail = "irving2@hotmail.com",
                    PhoneNumber = "2298046218",
                    Account = new Account
                    {
                        Username = "Irving",
                        Password = "salf,lsflfgs",
                        FirstLogin = true
                    }
                }
            };

            bool coordinatorIsAlreadyRegistered = _unitOfWork.Coordinators.CoordinatorIsAlreadyRegistered(newlyCreatedCoordinator, false);
            Assert.IsTrue(coordinatorIsAlreadyRegistered);
        }

        //

        [TestMethod]
        public void DetermineIfCoordinatorStaffNumberAlreadyExists_DoesNotExists()
        {

            Coordinator newlyCreatedCoordinator = new Coordinator
            {
                StaffNumber = "XGC16213",
                RegistrationDate = DateTime.Now,
                DischargeDate = null,
                User = new User
                {
                    IdUser = 2,
                    Name = "Irving",
                    LastName = "Lozada Rodríguez",
                    Gender = Gender.MALE,
                    UserStatus = UserStatus.ACTIVE,
                    Email = "irving@hotmail.com",
                    AlternateEmail = "",
                    PhoneNumber = "2298040941",
                    Account = new Account
                    {
                        Username = "Irving",
                        Password = "salf,lsflfgs",
                        FirstLogin = true
                    }
                }
            };

            bool coordinatorIsAlreadyRegistered = _unitOfWork.Coordinators.CoordinatorIsAlreadyRegistered(newlyCreatedCoordinator, false);
            Assert.IsFalse(coordinatorIsAlreadyRegistered);
        }

        [TestMethod]
        public void DetermineIfEmailAlreadyExists_DoesNotExists()
        {
            Coordinator newlyCreatedCoordinator = new Coordinator
            {
                StaffNumber = "XGC16097",
                RegistrationDate = DateTime.Now,
                DischargeDate = null,
                User = new User
                {
                    IdUser = 2,
                    Name = "Irving",
                    LastName = "Lozada Rodríguez",
                    Gender = Gender.MALE,
                    UserStatus = UserStatus.ACTIVE,
                    Email = "irv@hotmail.com",
                    AlternateEmail = "irving2@hotmail.com",
                    PhoneNumber = "2298040941",
                    Account = new Account
                    {
                        Username = "Irving",
                        Password = "salf,lsflfgs",
                        FirstLogin = true
                    }
                }
            };

            bool coordinatorIsAlreadyRegistered = _unitOfWork.Coordinators.CoordinatorIsAlreadyRegistered(newlyCreatedCoordinator, false);
            Assert.IsFalse(coordinatorIsAlreadyRegistered);
        }


        [TestMethod]
        public void DetermineIfAlternateEmailAlreadyExists_DoesNotExists()
        {
            Coordinator newlyCreatedCoordinator = new Coordinator
            {
                StaffNumber = "XGC16097",
                RegistrationDate = DateTime.Now,
                DischargeDate = null,
                User = new User
                {
                    IdUser = 2,
                    Name = "Irving",
                    LastName = "Lozada Rodríguez",
                    Gender = Gender.MALE,
                    UserStatus = UserStatus.ACTIVE,
                    Email = "irving2@hotmail.com",
                    AlternateEmail = "irv@hotmail.com",
                    PhoneNumber = "2298040941",
                    Account = new Account
                    {
                        Username = "Irving",
                        Password = "salf,lsflfgs",
                        FirstLogin = true
                    }
                }
            };

            bool coordinatorIsAlreadyRegistered = _unitOfWork.Coordinators.CoordinatorIsAlreadyRegistered(newlyCreatedCoordinator, false);
            Assert.IsFalse(coordinatorIsAlreadyRegistered);
        }

        [TestMethod]
        public void DetermineIfPhoneNumberAlreadyExists_DoesNotExists()
        {
            Coordinator newlyCreatedCoordinator = new Coordinator
            {
                StaffNumber = "XGC16097",
                RegistrationDate = DateTime.Now,
                DischargeDate = null,
                User = new User
                {
                    IdUser = 2,
                    Name = "Irving",
                    LastName = "Lozada Rodríguez",
                    Gender = Gender.MALE,
                    UserStatus = UserStatus.ACTIVE,
                    Email = "irving@hotmail.com",
                    AlternateEmail = "irving2@hotmail.com",
                    PhoneNumber = "2298046290",
                    Account = new Account
                    {
                        Username = "Irving",
                        Password = "salf,lsflfgs",
                        FirstLogin = true
                    }
                }
            };

            bool coordinatorIsAlreadyRegistered = _unitOfWork.Coordinators.CoordinatorIsAlreadyRegistered(newlyCreatedCoordinator, false);
            Assert.IsFalse(coordinatorIsAlreadyRegistered);
        }

    }
}
