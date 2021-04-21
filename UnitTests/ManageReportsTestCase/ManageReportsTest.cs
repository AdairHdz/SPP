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


namespace UnitTests.ManageReportsTestCase
{
	[TestClass]
	public class ManageReportsTest
	{
		private List<Activity> _data;
		private DbSet<Activity> _mockSet;
		private ProfessionalPracticesContext _mockContext;
		private UnitOfWork _unitOfWork;


		[TestInitialize]
		public void TestInitialize()
		{
			_data = new List<Activity>
			{
				new Activity
				{
					IdGroup = 1,
					IdActivity = 2,
					teacher = new Teacher
					{
						StaffNumber = "12345"
					}
				}
			};

			_mockSet = DbContextMock.GetQueryableMockDbSet(_data, x => x.IdActivity);
			_mockContext = DbContextMock.GetContext(_mockSet);
			_unitOfWork = new UnitOfWork(_mockContext);
		}


		[TestMethod]
		public void AddActivityCorrect()
		{
			List<Activity> actvities = new List<Activity>();
			DbSet<Activity> mockSet = DbContextMock.GetQueryableMockDbSet(actvities, a => a.IdActivity);
			ProfessionalPracticesContext mockContext = DbContextMock.GetContext(mockSet);
			UnitOfWork unitOfWork = DbContextMock.GetUnitOfWork(mockContext);
			string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			Activity newActivity = new Activity
			{
				IdGroup = 1,
				Name = "Actividad  2 reporte mensual",
				ActivityType = ActivityType.MonthlyReport,
				ActivityStatus = ActivityStatus.ACTIVE,
				IdActivity = 2,
				Description = " Deberan de subir su primer reporte mensual",
				ValueActivity = 100,
				StartDate = Convert.ToDateTime(date),
				FinishDate = Convert.ToDateTime(date),
				teacher = new Teacher
                {
					StaffNumber = "12345"
                }
			};

			unitOfWork.Activities.Add(newActivity);

			int expected = 1;
			int actual = actvities.Count;
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void ModifyActivityCorrect()
		{
			Activity activity = _unitOfWork.Activities.Get(2);
			activity.Name = "Actividad  2 reporte mensual";
			activity.ActivityType = ActivityType.MonthlyReport;
			activity.ActivityStatus = ActivityStatus.FINISHED;
			activity.Description = "Deberan de subir su primer reporte mensual";
			activity.ValueActivity = 100;
			_unitOfWork.Complete();
			_unitOfWork.Dispose();
			string expected = "Actividad  2 reporte mensual";
			string actual = activity.Name;
			Assert.AreEqual(expected, actual);
		}


		[TestMethod]
		public void ActivityInvalidInformation()
		{
			ActivityValidator validations = new ActivityValidator();
			Activity invalidActivity = new Activity
			{
				IdGroup = 1,
				Name = "",
				ActivityType = ActivityType.MonthlyReport,
				ActivityStatus = ActivityStatus.ACTIVE,
				IdActivity = 2,
				Description = "",
				ValueActivity = 1000,
				StartDate = null,
				FinishDate = null,
				teacher = new Teacher
				{
					StaffNumber = "12345"
				}
			};
			var result = validations.TestValidate(invalidActivity);
			result.ShouldHaveValidationErrorFor("Name");

		}
	}
}
