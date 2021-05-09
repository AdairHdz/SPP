using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.Repositories;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using UnitTests.DataPersistenceLayerTests;

namespace UnitTests.LinkedOrganizationDeletionTestCase
{
    [TestClass]
    public class LinkedOrganizationDeletionTest
    {
        private List<LinkedOrganization> _data;
        private DbSet<LinkedOrganization> _mockSet;
        private ProfessionalPracticesContext _mockContext;
        private LinkedOrganizationRepository _repository;
        private UnitOfWork _unitOfWork;

        [TestInitialize]
        public void TestInitialize()
        {
            _data = new List<LinkedOrganization>
            {
                new LinkedOrganization
                {
                    IdLinkedOrganization = 1,
                    Name = "Intel",
                    DirectUsers = "Comunidad estudiantil",
                    IndirectUsers = "Comunidad tecnológica",
                    Email = "intel@gmail.com",
                    PhoneNumbers = new List<Phone>
                    {
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "521",
                            PhoneNumber = "2281244285"
                        },
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "521",
                            PhoneNumber = "2281115489"
                        },
                    },
                    Address = "Enrique Segoviano",
                    IdCity = 1,
                    IdState = 1,
                    IdSector = 1,
                    LinkedOrganizationStatus = LinkedOrganizationStatus.ACTIVE,
                },
                 new LinkedOrganization
                {
                    IdLinkedOrganization = 2,
                    Name = "Sony",
                    DirectUsers = "Comunidad estudiantil",
                    IndirectUsers = "Comunidad tecnológica",
                    Email = "business@sony.com",
                    PhoneNumbers = new List<Phone>
                    {
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "521",
                            PhoneNumber = "2281244290"
                        },
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            IdLinkedOrganization = 1,
                            Extension = "521",
                            PhoneNumber = "2281240021"
                        },
                    },
                    Address = "Obrero Campesino",
                    IdCity = 1,
                    IdState = 1,
                    IdSector = 1,
                    LinkedOrganizationStatus = LinkedOrganizationStatus.ACTIVE,
                }
            };

            _mockSet = DbContextMock.GetQueryableMockDbSet(_data, x => x.IdLinkedOrganization);
            _mockContext = DbContextMock.GetContext(_mockSet);
            _repository = new LinkedOrganizationRepository(_mockContext);
            _unitOfWork = new UnitOfWork(_mockContext);

        }

        [TestMethod]
        public void DeleteLinkedOrganization_Success()
        {
            LinkedOrganization linkedOrganizationToBeDeleted = _unitOfWork.LinkedOrganizations.Get(1);
            linkedOrganizationToBeDeleted.LinkedOrganizationStatus = LinkedOrganizationStatus.INACTIVE;
            _unitOfWork.Complete();

            IList<LinkedOrganization> activeLinkedOrganizations =
                _unitOfWork.LinkedOrganizations.Find(linkedOrg => linkedOrg.LinkedOrganizationStatus == LinkedOrganizationStatus.ACTIVE).ToList();
            _unitOfWork.Complete();
            _unitOfWork.Dispose();
            int expected = 1;
            Assert.AreEqual(expected, activeLinkedOrganizations.Count());
        }

        [TestMethod]
        public void DeleteLinkedOrganizationwithInactiveProjects_Success()
        {
            LinkedOrganization linkedOrganizationToBeDeleted = _unitOfWork.LinkedOrganizations.Get(1);
            linkedOrganizationToBeDeleted.Projects = new List<Project>();
            linkedOrganizationToBeDeleted.Projects.Add(new Project
            {
                NameProject = "Desarrollo de sistema web",
                Description = "Creación de un sistema web de inventario",
                ObjectiveGeneral = "Desarrollar un sistema web",
                ObjectiveImmediate = "Crear una aplicación web que permita llevar el control de inventario",
                ObjectiveMediate = "Crear documentación pertinente",
                Methodology = "SCRUM",
                Resources = "Computadora portatil, escritorio",
                Status = ProjectStatus.INACTIVE,
                Duration = 100,
                Activities = "Desarrollar el sistema",
                Responsibilities = "Desarrollar el sistema",
                QuantityPracticing = 2,
                Term = "Febrero - Julio 2021",
                IdLinkedOrganization = 1,
                StaffNumberCoordinator = "S18012122",
                IdResponsibleProject = 1,
            });
            _unitOfWork.Complete();
            bool linkedOrganizationHasActiveProjects = _unitOfWork.LinkedOrganizations.HasActiveProjects(1);
            _unitOfWork.Dispose();
            Assert.IsFalse(linkedOrganizationHasActiveProjects);
        }

        [TestMethod]
        public void DeleteLinkedOrganization_ErrorDueToActiveProjects()
        {
            LinkedOrganization linkedOrganizationToBeDeleted = _unitOfWork.LinkedOrganizations.Get(1);
            linkedOrganizationToBeDeleted.Projects = new List<Project>();
            linkedOrganizationToBeDeleted.Projects.Add(new Project
            {
                NameProject = "Desarrollo de sistema web",
                Description = "Creación de un sistema web de inventario",
                ObjectiveGeneral = "Desarrollar un sistema web",
                ObjectiveImmediate = "Crear una aplicación web que permita llevar el control de inventario",
                ObjectiveMediate = "Crear documentación pertinente",
                Methodology = "SCRUM",
                Resources = "Computadora portatil, escritorio",
                Status = ProjectStatus.ACTIVE,
                Duration = 100,
                Activities = "Desarrollar el sistema",
                Responsibilities = "Desarrollar el sistema",
                QuantityPracticing = 2,
                Term = "Febrero - Julio 2021",
                IdLinkedOrganization = 1,
                StaffNumberCoordinator = "S18012122",
                IdResponsibleProject = 1,
            });
            _unitOfWork.Complete();            
            bool linkedOrganizationHasActiveProjects = _unitOfWork.LinkedOrganizations.HasActiveProjects(1);            
            _unitOfWork.Dispose();
            Assert.IsTrue(linkedOrganizationHasActiveProjects);
        }
    }
}
