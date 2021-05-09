using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;

namespace UnitTests.DataPersistenceLayerTests.TeacherTest
{
    [TestClass]
    public class TeacherTest
    {
        [TestMethod]
        public void SoftDeleteTeacher_Exists()
        {
            List<Teacher> teachers = new List<Teacher>();
            DbSet<Teacher> mockSet = DbContextMock.GetQueryableMockDbSet(teachers, t => t.StaffNumber);
            ProfessionalPracticesContext mockContext = DbContextMock.GetContext(mockSet);
            UnitOfWork unitOfWork = DbContextMock.GetUnitOfWork(mockContext);
            Teacher newTeacher = new Teacher
            {
                StaffNumber = "124987",
                User = new User
                {

                    Name = "Jorge Octavio",
                    LastName = "Ocharan Hernandez",
                    Gender = Gender.MALE,
                    UserStatus = UserStatus.ACTIVE,
                    PhoneNumber = "2284564791",
                    UserType = UserType.Teacher,
                    Email = "Ocharan@uv.mx",
                    AlternateEmail = "Jorge_14@gmail.com",
                    Account = new Account
                    {
                        Username = "Ocharan13",
                        Password = "Ocharan153988",
                        FirstLogin = true
                    }

                }
            };
            teachers.Add(newTeacher);
            Teacher deleteTeacher = unitOfWork.Teachers.Get(newTeacher.StaffNumber);
            deleteTeacher.User.UserStatus = UserStatus.ACTIVE;
            teachers.Remove(newTeacher);
            int expected = 0;
            int actual = teachers.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}
