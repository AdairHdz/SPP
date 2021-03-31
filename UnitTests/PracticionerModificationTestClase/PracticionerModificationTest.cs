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

			_mockSet = DbContextMock.GetQueryableMockDbSet(_practicioners, p => p.Enrollment);
			_mockContext = DbContextMock.GetContext(_mockSet);
			_unitOfWork = DbContextMock.GetUnitOfWork(_mockContext);
		}

		[TestMethod]
		public void ValidInformationTest()
		{
			Practicioner newPracticioner = new Practicioner
			{
				Enrollment = "zS19000000",
				Term = "FEBRERO - JULIO 2021",
				Credits = 288,
				User = new User
				{
					IdUser = 1,
					Name = "Angel Mauricio",
					LastName = "Lopez Hernandez",
					Gender = Gender.MALE,
					UserStatus = UserStatus.ACTIVE,
					Email = "angel@hotmail.com",
					AlternateEmail = "mau@hotmail.com",
					PhoneNumber = "2298043366",
					Account = new Account
					{
						Username = "zS19000000",
						Password = "WiLeontta432f@",
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
			Practicioner repeatedEnrollmentPracticioner = new Practicioner
			{
				Enrollment = "zS18000000",
				Term = "FEBRERO - JULIO 2021",
				Credits = 298,
				User = new User
				{
					IdUser = 1,
					Name = "Xiang",
					LastName = "Ling",
					Gender = Gender.FEMALE,
					UserStatus = UserStatus.ACTIVE,
					Email = "anahi@hotmail.com",
					AlternateEmail = "carmen@hotmail.com",
					PhoneNumber = "2208043366",
					Account = new Account
					{
						Username = "zS18000000",
						Password = "gouba432f@",
						FirstLogin = true
					}
				}
			};

			bool expected = true;
			bool actual = _unitOfWork.Practicioners.PracticionerIsAlreadyRegistered(repeatedEnrollmentPracticioner);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void InvalidInformationTest()
		{
			Practicioner invalidInformationPracticioner = new Practicioner
			{
				Enrollment = "",
				Term = "",
				Credits = 298,
				User = new User
				{
					IdUser = 1,
					Name = "",
					LastName = "",
					Gender = Gender.FEMALE,
					UserStatus = UserStatus.ACTIVE,
					Email = "ganyuhotmail.com",
					AlternateEmail = "ganyuLzhotmail.com",
					PhoneNumber = "",
					Account = new Account
					{
						Username = "",
						Password = "",
						FirstLogin = true
					}
				}
			};
			var result = _practicionerValidator.TestValidate(invalidInformationPracticioner);
			result.ShouldHaveValidationErrorFor("User.Name");

		}
	}
}