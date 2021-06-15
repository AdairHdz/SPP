using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace UnitTests.DataPersistenceLayerTests.PartialReportTest
{
    [TestClass]
    public class PartialReportGenerationTest
    {
        private UnitOfWork _unitOfWork;
        private List<PartialReport> _data;

        [TestInitialize]
        public void TestInitialize()
        {
            DateTime localDate = DateTime.Now;
            _data = new List<PartialReport>
            {
                new PartialReport
                {
                    NumberReport = "PRIMERO",
                    ResultsObtained = "Actividades completas",
                    HoursCovered = 240,
                    Observations = "Falto una actividad",
                    DeliveryDate = localDate,
                    IdProject = 1,
                    Enrollment = "zS18012149"
                }
            };
            DbSet<PartialReport> _mockSet = DbContextMock.GetQueryableMockDbSet(_data, x => x.IdParcialReport);
            ProfessionalPracticesContext _mockContext = DbContextMock.GetContext(_mockSet);
            _unitOfWork = new UnitOfWork(_mockContext);
        }

        [TestMethod]
        public void DetermineIfPracticionerExists_Exists()
        {
             List<Practicioner> data = new List<Practicioner>
            {
                new Practicioner
                {
                    Enrollment = "zS18012149",
                    Credits = 300,
                    Group = new Group
                    {
                        IdGroup= 1,
                        Nrc = "12345"
                    },
                    IdUser = 1
                }
            };
            DbSet<Practicioner> mockSet = DbContextMock.GetQueryableMockDbSet(data, x => x.Enrollment);
            ProfessionalPracticesContext mockContext = DbContextMock.GetContext(mockSet);
            UnitOfWork unitOfWork = new UnitOfWork(mockContext);

            Practicioner practicioner = unitOfWork.Practicioners.FindFirstOccurence(Practicioner => Practicioner.Enrollment == "zS18012149");
            Assert.IsNotNull(practicioner);
        }

        [TestMethod]
        public void DetermineIfPartialReportsExists_Exists()
        {
            IEnumerable<PartialReport> partialReports = _unitOfWork.PartialReports.Find(PartialReport => PartialReport.Enrollment.Equals("zS18012149"));

            Assert.IsNotNull(partialReports);
        }

        [TestMethod]
        public void DetermineIfTeacherExists_Exists()
        {
            List<Teacher> data = new List<Teacher>
            {
                new Teacher
                {
                    StaffNumber = "1234",
                    IdUser = 1
                }
            };
            DbSet<Teacher> mockSet = DbContextMock.GetQueryableMockDbSet(data, x => x.StaffNumber);
            ProfessionalPracticesContext mockContext = DbContextMock.GetContext(mockSet);
            UnitOfWork unitOfWork = new UnitOfWork(mockContext);

            Teacher teacher = unitOfWork.Teachers.FindFirstOccurence(Teacher => Teacher.StaffNumber == "1234");
            Assert.IsNotNull(teacher);
        }

        [TestMethod]
        public void DetermineIfAssignmentExists_Exists()
        {
            List<Assignment> data = new List<Assignment>
            {
                new Assignment
                {
                    Enrollment = "zS18012149",
                    IdProject = 1,
                    CompletionTerm = "Febrero - Mayo 2021",
                }
            };
            DbSet<Assignment> mockSet = DbContextMock.GetQueryableMockDbSet(data, x => x.IdAssignment);
            ProfessionalPracticesContext mockContext = DbContextMock.GetContext(mockSet);
            UnitOfWork unitOfWork = new UnitOfWork(mockContext);

            Assignment assignment = unitOfWork.Assignments.FindFirstOccurence(Assignment => Assignment.Enrollment == "zS18012149");
            Assert.IsNotNull(assignment);
        }

        [TestMethod]
        public void DetermineIfActivityPracticionersExists_Exists()
        {
            string dateConvert = "2021-04-04 12:14:11";
            IFormatProvider format = new System.Globalization.CultureInfo("es-ES", false);
            DateTime date = new DateTime();
            date = DateTime.ParseExact(dateConvert, "yyyy-MM-dd HH:mm:ss", format);

            List<ActivityPracticioner> data = new List<ActivityPracticioner>
            {
                new ActivityPracticioner
                {
                    IdActivityPracticioner=9,
                    Qualification=0,
                    ActivityPracticionerStatus = ActivityPracticionerStatus.NOTQUALIFIED,
                    Enrollment = "zS18012149",
                     Activity = new Activity
                    {
                        IdActivity =1,
                        Name="Primer reporte parcial",
                        ActivityType = ActivityType.PartialReport,
                        ActivityStatus = ActivityStatus.ACTIVE,
                        Description = "Entregar el primer reporte parcial corresponsdiente a 200 horas.",
                        ValueActivity = 10,
                        StartDate = new DateTime(),
                        FinishDate = date,
                        StaffNumberTeacher ="5678"
                    }
                }
            };
            DbSet<ActivityPracticioner> mockSet = DbContextMock.GetQueryableMockDbSet(data, x => x.IdActivityPracticioner);
            ProfessionalPracticesContext mockContext = DbContextMock.GetContext(mockSet);
            UnitOfWork unitOfWork = new UnitOfWork(mockContext);

            IEnumerable<ActivityPracticioner> activityPracticioners  = unitOfWork.ActivityPracticioners.Find(ActivityPracticioner => ActivityPracticioner.Enrollment.Equals("zS18012149") &&
                        ActivityPracticioner.Activity.ActivityType == ActivityType.PartialReport && ActivityPracticioner.Activity.ActivityStatus == ActivityStatus.ACTIVE);
            Assert.IsNotNull(activityPracticioners);
        }

        [TestMethod]
        public void RegisterPartialReport_Exists()
        {
            DateTime localDate = DateTime.Now;
            List<PartialReport> partialReports = new List<PartialReport>();
            DbSet<PartialReport> mockSet = DbContextMock.GetQueryableMockDbSet(partialReports, p => p.IdParcialReport);
            ProfessionalPracticesContext mockContext = DbContextMock.GetContext(mockSet);
            UnitOfWork unitOfWork = DbContextMock.GetUnitOfWork(mockContext);
            PartialReport newPartialReport = new PartialReport
            {
                NumberReport = "PRIMERO",
                ResultsObtained = "Actividades completas",
                HoursCovered = 240,
                Observations = "Falto una actividad",
                DeliveryDate = localDate,
                IdProject = 1,
                Enrollment = "zS18012149"
            };
            unitOfWork.PartialReports.Add(newPartialReport);
            int expected = 1;
            int actual = partialReports.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RegisterActivitiesMade_Exists()
        {
            DateTime localDate = DateTime.Now;
            List<ActivityMade> activitiesMades = new List<ActivityMade>();
            DbSet<ActivityMade> mockSet = DbContextMock.GetQueryableMockDbSet(activitiesMades, p => p.IdActivity);
            ProfessionalPracticesContext mockContext = DbContextMock.GetContext(mockSet);
            UnitOfWork unitOfWork = DbContextMock.GetUnitOfWork(mockContext);
            List<ActivityMade> newActivities = new List<ActivityMade>
            {
                new ActivityMade
                {
                    Name = "Pruebas",
                    IdPartialReport = 1,
                    PlannedWeek = "S1 S2 S3",
                    RealWeek = "S1 S2 S3"
                },
                new ActivityMade
                {
                    Name = "Modelo de dominio",
                    IdPartialReport = 1,
                    PlannedWeek = "S1 S2 S3",
                    RealWeek = "S1 S2 S3"
                }
            };
            unitOfWork.ActivityMades.AddRange(newActivities);
            int expected = 2;
            int actual = newActivities.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}
