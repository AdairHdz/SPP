using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using UnitTests.DataPersistenceLayerTests;

namespace UnitTests.ConsultProgressTestCase
{
	[TestClass]
	public class ConsultProgressTest
	{
		[TestMethod]
		public void ValidGetPracticionerProject()
		{
			List<Assignment> assigment = new List<Assignment>
			{
				new Assignment
				{
					IdAssignment = 1,
					CompletionTerm = "FEBRERO-JUNIO 2021",
					Status = AssignmentStatus.Assigned,
					IdOfficeOfAcceptance = 1,
					IdProject = 1,
					Enrollment = "zS18012124",
					DateAssignment = DateTime.Now
				}
			};
			DbSet<Assignment> _mockSet = DbContextMock.GetQueryableMockDbSet(assigment, a => a.IdAssignment);
			ProfessionalPracticesContext _mockContext = DbContextMock.GetContext(_mockSet);
			UnitOfWork unitOfWork = new UnitOfWork(_mockContext);
			IEnumerable<Assignment> assignment = unitOfWork.Assignments.Find(Assigment => Assigment.Enrollment == "zS18012124" && Assigment.IdProject == 1);
			Assert.IsNotNull(assignment);
		}

		[TestMethod]
		public void ValidGetPracticionerPartialReports()
		{
			List<ActivityPracticioner> partialReport = new List<ActivityPracticioner>
			{
				new ActivityPracticioner
				{
					IdActivityPracticioner = 2,
					Qualification = 10,
					Observation = "Muy buen reporte",
					ActivityPracticionerStatus = ActivityPracticionerStatus.QUALIFIED,
					Enrollment = "zS18012124",
					IdActivity = 2,
					Activity = new Activity
					{
						IdActivity = 2,
						Name = "Primer Reporte Parcial ",
						ActivityType = ActivityType.PartialReport,
						ActivityStatus = ActivityStatus.ACTIVE,
						Description = "En esta actividad deberán subir su reporte parcial",
						ValueActivity = 10,
						StaffNumberTeacher = "12345"
					}
				}
			};
			DbSet<ActivityPracticioner> _mockSet = DbContextMock.GetQueryableMockDbSet(partialReport, a => a.IdActivityPracticioner);
			ProfessionalPracticesContext _mockContext = DbContextMock.GetContext(_mockSet);
			UnitOfWork unitOfWork = new UnitOfWork(_mockContext);
			IEnumerable<ActivityPracticioner> report = unitOfWork.ActivityPracticioners.Find(ActivityPracticioner => ActivityPracticioner.Enrollment == "zS18012124"
			&& ActivityPracticioner.Activity.ActivityType == ActivityType.PartialReport);
			Assert.IsNotNull(report);
		}

		[TestMethod]
		public void ValidGetPracticionerMonthlyReports()
		{
			List<ActivityPracticioner> monthlyReport = new List<ActivityPracticioner>
			{
				new ActivityPracticioner
				{
					IdActivityPracticioner = 1,
					Qualification = 10,
					Observation = "Muy buen reporte",
					ActivityPracticionerStatus = ActivityPracticionerStatus.QUALIFIED,
					Enrollment = "zS18012124",
					Activity = new Activity
					{
						IdActivity = 1,
						Name = "Primer Reporte Mensual ",
						ActivityType = ActivityType.MonthlyReport,
						ActivityStatus = ActivityStatus.ACTIVE,
						Description = "En esta actividad deberán subir su reporte mensual",
						ValueActivity = 10,
						StaffNumberTeacher = "12345"
					}
				}
			};
			DbSet<ActivityPracticioner> _mockSet = DbContextMock.GetQueryableMockDbSet(monthlyReport, a => a.IdActivityPracticioner);
			ProfessionalPracticesContext _mockContext = DbContextMock.GetContext(_mockSet);
			UnitOfWork unitOfWork = new UnitOfWork(_mockContext);
			IEnumerable<ActivityPracticioner> report = unitOfWork.ActivityPracticioners.Find(ActivityPracticioner => ActivityPracticioner.Enrollment == "zS18012124"
			&& ActivityPracticioner.Activity.ActivityType == ActivityType.MonthlyReport);
			Assert.IsNotNull(report);
		}
	}
}
