using System;
using System.Collections.Generic;
using System.Data.Entity;
using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.DataPersistenceLayerTests;

namespace UnitTests.ProjectAssignationTestCase
{
    [TestClass]
    public class ProjectAssignationTest
    {
        private List<Project> _projectData;
        private List<Assignment> _assignmentData;
        private DbSet<Project> _projectMockSet;
        private DbSet<Assignment> _assignmentMockSet;
        private ProfessionalPracticesContext _projectMockContext;
        private ProfessionalPracticesContext _assignmentMockContext;
        private UnitOfWork _projectUnitOfWork;
        private UnitOfWork _assignmentUnitOfWork;

        [TestInitialize]
        public void TestInitialize()
        {
            _projectData = new List<Project>
            {
                new Project
                {
                    IdProject = 1,
                    NameProject = "Desarrollo de inventario",
                    Description = "Se debe desarrollar un inventario para una red inmobiliaria",
                    ObjectiveGeneral = "El objetivo general",
                    ObjectiveImmediate = "El objetivo inmediato",
                    ObjectiveMediate = "El objetivo mediato",
                    Methodology = "SCRUM",
                    Resources = "Recursos",
                    Status = ProjectStatus.ACTIVE,
                    Duration = 480,
                    Activities = "Actividades",
                    Responsibilities = "Responsabilidades",
                    QuantityPracticing = 2,
                    IdLinkedOrganization = 1,
                    StaffNumberCoordinator = "ABC123",
                    IdResponsibleProject = 1
                },
                new Project
                {
                    IdProject = 2,
                    NameProject = "Sistema bibliotecario",
                    Description = "Se debe desarrollar un sistema bibliotecario",
                    ObjectiveGeneral = "El objetivo general",
                    ObjectiveImmediate = "El objetivo inmediato",
                    ObjectiveMediate = "El objetivo mediato",
                    Methodology = "SCRUM",
                    Resources = "Recursos",
                    Status = ProjectStatus.ACTIVE,
                    Duration = 360,
                    Activities = "Actividades",
                    Responsibilities = "Responsabilidades",
                    QuantityPracticing = 1,
                    IdLinkedOrganization = 1,
                    StaffNumberCoordinator = "ABC123",
                    IdResponsibleProject = 1
                }
            };

            _assignmentData = new List<Assignment>();

            _projectMockSet = DbContextMock.GetQueryableMockDbSet(_projectData, practicioner => practicioner.IdProject);
            _assignmentMockSet = DbContextMock.GetQueryableMockDbSet(_assignmentData, assignment => assignment.IdAssignment);
            _projectMockContext = DbContextMock.GetContext(_projectMockSet);
            _assignmentMockContext = DbContextMock.GetContext(_assignmentMockSet);
            _projectUnitOfWork = new UnitOfWork(_projectMockContext);
            _assignmentUnitOfWork = new UnitOfWork(_assignmentMockContext);
        }

        [TestMethod]
        public void AssignProject_Success()
        {
            Assignment assignment = new Assignment
            {
                DateAssignment = DateTime.Now,
                StartTerm = "FEB-JUL 2021",
                IdProject = 1,
                Enrollment = "zS18012122"
            };

            _assignmentUnitOfWork.Assignments.Add(assignment);
            _assignmentUnitOfWork.Complete();

            Project assignedProject = _projectUnitOfWork.Projects.Get(1);
            assignedProject.QuantityPracticingAssing += 1;
            _projectUnitOfWork.Complete();

            int expected = 1;
            int actual = _projectUnitOfWork.Projects.Get(1).QuantityPracticingAssing;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AssignProjectAndChangeStatusToFilled_Success()
        {
            Assignment assignment = new Assignment
            {
                DateAssignment = DateTime.Now,
                StartTerm = "FEB-JUL 2021",
                IdProject = 2,
                Enrollment = "zS18009988"
            };

            _assignmentUnitOfWork.Assignments.Add(assignment);
            _assignmentUnitOfWork.Complete();

            Project assignedProject = _projectUnitOfWork.Projects.Get(2);
            assignedProject.QuantityPracticingAssing += 1;

            if(assignedProject.QuantityPracticingAssing == assignedProject.QuantityPracticing)
            {
                assignedProject.Status = ProjectStatus.FILLED;
            }

            _projectUnitOfWork.Complete();

            ProjectStatus expected = ProjectStatus.FILLED;
            ProjectStatus actual = _projectUnitOfWork.Projects.Get(2).Status;
            Assert.AreEqual(expected, actual);
        }
    }
}
