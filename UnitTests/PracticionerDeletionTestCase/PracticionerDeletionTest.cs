using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
		private UnitOfWork _unitOfWork;


		[TestInitialize]
		public void TestInitialize()
		{
			_data = new List<Practicioner>
				{
					new Practicioner
					{
						Enrollment = "zS18012124",
						Term = "FEBRERO - JULIO 2021",
						Credits = 285,
						User = new User
						{
							IdUser = 1,
							Name = "Yazmin Alejandra",
							LastName = "Luna Herrera",
							Gender = Gender.FEMALE,
							UserStatus = UserStatus.ACTIVE,
							Email = "zs18012124@estudiantes.uv.mx",
							AlternateEmail = "ale_200200@hotmail.com",
							PhoneNumber = "2281564676",
							Account = new Account
							{
								Username = "zS18012124",
								Password = "Wigetta_200200",
								FirstLogin = true
							}
						}
					}
				};

			_mockSet = DbContextMock.GetQueryableMockDbSet(_data, practicioner => practicioner.Enrollment);
			_mockContext = DbContextMock.GetContext(_mockSet);
			_unitOfWork = new UnitOfWork(_mockContext);
		}

		[TestMethod]
		public void PracticionerDeletedTest()
		{
			_unitOfWork.Practicioners.SetPracticionerStatusAsInactive("zS18012124");
			Practicioner retrievedPracticioner = _unitOfWork.Practicioners.Get("zS18012124");
			Assert.AreEqual(UserStatus.INACTIVE, retrievedPracticioner.User.UserStatus);
		}

	}
}
