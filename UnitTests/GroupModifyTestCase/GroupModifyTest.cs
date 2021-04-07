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


namespace UnitTests.GroupModifyTestCase
{
	[TestClass]
	public class GroupModifyTest
	{
		private GroupValidator _groupValidator;
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
				IdGroup = 1,
				Nrc = "12345",
				GroupStatus = GroupStatus.ACTIVE,
				Term = "FEBRERO-JUNIO 2021",
				StaffNumber = "65245"
			};
			Assert.IsTrue(_unitOfWork.Groups.GroupCanBeModify(newGroup));
		}

		[TestMethod]
		public void RepeatedInformationTest()
		{
			Group newGroupExist = new Group
			{
				IdGroup = 3,
				Nrc = "12341",
				GroupStatus = GroupStatus.ACTIVE,
				Term = "FEBRERO-JUNIO 2021",
				StaffNumber = "65245"
			};
			Assert.IsFalse(_unitOfWork.Groups.GroupCanBeModify(newGroupExist));
		}

		[TestMethod]
		public void InvalidInformationTest()
		{
			Group newGroup = new Group
			{
				IdGroup = 1,
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