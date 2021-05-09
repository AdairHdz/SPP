using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.Repositories;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.DataPersistenceLayerTests;

namespace UnitTests.TeacherRegistryTestCase
{
    [TestClass]
    public class TeacherRegistry
    {
        private DbSet<Teacher> _mockSet;
        private ProfessionalPracticesContext _mockContext;
        private UnitOfWork _mockUnitOfWork;
        private List<Teacher> _data;

        [TestInitialize]
        public void TestInitialize()
        {
            _data = new List<Teacher>
            {
              new Teacher {
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
            _mockSet = DbContextMock.GetQueryableMockDbSet(_data, t => t.StaffNumber);
            _mockContext = DbContextMock.GetContext(_mockSet);
            _mockUnitOfWork = DbContextMock.GetUnitOfWork(_mockContext);
        }

        [TestMethod]
        public void RegisterTeacher_Success()
        {
            Teacher newTeacher = new Teacher
            {
                StaffNumber = "G722D",
                RegistrationDate = DateTime.Now,
                DischargeDate = null,
                User = new User
                {
                    IdUser = 2,
                    Name = "Irving de Jesus",
                    LastName = "Lozada Rodriguez",
                    Gender = Gender.MALE,
                    UserStatus = UserStatus.ACTIVE,
                    Email = "irving@hotmail.com",
                    AlternateEmail = "jesus_rod@hotmail.com",
                    PhoneNumber = "2298040096",
                    Account = new Account
                    {
                        Username = "IrvingRod",
                        Password = "salf,lsflsfgs",
                        FirstLogin = true
                    }
                }
            };
            _mockUnitOfWork.Teachers.Add(newTeacher);
            _mockUnitOfWork.Complete();
            int expected = 2;
            int actual = _mockUnitOfWork.Teachers.GetAll().ToList().Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RegisterTeacher_ErrorDueToRepeatedStaffNumber()
        {
            Teacher newTeacher = new Teacher
            {
                StaffNumber = "XGC16215",
                RegistrationDate = DateTime.Now,
                DischargeDate = null,
                User = new User
                {
                    IdUser = 2,
                    Name = "Irving de Jesus",
                    LastName = "Lozada Rodriguez",
                    Gender = Gender.MALE,
                    UserStatus = UserStatus.ACTIVE,
                    Email = "irving@hotmail.com",
                    AlternateEmail = "jesus_rod@hotmail.com",
                    PhoneNumber = "2298040096",
                    Account = new Account
                    {
                        Username = "IrvingRod",
                        Password = "salf,lsflsfgs",
                        FirstLogin = true
                    }
                }
            };            
            bool expected = true;
            bool actual = _mockUnitOfWork.Teachers.TeacherIsAlreadyRegistered(newTeacher, false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RegisterTeacher_ErrorDueToRepeatedEmail()
        {
            Teacher newTeacher = new Teacher
            {
                StaffNumber = "G722D",
                RegistrationDate = DateTime.Now,
                DischargeDate = null,
                User = new User
                {
                    IdUser = 2,
                    Name = "Irving de Jesus",
                    LastName = "Lozada Rodriguez",
                    Gender = Gender.MALE,
                    UserStatus = UserStatus.ACTIVE,
                    Email = "eduardo@hotmail.com",
                    AlternateEmail = "jesus_rod@hotmail.com",
                    PhoneNumber = "2298040096",
                    Account = new Account
                    {
                        Username = "IrvingRod",
                        Password = "salf,lsflsfgs",
                        FirstLogin = true
                    }
                }
            };
            bool expected = true;
            bool actual = _mockUnitOfWork.Teachers.TeacherIsAlreadyRegistered(newTeacher, false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RegisterTeacher_ErrorDueToRepeatedAlternateEmail()
        {
            Teacher newTeacher = new Teacher
            {
                StaffNumber = "G722D",
                RegistrationDate = DateTime.Now,
                DischargeDate = null,
                User = new User
                {
                    IdUser = 2,
                    Name = "Irving de Jesus",
                    LastName = "Lozada Rodriguez",
                    Gender = Gender.MALE,
                    UserStatus = UserStatus.ACTIVE,
                    Email = "irving@hotmail.com",
                    AlternateEmail = "aldair@hotmail.com",
                    PhoneNumber = "2298040096",
                    Account = new Account
                    {
                        Username = "IrvingRod",
                        Password = "salf,lsflsfgs",
                        FirstLogin = true
                    }
                }
            };
            bool expected = true;
            bool actual = _mockUnitOfWork.Teachers.TeacherIsAlreadyRegistered(newTeacher, false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RegisterTeacher_ErrorDueToRepeatedPhoneNumber()
        {
            Teacher newTeacher = new Teacher
            {
                StaffNumber = "G722D",
                RegistrationDate = DateTime.Now,
                DischargeDate = null,
                User = new User
                {
                    IdUser = 2,
                    Name = "Irving de Jesus",
                    LastName = "Lozada Rodriguez",
                    Gender = Gender.MALE,
                    UserStatus = UserStatus.ACTIVE,
                    Email = "irving@hotmail.com",
                    AlternateEmail = "jesus_rod@hotmail.com",
                    PhoneNumber = "2298046218",
                    Account = new Account
                    {
                        Username = "IrvingRod",
                        Password = "salf,lsflsfgs",
                        FirstLogin = true
                    }
                }
            };
            bool expected = true;
            bool actual = _mockUnitOfWork.Teachers.TeacherIsAlreadyRegistered(newTeacher, false);
            Assert.AreEqual(expected, actual);
        }

    }
}
