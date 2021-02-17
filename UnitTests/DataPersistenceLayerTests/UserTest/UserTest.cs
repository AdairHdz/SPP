using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.Repositories;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.DataPersistenceLayerTests.UserTest
{
    [TestClass]
    public class UserTest
    {
        private List<User> _data;
        private  DbSet<User> _mockSet;
        private  ProfessionalPracticesContext _mockContext;
        private  Repository<User> _repository;
        private  UnitOfWork _unitOfWork;

        [TestInitialize]
        public void TestInitialize()
        {
            _data = new List<User>()
            {
                new User()
                {
                    IdUser = 1,
                    Name = "Eduardo Aldair",
                    LastName = "Hernández Solís",
                    IdGender = 1,
                    IdStatus = 1,
                    Email = "eduardo@hotmail.com",
                    AlternateEmail = "aldair@hotmail.com",
                    PhoneNumber = "2298046218"
                }
            };
            _mockSet = DbContextMock.GetQueryableMockDbSet(_data, x => x.IdUser);
            _mockContext = DbContextMock.GetContext(_mockSet);
            _repository = new Repository<User>(_mockContext);
            _unitOfWork = new UnitOfWork(_mockContext);
        }

        [TestMethod]
        public void RegisterUser_Success()
        {
            User newUser = new User()
            {
                Name = "Irving",
                LastName = "Lozada Rodríguez",
                IdGender = 1,
                IdStatus = 1,
                Email = "irving@hotmail.com",
                PhoneNumber = "2298040941"
            };

            _unitOfWork.Users.Add(newUser);
            _unitOfWork.Complete();
            _unitOfWork.Dispose();

            int expected = 2;
            int actual = _mockSet.Count();

            Assert.AreEqual(expected, actual);            
        }

        [TestMethod]
        public void RegisterManyUsers_Success()
        {
            List<User> users = new List<User>()
            {
                new User(){
                    Name = "Yair",
                    LastName = "Domínguez López",
                    IdGender = 1,
                    IdStatus = 1,
                    Email = "yair@gmail.com",
                    PhoneNumber = "2298040941"
                },
                new User()
                {
                    Name = "Francisco",
                    LastName = "Portilla Texon",
                    IdGender = 1,
                    IdStatus = 1,
                    Email = "francisco@hotmail.com",
                    PhoneNumber = "2298040941"
                }
            };

            _unitOfWork.Users.AddRange(users);
            _unitOfWork.Complete();
            _unitOfWork.Dispose();

            int expected = 3;
            int actual = _data.Count();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetAllUsers_Success()
        {
            var users = _unitOfWork.Users.GetAll();           

            int expected = 1;
            int actual = users.Count();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetUser_Success()
        {            
            User user = _unitOfWork.Users.Get(1);            
            string expected = "Eduardo Aldair";
            string actual = user.Name;
            _unitOfWork.Complete();
            _unitOfWork.Dispose();
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DeleteUser_Success()
        {            
            User user = _unitOfWork.Users.Get(1);
            _unitOfWork.Users.Remove(user);
            _unitOfWork.Complete();
            _unitOfWork.Dispose();

            int expected = 0;
            int actual = _data.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FindUser_Success()
        {
            var users = _unitOfWork.Users.Find(x => x.IdGender == 1);
            string expected = "Hernández Solís";
            string actual = users.ElementAt(0).LastName;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FindFirstOccurence_Success()
        {
            User user = _unitOfWork.Users.FindFirstOccurence(x => x.Name == "Eduardo Aldair");
            string expected = "eduardo@hotmail.com";
            string actual = user.Email;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveRange_Success()
        {
            var users = _unitOfWork.Users.GetAll();
            _unitOfWork.Users.RemoveRange(users);
            int expected = 0;
            int actual = _data.Count;

            Assert.AreEqual(expected, actual);
        }
    }
}
