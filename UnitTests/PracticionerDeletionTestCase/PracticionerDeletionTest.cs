using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.Repositories;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using UnitTests.DataPersistenceLayerTests;

namespace UnitTests.PracticionerDeletionTestCase
{
	[TestClass]
	public class PracticionerDeletionTest
	{
		private List<Practicioner> _data;
		private DbSet<Practicioner> _mockSet;
		private ProfessionalPracticesContext _mockContext;
		private PracticionerRepository _repository;
		private UnitOfWork _unitOfWork;


		[TestInitialize]
		public void TestInitialize()
		{
			_data = new List<Practicioner>
				{
					new Practicioner
					{
						Enrollment = "zS18000000",
						Term = "FEBRERO - JULIO 2021",
						Credits = 288,
						User = new User
						{
							IdUser = 1,
							Name = "Anahi del Carmen",
							LastName = "Lune Herrera",
							Gender = Gender.FEMALE,
							UserStatus = UserStatus.ACTIVE,
							Email = "anahi@hotmail.com",
							AlternateEmail = "carmen@hotmail.com",
							PhoneNumber = "2208043366",
							Account = new Account
							{
								Username = "zS18000000",
								Password = "Wigetta432f@",
								FirstLogin = true
							}
						}
					}
				};

			_mockSet = DbContextMock.GetQueryableMockDbSet(_data, practicioner => practicioner.Enrollment);
			_mockContext = DbContextMock.GetContext(_mockSet);
			_repository = new PracticionerRepository(_mockContext);
			_unitOfWork = new UnitOfWork(_mockContext);
		}

		[TestMethod]
		public void PracticionerDeletedTest()
		{
			_unitOfWork.Practicioners.SetPracticionerStatusAsInactive("zS18000000");
			Practicioner retrievedPracticioner = _unitOfWork.Practicioners.Get("zS18000000");
			Assert.AreEqual(UserStatus.INACTIVE, retrievedPracticioner.User.UserStatus);
		}

	}
}
