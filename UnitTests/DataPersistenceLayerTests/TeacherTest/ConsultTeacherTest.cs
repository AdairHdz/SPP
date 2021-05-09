using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;

namespace UnitTests.DataPersistenceLayerTests.TeacherTest
{
    [TestClass]
    public class ConsultTeacherTest
    {
        private UnitOfWork _unitOfWork;
        private List<Teacher> _data;

        [TestInitialize]
        public void TestInitialize()
        {
            _data = new List<Teacher>
            {
                new Teacher
                {
                    StaffNumber ="124987",
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
                }
            };
            DbSet<Teacher> _mockSet = DbContextMock.GetQueryableMockDbSet(_data, x => x.StaffNumber);
            ProfessionalPracticesContext _mockContext = DbContextMock.GetContext(_mockSet);
            _unitOfWork = new UnitOfWork(_mockContext);
        }

        [TestMethod]
        public void ConsultTeacher()
        {
            IEnumerable<Teacher> teachers = _unitOfWork.Teachers.GetAll();
            Assert.IsNotNull(teachers);
        }

        [TestMethod]
        public void ConsultTeacherActive()
        {
            IEnumerable<Teacher> teachers = _unitOfWork.Teachers.Find(Teacher => Teacher.User.UserStatus == UserStatus.ACTIVE);
            Assert.IsNotNull(teachers);
        }

        [TestMethod]
        public void ConsultTeacherInactive()
        {
            IEnumerable<Teacher> teachers = _unitOfWork.Teachers.Find(Teacher => Teacher.User.UserStatus == UserStatus.INACTIVE);
            Assert.IsNotNull(teachers);
        }

        [DataRow("Octavio")]
        [DataRow("Jorge")]
        [DataRow("Jorge Octavio")]
        [TestMethod]
        public void ConsultTeacherByName(string nameSeach)
        {
            IEnumerable<Teacher> teachers = _unitOfWork.Teachers.Find(Teacher => Teacher.User.Name.ToUpperInvariant().Contains(nameSeach.ToUpperInvariant()));
            Assert.IsNotNull(teachers);
        }

        [DataRow("Hernandez")]
        [DataRow("Ocharan")]
        [DataRow("Ocharan Hernandez")]
        [TestMethod]
        public void ConsultTeacherByLastName(string lastNameSeach)
        {
            IEnumerable<Teacher> teachers = _unitOfWork.Teachers.Find(Teacher => Teacher.User.LastName.ToUpperInvariant().Contains(lastNameSeach.ToUpperInvariant()));
            Assert.IsNotNull(teachers);
        }

        [TestMethod]
        public void ConsultTeacherByEmail()
        {
            string emailSeach = "Ocharan@uv.mx";
            IEnumerable<Teacher> teachers = _unitOfWork.Teachers.Find(Teacher => Teacher.User.Email.Contains(emailSeach));
            Assert.IsNotNull(teachers);
        }

        [TestMethod]
        public void ConsultTeacherByStaffNumber()
        {
            string staffNumber = "124987";
            IEnumerable<Teacher> teachers = _unitOfWork.Teachers.Find(Teacher => Teacher.StaffNumber.Contains(staffNumber));
            Assert.IsNotNull(teachers);
        }
    }
}
