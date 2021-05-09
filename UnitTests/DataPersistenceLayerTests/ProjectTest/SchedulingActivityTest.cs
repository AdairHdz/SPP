using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;

namespace UnitTests.DataPersistenceLayerTests.ProjectTest
{
    [TestClass]
    public class SchedulingActivityTest
    {
        private UnitOfWork _unitOfWork;
        private List<SchedulingActivity> _data;

        [TestInitialize]
        public void TestInitialize()
        {
            _data = new List<SchedulingActivity>
            {
                new SchedulingActivity
                {
                    IdSchedulingActivity = 4,
                    Month = "Febrero",
                    Activity = "Análisis de requerimientos, recopilación de información, documentación y revisión de procedimientos.Capacitación en el IDE de desarrollo y metodología.",
                    IdProject = 4
                }
            };
            DbSet<SchedulingActivity> _mockSet = DbContextMock.GetQueryableMockDbSet(_data, x => x.IdSchedulingActivity);
            ProfessionalPracticesContext _mockContext = DbContextMock.GetContext(_mockSet);
            _unitOfWork = new UnitOfWork(_mockContext);
        }

        [TestMethod]
        public void RegisterSchedulingActivity_Exists()
        {
            SchedulingActivity newSchedulingActivity = new SchedulingActivity
            {
                Month = "Febrero",
                Activity = "Análisis de requerimientos, recopilación de información, documentación y revisión de procedimientos.Capacitación en el IDE de desarrollo y metodología."
            };
            _unitOfWork.SchedulingActivities.Add(newSchedulingActivity);
            int expected = 1;
            int actual = _data.Count-1;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DeleteSchedulingActivity_Exists()
        {
            SchedulingActivity deleteSchedulingActivity = new SchedulingActivity
            {
                IdSchedulingActivity = 4,
                Month = "Febrero",
                Activity = "Análisis de requerimientos, recopilación de información, documentación y revisión de procedimientos.Capacitación en el IDE de desarrollo y metodología.",
                IdProject = 4
            }; 
            _unitOfWork.SchedulingActivities.Remove(deleteSchedulingActivity);
            int expected = 1;
            int actual = _data.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateSchedulingActivity_Exists()
        {
            SchedulingActivity updateSchedulingActivity = _unitOfWork.SchedulingActivities.Get(4);
            updateSchedulingActivity = new SchedulingActivity();
            updateSchedulingActivity.Month = "Mayo";
            updateSchedulingActivity.Activity = "Análisis de requerimientos, recopilación de información, documentación y revisión de procedimientos.Capacitación en el IDE de desarrollo y metodología.";
            int expected = 1;
            int actual = _data.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DetermineIfSchedulingActivityAlreadyExists_Exists()
        {
            SchedulingActivity schedulingActivity = _unitOfWork.SchedulingActivities.FindFirstOccurence(SchedulingActivity => SchedulingActivity.IdProject == 3 && SchedulingActivity.Month.Equals("Abril"));
            Assert.IsNull(schedulingActivity);
        }

        [TestMethod]
        public void DetermineIfSchedulingActivityWithIdAlreadyExists_Exists()
        {
            SchedulingActivity schedulingActivity = _unitOfWork.SchedulingActivities.FindFirstOccurence(SchedulingActivity => SchedulingActivity.IdProject == 3 && SchedulingActivity.Month.Equals("Abril") && SchedulingActivity.IdSchedulingActivity != 4);
            Assert.IsNull(schedulingActivity);
        }
    }
}
