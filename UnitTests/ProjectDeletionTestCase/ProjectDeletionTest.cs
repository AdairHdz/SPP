using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.DataPersistenceLayerTests;

namespace UnitTests.ProjectDeletionTestCase
{
    [TestClass]
    public class ProjectDeletionTest
    {
        private List<Project> _data;
        private DbSet<Project> _mockSet;
        private ProfessionalPracticesContext _mockContext;
        private UnitOfWork _unitOfWork;

        [TestInitialize]
        public void TestInitialize()
        {
            _data = new List<Project>
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
            _mockSet = DbContextMock.GetQueryableMockDbSet(_data, practicioner => practicioner.IdProject);
            _mockContext = DbContextMock.GetContext(_mockSet);            
            _unitOfWork = new UnitOfWork(_mockContext);
        }

        [TestMethod]
        public void DeleteProject_Success()
        {
            Project project = _unitOfWork.Projects.Get(1);
            project.Status = ProjectStatus.INACTIVE;
            _unitOfWork.Complete();
            

            int expected = 1;
            int currentlyActiveProjects = _unitOfWork.Projects.Find(proj => proj.Status == ProjectStatus.ACTIVE).Count();
            _unitOfWork.Dispose();
            Assert.AreEqual(expected, currentlyActiveProjects);
        }
    }
}
