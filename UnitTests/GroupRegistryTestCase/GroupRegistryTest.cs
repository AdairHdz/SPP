using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PresentationLayer.Validators;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using UnitTests.DataPersistenceLayerTests;


namespace UnitTests.GroupRegistryTestCase
{
	[TestClass]
	public class GroupRegistryTest
	{
		private GroupValidator _groupValidator;
		private List<Practicioner> _practicioners;
		private DbSet<Practicioner> _mockSetPracticioner;
		private List<Group> _group;
		private DbSet<Group> _mockSetGroup;
		private List<Teacher> _teacher;
		private DbSet<Teacher> _mockSetTeacher;
		private ProfessionalPracticesContext _mockContext;
		private UnitOfWork _unitOfWork;

		[TestInitialize]
		public void TestInitialize()
		{
			_groupValidator = new GroupValidator();
			_practicioners = new List<Practicioner> {
				new Practicioner
				{
					Enrollment = "zS18012124",
					Term = "FEBRERO - JULIO 2021",
					Credits = 288,
					User = new User
					{
						IdUser = 1,
						Name = "Pedro",
						LastName = "Lopez",
						Gender = Gender.MALE,
						UserStatus = UserStatus.ACTIVE,
						Email = "emailexample@hotmail.com",
						AlternateEmail = "example@hotmail.com",
						PhoneNumber = "2281456785",
						Account = new Account
						{
							Username = "zS18012124",
							Password = "1234",
							FirstLogin = true
						}
					}
				},

				new Practicioner
				{
					Enrollment = "zS18012125",
					Term = "FEBRERO - JULIO 2021",
					Credits = 288,
					User = new User
					{
						IdUser = 2,
						Name = "Priscila",
						LastName = "Luna",
						Gender = Gender.FEMALE,
						UserStatus = UserStatus.ACTIVE,
						Email = "emailexample@hotmail.com",
						AlternateEmail = "example@hotmail.com",
						PhoneNumber = "2281456784",
						Account = new Account
						{
							Username = "zS18012125",
							Password = "1234",
							FirstLogin = true
						}
					}
				},

				new Practicioner
				{
					Enrollment = "zS18012126",
					Term = "FEBRERO - JULIO 2021",
					Credits = 288,
					User = new User
					{
						IdUser = 3,
						Name = "Pablo",
						LastName = "Lara",
						Gender = Gender.MALE,
						UserStatus = UserStatus.ACTIVE,
						Email = "emailexample@hotmail.com",
						AlternateEmail = "example@hotmail.com",
						PhoneNumber = "2281456783",
						Account = new Account
						{
							Username = "zS18012126",
							Password = "1234",
							FirstLogin = true
						}
					}
				},

				new Practicioner
				{
					Enrollment = "zS18012127",
					Term = "FEBRERO - JULIO 2021",
					Credits = 288,
					User = new User
					{
						IdUser = 4,
						Name = "Patricio",
						LastName = "Lourdes",
						Gender = Gender.MALE,
						UserStatus = UserStatus.ACTIVE,
						Email = "emailexample@hotmail.com",
						AlternateEmail = "example@hotmail.com",
						PhoneNumber = "2281456782",
						Account = new Account
						{
							Username = "zS18012127",
							Password = "1234",
							FirstLogin = true
						}
					}
				},

				new Practicioner
				{
					Enrollment = "zS18012128",
					Term = "FEBRERO - JULIO 2021",
					Credits = 288,
					User = new User
					{
						IdUser = 5,
						Name = "Patricia",
						LastName = "Lozcano",
						Gender = Gender.FEMALE,
						UserStatus = UserStatus.ACTIVE,
						Email = "emailexample@hotmail.com",
						AlternateEmail = "example@hotmail.com",
						PhoneNumber = "2281456781",
						Account = new Account
						{
							Username = "zS18012128",
							Password = "1234",
							FirstLogin = true
						}
					}
				}
			};

			_teacher = new List<Teacher>
			{
				new Teacher
				{
					StaffNumber = "65245",
					RegistrationDate = DateTime.Now,
					DischargeDate = null,
					User = new User
					{
						IdUser = 6,
						Name = "Tamara",
						LastName = "Lopez",
						Gender = Gender.FEMALE,
						UserStatus = UserStatus.ACTIVE,
						Email = "emailexample@hotmail.com",
						AlternateEmail = "example@hotmail.com",
						PhoneNumber = "2281456789",
						Account = new Account
						{
							Username = "TeacherTamara",
							Password = "1234",
							FirstLogin = true
						}
					}
				}
			};

			_group = new List<Group>
			{
				new Group
				{
					IdGroup = 1,
					Nrc = "12341",
					GroupStatus = GroupStatus.ACTIVE,
					Term ="FEBRERO-JUNIO 2021",
					StaffNumber = "65245"
				}
			};

			_mockSetPracticioner = DbContextMock.GetQueryableMockDbSet(_practicioners, p => p.Enrollment);
			_mockContext = DbContextMock.GetContext(_mockSetPracticioner);
			_mockSetTeacher = DbContextMock.GetQueryableMockDbSet(_teacher, t => t.StaffNumber);
			_mockContext = DbContextMock.GetContext(_mockSetTeacher);
			_mockSetGroup = DbContextMock.GetQueryableMockDbSet(_group, g => g.IdGroup);
			_mockContext = DbContextMock.GetContext(_mockSetGroup);

			_unitOfWork = DbContextMock.GetUnitOfWork(_mockContext);
		}

		[TestMethod]
		public void ValidInformationTestGroup()
		{
			Group newGroup = new Group
			{
				IdGroup = 2,
				Nrc = "12345",
				GroupStatus = GroupStatus.ACTIVE,
				Term = "FEBRERO-JUNIO 2021",
				StaffNumber = "65245"
			};
			_unitOfWork.Groups.Add(newGroup);
			int expected = 2;
			int actual = _group.Count;
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void RepeatedInformationTest()
		{
			Group newGroupExist =  new Group
			{
				IdGroup = 3,
				Nrc = "12341",
				GroupStatus = GroupStatus.ACTIVE,
				Term = "FEBRERO-JUNIO 2021",
				StaffNumber = "65245"
			};

			bool expected = true;
			bool actual = _unitOfWork.Groups.GroupIsAlreadyRegistered(newGroupExist);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void InvalidInformationTest()
		{
			Group newGroup = new Group
			{
				IdGroup = 0,
				Nrc = null,
				GroupStatus = GroupStatus.ACTIVE,
				Term = null,
				StaffNumber = null
			};
			var result = _groupValidator.TestValidate(newGroup);
			result.ShouldHaveValidationErrorFor("Nrc");

		}
	}
}
