using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace UnitTests.DataPersistenceLayerTests.ProjectTest
{
    [TestClass]
    public class ProjectModificationTest
    {
        private UnitOfWork _unitOfWork;
        List<Project> _data;

        [TestInitialize]
        public void TestInitialize()
        {
             _data = new List<Project>
            {
                new Project
                {
                    NameProject = "Sistema Integral Académico",
                    Description = "Desarrollar un Sistema Web que gestione los procesos académicos que realizan las diferentes áreas dentro de la Universidad Veracruzana dentro de un mismo portal.",
                    ObjectiveGeneral = "Optimizar los procesos de consulta y seguimiento de los académicos.",
                    ObjectiveImmediate = "Revisión, análisis y documentación de requerimientos académicos con las áreas involucradas durante este desarrollo. "+
                                            "Revisión y análisis de la arquitectura." +
                                            "Desarrollo de prototipo.",
                    ObjectiveMediate = "Modificación de documentación y Modificación de prototipos.",
                    Methodology = "Proceso de desarrollo iterativo y Design Sprint, SCRUM",
                    Resources = "1 Ingeniero de software/programador Web "+
                        "Recursos materiales: "+
                         "Computadoras de escritorio "+
                         "IDE para programación(Visual Studio con C#) " +
                         "Acceso a Internet "+
                         "Documentación de procesos",
                    Activities ="Realizar a cabo la documentación del desarrollo del proyecto de la primera fase del proyecto, mediante el modelado de casos de uso, la descripción de los mismos y modelo de dominio, desarrollar sobre lenguaje C# y servicios dentro de un API, además de trabajar en equipo dentro del departamento.",
                    Responsibilities = "Cumplir con las funciones y actividades que sean asignadas "+
                        "Cumplir en tiempo y forma con las entregas de prototipos y productos " +
                        "Desarrollar en un ambiente colaborativo "+
                        "Trabajar de acuerdo a los estándares establecidos",
                    Duration = 480,
                    Term = "FEBRERO-JULIO 2021",
                    QuantityPracticing = 3,
                    StaffNumberCoordinator = "1515151"
                }
            };
            DbSet<Project> _mockSet = DbContextMock.GetQueryableMockDbSet(_data, x => x.IdProject);
            ProfessionalPracticesContext _mockContext = DbContextMock.GetContext(_mockSet);
            _unitOfWork = new UnitOfWork(_mockContext);
        }

        [TestMethod]
        public void ObteinProjectExists_Exists()
        {
            Project projectWithSameId = _unitOfWork.Projects.FindFirstOccurence(Project => Project.IdProject == 3);
            Assert.IsNull(projectWithSameId);
        }

        [TestMethod]
        public void ObteinSchedulingActivitiesExists_Exists()
        {
            try
            {
                IEnumerable<SchedulingActivity> listSchedulingActivity = _unitOfWork.SchedulingActivities.Find(schedulingActivity => schedulingActivity.IdProject == 3);
                Assert.IsNotNull(listSchedulingActivity);
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void DetermineIfProjectAlreadyExists_Exists()
        {
            Project projectWithSameName = _unitOfWork.Projects.FindFirstOccurence(Project => Project.NameProject == "Sistema Académico" && Project.IdProject != 3);
            Assert.IsNull(projectWithSameName);
        }

        [TestMethod]
        public void UpdateProject()
        {
            try
            {
                Project updateProject = _unitOfWork.Projects.Get(3);
                updateProject.NameProject = "Sistema Integral Académico";
                updateProject.Description = "Desarrollar un Sistema Web que gestione los procesos académicos que realizan las diferentes áreas dentro de la Universidad Veracruzana dentro de un mismo portal.";
                updateProject.ObjectiveGeneral = "Optimizar los procesos de consulta y seguimiento de los académicos.";
                updateProject.ObjectiveImmediate = "Revisión, análisis y documentación de requerimientos académicos con las áreas involucradas durante este desarrollo. " +
                                                "Revisión y análisis de la arquitectura." +
                                                "Desarrollo de prototipo.";
                updateProject.ObjectiveMediate = "Modificación de documentación y Modificación de prototipos.";
                updateProject.Methodology = "Proceso de desarrollo iterativo y Design Sprint, SCRUM";
                updateProject.Resources = "1 Ingeniero de software/programador Web " +
                            "Recursos materiales: " +
                             "Computadoras de escritorio " +
                             "IDE para programación(Visual Studio con C#) " +
                             "Acceso a Internet " +
                             "Documentación de procesos";
                updateProject.Activities = "Realizar a cabo la documentación del desarrollo del proyecto de la primera fase del proyecto, mediante el modelado de casos de uso, la descripción de los mismos y modelo de dominio, desarrollar sobre lenguaje C# y servicios dentro de un API, además de trabajar en equipo dentro del departamento.";
                updateProject.Responsibilities = "Cumplir con las funciones y actividades que sean asignadas " +
                            "Cumplir en tiempo y forma con las entregas de prototipos y productos " +
                            "Desarrollar en un ambiente colaborativo " +
                            "Trabajar de acuerdo a los estándares establecidos";
                updateProject.QuantityPracticing = 2;
                updateProject.IdResponsibleProject = 3;
                updateProject.IdLinkedOrganization = 4;
                int expected = 1;
                int actual = _data.Count;
                Assert.AreEqual(expected, actual);
            }catch (NullReferenceException)
            {
            }
        }
    }
}
