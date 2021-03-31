using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PresentationLayer.Validators;
using System.Collections.Generic;
using System.Data.Entity;
using UnitTests.DataPersistenceLayerTests;


namespace UnitTests.PracticionerModificationTestCase
{
	[TestClass]
	public class PracticionerModificationTest
	{
		private PracticionerValidator _practicionerValidator;
		private List<Practicioner> _practicioners;
		private DbSet<Practicioner> _mockSet;
		private ProfessionalPracticesContext _mockContext;
		private UnitOfWork _unitOfWork;

		[TestInitialize]
		public void TestInitialize()
		{
			_practicionerValidator = new PracticionerValidator();
			_practicioners = new List<Practicioner> {
				new Practicioner
				{
					Enrollment = "zS19012344",
					Term = "FEBRERO - JULIO 2021",
					Credits = 285,
					User = new User
					{
						IdUser = 1,
						Name = "Isamel",
						LastName = "Luna Ceballos",
						Gender = Gender.MALE,
						UserStatus = UserStatus.ACTIVE,
						Email = "zs19012344@estudiantes.uv.mx",
						AlternateEmail = "isma@hotmail.com",
						PhoneNumber = "2289123456",
						Account = new Account
						{
							Username = "zS19012344",
							Password = "Annita123@",
							FirstLogin = true
						}
					}
				}
			};

			_mockSet = DbContextMock.GetQueryableMockDbSet(_practicioners, p => p.Enrollment);
			_mockContext = DbContextMock.GetContext(_mockSet);
			_unitOfWork = DbContextMock.GetUnitOfWork(_mockContext);
		}

		[TestMethod]
		public void ValidInformationTest()
		{
			Practicioner newPracticioner = new Practicioner
			{
				Enrollment = "zS18012344",
				Term = "FEBRERO - JULIO 2021",
				Credits = 285,
				User = new User
				{
					IdUser = 1,
					Name = "Yazmin",
					LastName = "Luna",
					Gender = Gender.FEMALE,
					UserStatus = UserStatus.ACTIVE,
					Email = "zs18012344@estudiantes.uv.mx",
					AlternateEmail = "ale200200@hotmail.com",
					PhoneNumber = "2281564600",
					Account = new Account
					{
						Username = "zS18012344",
						Password = "Wigetta_200200",
						FirstLogin = true
					}
				}
			};
			var result = _practicionerValidator.TestValidate(newPracticioner);
			result.ShouldNotHaveAnyValidationErrors();
		}

		[TestMethod]
		public void RepeatedInformationTest()
		{
			Practicioner repeatedInformationPracticioner = new Practicioner
			{
				Enrollment = "zS19012344",
				Term = "FEBRERO - JULIO 2021",
				Credits = 285,
				User = new User
				{
					IdUser = 1,
					Name = "Yazmin",
					LastName = "Luna Ceballos",
					Gender = Gender.FEMALE,
					UserStatus = UserStatus.ACTIVE,
					Email = "zs19012344@estudiantes.uv.mx",
					AlternateEmail = "isma@hotmail.com",
					PhoneNumber = "2289123456",
					Account = new Account
					{
						Username = "zS19012344",
						Password = "Annita123@",
						FirstLogin = true
					}
				}
			};

			bool expected = true;
			bool actual = _unitOfWork.Practicioners.PracticionerIsAlreadyRegistered(repeatedInformationPracticioner);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void InvalidInformationTest()
		{
			Practicioner invalidInformationPracticioner = new Practicioner
			{
				Enrollment = "AnaMaria",
				Term = "",
				Credits = 6,
				User = new User
				{
					IdUser = 1,
					Name = "123",
					LastName = "@@@@@",
					Gender = Gender.FEMALE,
					UserStatus = UserStatus.ACTIVE,
					Email = "zs1801212estudiantes.uv.mx",
					AlternateEmail = "ale_200200hotmail.com",
					PhoneNumber = "666666",
					Account = new Account
					{
						Username = "AnaMaria",
						Password = "a",
						FirstLogin = true
					}
				}
			};
			var result = _practicionerValidator.TestValidate(invalidInformationPracticioner);
			result.ShouldHaveValidationErrorFor("User.Name");

		}
	}
}
