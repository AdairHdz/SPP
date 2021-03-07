using System;
using System.Collections.Generic;
using System.Data.Entity;
using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.DataPersistenceLayerTests;

namespace UnitTests.CoordinatorRegistryTestCase
{
    [TestClass]
    public class CoordinatorRegistryTest
    {
        [TestMethod]
        public void CoordinatorRegisteredTest()
        {

            List<Coordinator> coordinators = new List<Coordinator>();
            DbSet<Coordinator> mockSet = DbContextMock.GetQueryableMockDbSet(coordinators, c => c.StaffNumber);
            ProfessionalPracticesContext mockContext =
                DbContextMock.GetContext(mockSet);
            UnitOfWork unitOfWork = DbContextMock.GetUnitOfWork(mockContext);

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

            unitOfWork.Coordinators.Add(newlyCreatedCoordinator);            

            int expected = 1;
            int actual = coordinators.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}
