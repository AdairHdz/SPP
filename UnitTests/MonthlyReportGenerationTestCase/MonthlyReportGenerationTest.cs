using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PresentationLayer.Validators;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using UnitTests.DataPersistenceLayerTests;

namespace UnitTests.MonthlyReportGenerationTestCase
{
	[TestClass]
	public class MonthlyReportGenerationTest
	{
		private ProfessionalPracticesContext _mockContext;
		private UnitOfWork _unitOfWork;
		private List<MonthlyReport> _monthlyReports;
		private DbSet<MonthlyReport> _mockSet;

		[TestInitialize]
		public void TestInitialize()
		{
			_monthlyReports = new List<MonthlyReport>
			{
				new MonthlyReport
				{
					IdMonthlyReport = 2,
					DeliveryDate = DateTime.Now,
					PerformedActivities = "Acordar con la organización vinculada las tareas que se realizarán",
					ResultsObtained = "Documento de calendario de actividades",
					HoursReported = 20,
					HoursCumulative = 20,
					Enrollment = "zS18012124",
					IdProject = 1
				}
			};

			_mockSet = DbContextMock.GetQueryableMockDbSet(_monthlyReports, p => p.IdMonthlyReport);
			_mockContext = DbContextMock.GetContext(_mockSet);
			_unitOfWork = DbContextMock.GetUnitOfWork(_mockContext);
		}

		[TestMethod]
		public void ValidInformationTest()
		{
			MonthlyReport monthlyReport = new MonthlyReport
			{
				IdMonthlyReport = 1,
				DeliveryDate = DateTime.Now,
				PerformedActivities = "Acordar con la organización vinculada las tareas que se realizarán",
				ResultsObtained = "Documento de calendario de actividades",
				HoursReported = 20,
				HoursCumulative = 20,
				Enrollment = "zS18012124",
				IdProject = 1
			};
			MonthlyReportValidator monthlyReportValidator = new MonthlyReportValidator();
			ValidationResult validationResult = monthlyReportValidator.Validate(monthlyReport);
			Assert.IsTrue(validationResult.IsValid);
		}

		[TestMethod]
		public void InvalidInformationTest()
		{
			MonthlyReport monthlyReport = new MonthlyReport
			{
				IdMonthlyReport = 1,
				DeliveryDate = DateTime.Now,
				PerformedActivities = null,
				ResultsObtained = null,
				HoursCumulative = 20,
				Enrollment = "zS18012124",
				IdProject = 1
			};
			MonthlyReportValidator monthlyReportValidator = new MonthlyReportValidator();
			ValidationResult validationResult = monthlyReportValidator.Validate(monthlyReport);
			int errors = validationResult.Errors.Count;
			Assert.AreNotEqual(errors, 0);
		}

		[TestMethod]
		public void RegisterInformationTest()
		{
			MonthlyReport monthlyReport = new MonthlyReport
			{
				IdMonthlyReport = 1,
				DeliveryDate = DateTime.Now,
				PerformedActivities = "Acordar con la organización vinculada las tareas que se realizarán",
				ResultsObtained = "Documento de calendario de actividades",
				HoursReported = 20,
				HoursCumulative = 20,
				Enrollment = "zS18012124",
				IdProject = 1
			};

			_unitOfWork.MonthlyReports.Add(monthlyReport);
			int expected = 2;
			int actual = _monthlyReports.Count;
			Assert.AreEqual(expected, actual);
		}
	}
}
