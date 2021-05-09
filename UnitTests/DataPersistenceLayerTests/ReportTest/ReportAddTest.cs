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
    public class ReportAddTest
    {
        private UnitOfWork _unitOfWork;
        private List<ActivityPracticioner> _data;

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
        }

        [TestMethod]
        public void GetPracticioner()
        {
            List<Practicioner>  data = new List<Practicioner>
            {
                new Practicioner
                {
                    IdUser =2,
                    Enrollment="zS18012149",
                    Term = "FEBREO-JULIO 2021",
                    Credits = 300
                }
            };
            DbSet<Practicioner> _mockSet = DbContextMock.GetQueryableMockDbSet(data, x => x.IdUser);
            ProfessionalPracticesContext _mockContext = DbContextMock.GetContext(_mockSet);
            UnitOfWork unitOfWork = new UnitOfWork(_mockContext);
            Practicioner practicioner = unitOfWork.Practicioners.FindFirstOccurence(Practicioner => Practicioner.IdUser == 2);
            Assert.IsNotNull(practicioner);
        }

        [TestMethod]
        public void ConsultPartialReport()
        {
            IEnumerable<ActivityPracticioner> activityPracticioners = _unitOfWork.ActivityPracticioners.Find(ActivityPracticioner => ActivityPracticioner.Enrollment.Equals("zS1801249") && ActivityPracticioner.Activity.ActivityType.Equals(ActivityType.PartialReport));
            Assert.IsNotNull(activityPracticioners);
        }

        [TestMethod]
        public void ConsultMonthlyReport()
        {
            IEnumerable<ActivityPracticioner> activityPracticioners = _unitOfWork.ActivityPracticioners.Find(ActivityPracticioner => ActivityPracticioner.Enrollment.Equals("zS1801249") && ActivityPracticioner.Activity.ActivityType.Equals(ActivityType.MonthlyReport));
            Assert.IsNotNull(activityPracticioners);
        }

        [TestMethod]
        public void AddReport()
        {
            List<Document> data = new List<Document>
            {
                new Document
                {
                    IdDocument =9,
                    Name = "DoctoProyecto.docx"
                }
            };
            DbSet<Document> _mockSet = DbContextMock.GetQueryableMockDbSet(data, x => x.IdDocument);
            ProfessionalPracticesContext _mockContext = DbContextMock.GetContext(_mockSet);
            UnitOfWork unitOfWork = new UnitOfWork(_mockContext);
            Document documentUpdate = unitOfWork.Documents.Get(9);
            documentUpdate.Name = "DoctoProyecto.docx";
            documentUpdate.RouteSave = "C:/Users/MARTHA/Documents/Activity/1";
            documentUpdate.TypeDocument = "Reporte Mensual";
            documentUpdate.TypeDocument = "Reporte Parcial";
            DateTime deliveryDate = DateTime.Now;
            documentUpdate.DeliveryDate = deliveryDate;

            ActivityPracticioner activityPracticionerUpdate = _unitOfWork.ActivityPracticioners.Get(9);
            activityPracticionerUpdate.Activity.ActivityStatus = ActivityStatus.ACTIVE;
            activityPracticionerUpdate.Answer = "Entrega";
            int expected = 1;
            int actual = _data.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}
