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

namespace UnitTests.TeacherModificationTestCase
{
	[TestClass]
	public class TeacherModificationTest
	{
		private TeacherValidator _teacherValidator;
		private List<Teacher> _teachers;
		private DbSet<Teacher> _mockSet;
		private ProfessionalPracticesContext _mockContext;
		private UnitOfWork _unitOfWork;

		[TestInitialize]
		public void TestInitialize()
		{
			_teacherValidator = new TeacherValidator(true);
			_teachers = new List<Teacher> {
				new Teacher
				{
					StaffNumber = "54321",
					RegistrationDate = DateTime.Now,
					User = new User
					{
						IdUser = 1,
						Name = "Alejandra",
						LastName = "Luna Ceballos",
						Gender = Gender.FEMALE,
						UserStatus = UserStatus.ACTIVE,
						Email = "angelita@hotmail.com",
						AlternateEmail = "aleWige@hotmail.com",
						PhoneNumber = "2281213456"
					}
				}
			};

			_mockSet = DbContextMock.GetQueryableMockDbSet(_teachers, t => t.StaffNumber);
			_mockContext = DbContextMock.GetContext(_mockSet);
			_unitOfWork = DbContextMock.GetUnitOfWork(_mockContext);
		}

		[TestMethod]
		public void ValidInformationTeacherTest()
		{
			Teacher teacher = _unitOfWork.Teachers.Get("54321");
			teacher.User.Name = "Yazmin";
			teacher.User.LastName = "Luna Ceballos";
			teacher.User.Gender = Gender.FEMALE;
			teacher.User.UserStatus = UserStatus.ACTIVE;
			teacher.User.Email = "yazmin@hotmial.com";
			teacher.User.AlternateEmail = "ale200200@hotmail.com>";
			teacher.User.PhoneNumber = "2281564600";
			_unitOfWork.Complete();
			_unitOfWork.Dispose();
			string expected = "Yazmin";
			string actual = teacher.User.Name;
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void InvalidInformationTeacherTest()
		{
			Teacher teacher = _unitOfWork.Teachers.Get("54321");
			teacher.User.Name = "@@";
			teacher.User.LastName = "";
			teacher.User.UserStatus = UserStatus.ACTIVE;
			teacher.User.Email = "yazmin.uv.mx";
			teacher.User.AlternateEmail = "alehotmail.com>";
			teacher.User.PhoneNumber = "Alejandra";
			var result = _teacherValidator.TestValidate(teacher);
			result.ShouldHaveValidationErrorFor("User.Name");
		}

		[TestMethod]
		public void RepeatedInformationTest()
		{
			Teacher repeatedInformationTeacher = new Teacher
			{
				StaffNumber = "54321",
				RegistrationDate = DateTime.Now,
				User = new User
				{
					IdUser = 4,
					Name = "Yazmin",
					LastName = "Luna Ceballos",
					Gender = Gender.FEMALE,
					UserStatus = UserStatus.ACTIVE,
					Email = "angelita@hotmail.com",
					AlternateEmail = "aleWige@hotmail.com",
					PhoneNumber = "2281213456"
				}
			};

			bool expected = true;
			bool actual = _unitOfWork.Teachers.TeacherIsAlreadyRegistered(repeatedInformationTeacher, false);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void UpdateStatus_Success()
		{
			Teacher teacher = _unitOfWork.Teachers.Get("54321");
			UserStatus expected = UserStatus.INACTIVE;
			teacher.User.UserStatus = expected;
			_unitOfWork.Complete();
			_unitOfWork.Dispose();
			UserStatus actual = teacher.User.UserStatus;
			Assert.AreEqual(expected, actual);
		}
	}
}