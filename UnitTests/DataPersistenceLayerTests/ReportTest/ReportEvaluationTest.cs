using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace UnitTests.DataPersistenceLayerTests.ReportTest
{
    [TestClass]
    public class ReportEvaluationTest
    {
        private UnitOfWork _unitOfWork;
        private UnitOfWork _unitOfWorkDocument;
        private UnitOfWork _unitOfWorkActivity;
        private List<ActivityPracticioner> _data;
        private List<Document> _dataDocument;
        private List<Activity> _dataActivity;

        [TestInitialize]
        public void TestInitialize()
        {
            string dateConvert = "2021-04-04 12:14:11";
            IFormatProvider format = new System.Globalization.CultureInfo("es-ES", false);
            DateTime date = new DateTime();
            date = DateTime.ParseExact(dateConvert, "yyyy-MM-dd HH:mm:ss", format);

            _data = new List<ActivityPracticioner>
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
            DbSet<ActivityPracticioner> _mockSet = DbContextMock.GetQueryableMockDbSet(_data, x => x.IdActivityPracticioner);
            ProfessionalPracticesContext _mockContext = DbContextMock.GetContext(_mockSet);
            _unitOfWork = new UnitOfWork(_mockContext);

            _dataActivity = new List<Activity>{
                new Activity
                {
                    IdActivity = 1,
                    Name = "Primer reporte parcial",
                    ActivityType = ActivityType.PartialReport,
                    ActivityStatus = ActivityStatus.ACTIVE,
                    Description = "Entregar el primer reporte parcial corresponsdiente a 200 horas.",
                    ValueActivity = 10,
                    StartDate = new DateTime(),
                    FinishDate = date,
                    StaffNumberTeacher = "5678"
                }
            };
            DbSet<Activity> _mockSetActivity = DbContextMock.GetQueryableMockDbSet(_dataActivity, x => x.IdActivity);
            ProfessionalPracticesContext _mockContextActivity = DbContextMock.GetContext(_mockSetActivity);
            _unitOfWorkActivity = new UnitOfWork(_mockContextActivity);

            _dataDocument = new List<Document>{
                new Document
                {
                    IdDocument = 1,
                    Name = "datos.docx",
                    RouteSave = "C:/Users/MARTHA/Documents/Activity/1",
                    DeliveryDate = date,
                    ActivityPracticioner = new ActivityPracticioner
                    {
                        IdActivityPracticioner=9,
                        Qualification=0,
                        ActivityPracticionerStatus = ActivityPracticionerStatus.NOTQUALIFIED,
                        Enrollment = "zS18012149",
                        IdActivity = 1,
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
                }
            };
            DbSet<Document> _mockSetDocument = DbContextMock.GetQueryableMockDbSet(_dataDocument, x => x.IdDocument);
            ProfessionalPracticesContext _mockContextDocument = DbContextMock.GetContext(_mockSetDocument);
            _unitOfWorkDocument = new UnitOfWork(_mockContextDocument);
        }

        [TestMethod]
        public void GetTeacher()
        {
            List<Teacher> data = new List<Teacher>
            {
                new Teacher
                {
                    IdUser =2,
                    StaffNumber = "5518"
                }
            };
            DbSet<Teacher> _mockSet = DbContextMock.GetQueryableMockDbSet(data, x => x.IdUser);
            ProfessionalPracticesContext _mockContext = DbContextMock.GetContext(_mockSet);
            UnitOfWork unitOfWork = new UnitOfWork(_mockContext);
            Teacher teacher = unitOfWork.Teachers.FindFirstOccurence(Teacher => Teacher.IdUser == 2);
            Assert.IsNotNull(teacher);
        }

        [TestMethod]
        public void ConsultActivities()
        {
            IEnumerable<Activity> activities = _unitOfWorkActivity.Activities.Find(Activity => Activity.StaffNumberTeacher.Equals("5678") && (Activity.ActivityType.Equals(ActivityType.MonthlyReport) || Activity.ActivityType.Equals(ActivityType.PartialReport)));
            Assert.IsNotNull(activities);
        }

        [TestMethod]
        public void ConsultActivityPracticioners()
        {
            IEnumerable<ActivityPracticioner> activityPracticioners = _unitOfWork.ActivityPracticioners.Find(ActivityPracticioner => ActivityPracticioner.IdActivity == 9);
            Assert.IsNotNull(activityPracticioners);
        }

        [TestMethod]
        public void ConsultDocuments()
        {
            IEnumerable<Document> documents = _unitOfWorkDocument.Documents.Find(Document => Document.ActivityPracticioner.IdActivity == 1);
            Assert.IsNotNull(documents);
        }

        [TestMethod]
        public void AddReport()
        {
            ActivityPracticioner updateActivityPracticioner = _unitOfWork.ActivityPracticioners.Get(9);
            updateActivityPracticioner.Observation = "Muy bien realizado";
            updateActivityPracticioner.Qualification = 10;
            updateActivityPracticioner.ActivityPracticionerStatus = ActivityPracticionerStatus.QUALIFIED;
            int expected = 1;
            int actual = _data.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}
