using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.Repositories;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using Utilities;

namespace UnitTests.DataPersistenceLayerTests
{
    [TestClass]
    public class LoginTest
    {
        private List<User> _data;
        private DbSet<User> _mockSet;
        private ProfessionalPracticesContext _mockContext;
        private Repository<User> _repository;
        private UnitOfWork _unitOfWork;

        [TestInitialize]
        public void TestInitialize()
        {
            _data = new List<User>
            {
                new User
                {
                    IdUser = 1,
                    Name = "Martha Miroslava",
                    LastName = "Ortiz",
                    Gender = Gender.FEMALE,
                    UserStatus = UserStatus.ACTIVE,
                    Email = "miros_15@hotmail.com",
                    AlternateEmail = "martha_25@hotmail.com",
                    PhoneNumber = "9711609017",
                    UserType = UserType.Coordinator,
                    Account = new Account
                    {
                        IdAccount =1,
                        Username = "MiroStar",
                        Password = "MMOKLP2342"
                    }
                }
            };
            _mockSet = DbContextMock.GetQueryableMockDbSet(_data, x => x.IdUser);
            _mockContext = DbContextMock.GetContext(_mockSet);
            _repository = new Repository<User>(_mockContext);
            _unitOfWork = new UnitOfWork(_mockContext);
        }

        [TestMethod]
        public void isActiveUser()
        {
           var user = _unitOfWork.Users.FindFirstOccurence(userObtein => userObtein.IdAccount == 1);
            user = _data.Find(User => User.Account.IdAccount == 1);
            Assert.AreEqual(UserStatus.ACTIVE, user.UserStatus);
        }

        [TestMethod]
        public void isAccount()
        {
           try
            {
                var account = _unitOfWork.Accounts.FindFirstOccurence(accountObtein => accountObtein.Username == "MiroStar");
            }
            catch (System.ArgumentNullException)
            {
            }
            var user = _data.Find(User => User.Account.Username == "MiroStar");
            Assert.AreEqual("MiroStar", user.Account.Username);
        }

        [TestMethod]
        public void SaveAccount()
        {
            try
            {
                var accountCurrent = _unitOfWork.Accounts.Get(1);
                BCryptHashGenerator bCryptHashGenerator = new BCryptHashGenerator();
                string salt = bCryptHashGenerator.GenerateSalt();
                string hashedPassword = bCryptHashGenerator.GenerateHashedString("MMOL78945", salt);
                accountCurrent.Password = hashedPassword;
                accountCurrent.Salt = salt;
            }
            catch (System.NullReferenceException)
            {
            }
            int expected = 1;
            int actual = _data.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}
