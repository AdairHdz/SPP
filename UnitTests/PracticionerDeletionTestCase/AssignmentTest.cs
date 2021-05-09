using System;
using System.Collections.Generic;
using System.Data.Entity;
using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.DataPersistenceLayerTests;

namespace UnitTests.PracticionerDeletionTestCase
{
    [TestClass]
    public class AssignmentTest
    {
        private List<Assignment> _data;
        private DbSet<Assignment> _mockSet;
        private ProfessionalPracticesContext _mockContext;
        private UnitOfWork _unitOfWork;

        [TestInitialize]
        public void TestInitialize()
        {
            _data = new List<Assignment>
            {
                new Assignment
                {
                    IdAssignment = 1,
                    StartTerm = "Febrero - Julio 2021",
                    CompletionTerm = "Agosto - Diciembre 2021",
                    DateAssignment = DateTime.Now,
                    Status = AssignmentStatus.Assigned,
                    IdProject = 1,
                    Enrollment = "S18012122",
                    IdOfficeOfAcceptance = 1
                }
            };
            _mockSet = DbContextMock.GetQueryableMockDbSet(_data, assignment => assignment.IdAssignment);
            _mockContext = DbContextMock.GetContext(_mockSet);
            _unitOfWork = new UnitOfWork(_mockContext);
        }

        [TestMethod]
        public void FindActiveAssignment_Success()
        {
            int idProject = 1;
            Assignment activeAssignment = _unitOfWork.Assignments.FindFirstOccurence(assignment => assignment.IdProject == idProject);
            _unitOfWork.Dispose();
            Assert.IsNotNull(activeAssignment);
        }

        [TestMethod]
        public void FindActiveAssignment_NoActiveAssignment()
        {
            int idProject = 2;
            Assignment activeAssignment = _unitOfWork.Assignments.FindFirstOccurence(assignment => assignment.IdProject == idProject);
            _unitOfWork.Dispose();
            Assert.IsNull(activeAssignment);
        }
    }
}
