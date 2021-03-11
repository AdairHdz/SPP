using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PresentationLayer.Validators;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using UnitTests.DataPersistenceLayerTests;

namespace UnitTests.CoordinatorRegistryTestCase
{
    [TestClass]
    public class InformationValidationTest
    {
        private CoordinatorValidator _coordinatorValidator;
        private List<Coordinator> _coordinators;
        private DbSet<Coordinator> _mockSet;
        private ProfessionalPracticesContext _mockContext;
        private UnitOfWork _unitOfWork;

        [TestInitialize]
        public void TestInitialize()
        {
            _coordinatorValidator = new CoordinatorValidator();
            _coordinators = new List<Coordinator>
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

            _mockSet = DbContextMock.GetQueryableMockDbSet(_coordinators, c => c.StaffNumber);
            _mockContext =
                DbContextMock.GetContext(_mockSet);
            _unitOfWork = DbContextMock.GetUnitOfWork(_mockContext);
        }

        [TestMethod]
        public void ValidInformationTest()
        {
            Coordinator newlyCreatedCoordinator = new Coordinator
            {
                StaffNumber = "XGC16190",
                RegistrationDate = DateTime.Now,
                DischargeDate = null,
                User = new User
                {
                    IdUser = 1,
                    Name = "Yair",
                    LastName = "Dominguez Lopez",
                    Gender = Gender.MALE,
                    UserStatus = UserStatus.ACTIVE,
                    Email = "yair@hotmail.com",
                    AlternateEmail = "",
                    PhoneNumber = "2298046193",
                    Account = new Account
                    {
                        Username = "YairDomlo",
                        Password = "salf,lsflfgs",
                        FirstLogin = true
                    }
                }
            };
            var result = _coordinatorValidator.TestValidate(newlyCreatedCoordinator);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]
        public void RepeatedStaffNumberTest()
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
            
            bool expected = true;
            bool actual = _unitOfWork.Coordinators.CoordinatorIsAlreadyRegistered(newlyCreatedCoordinator);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ThereIsAnActiveCoordinatorAlreadyTest()
        {
            Coordinator newlyCreatedCoordinator = new Coordinator
            {
                StaffNumber = "XGC16152",
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
            
            Coordinator retrievedCoordinator = _unitOfWork.Coordinators.FindFirstOccurence(coordinator => coordinator.User.UserStatus == UserStatus.ACTIVE);
            Assert.IsNotNull(retrievedCoordinator);
        }

        [TestMethod]
        public void InvalidNameTest()
        {
            Coordinator newlyCreatedCoordinator = new Coordinator
            {
                StaffNumber = "XGC16152",
                RegistrationDate = DateTime.Now,
                DischargeDate = null,
                User = new User
                {
                    IdUser = 2,
                    Name = "",
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
            var result = _coordinatorValidator.TestValidate(newlyCreatedCoordinator);
            result.ShouldHaveValidationErrorFor("User.Name");

        }
    }
}
