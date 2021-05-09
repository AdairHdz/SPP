using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;


namespace UnitTests.DataPersistenceLayerTests.ProjectTest
{
    [TestClass]
    public class ProjectRegistryTest
    {
        private UnitOfWork _unitOfWork;

        [TestInitialize]
        public void TestInitialize()
        {
            List<Project> _data = new List<Project>
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
                    DaysHours = "A acordar con el estudiante (en horario de oficina)",
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
        public void DetermineIfProjectAlreadyExists_Exists()
        {
            Project projectWithSameName = _unitOfWork.Projects.FindFirstOccurence(Project => Project.NameProject == "Sistema Académico");

            Assert.IsNotNull(projectWithSameName);
        }

        [TestMethod]
        public void RegisterProject_Exists()
        {
            List<Project> projects = new List<Project>();
            DbSet<Project> mockSet = DbContextMock.GetQueryableMockDbSet(projects, p => p.NameProject);
            ProfessionalPracticesContext mockContext = DbContextMock.GetContext(mockSet);
            UnitOfWork unitOfWork = DbContextMock.GetUnitOfWork(mockContext);
            Project newProject = new Project
            {
                NameProject = "Sistema Integral Académico",
                Description = "Desarrollar un Sistema Web que gestione los procesos académicos que realizan las diferentes áreas dentro de la Universidad Veracruzana dentro de un mismo portal.",
                ObjectiveGeneral = "Optimizar los procesos de consulta y seguimiento de los académicos.",
                ObjectiveImmediate = "Revisión, análisis y documentación de requerimientos académicos con las áreas involucradas durante este desarrollo. " +
                                            "Revisión y análisis de la arquitectura." +
                                            "Desarrollo de prototipo.",
                ObjectiveMediate = "Modificación de documentación y Modificación de prototipos.",
                Methodology = "Proceso de desarrollo iterativo y Design Sprint, SCRUM",
                Resources = "1 Ingeniero de software/programador Web " +
                        "Recursos materiales: " +
                         "Computadoras de escritorio " +
                         "IDE para programación(Visual Studio con C#) " +
                         "Acceso a Internet " +
                         "Documentación de procesos",
                Activities = "Realizar a cabo la documentación del desarrollo del proyecto de la primera fase del proyecto, mediante el modelado de casos de uso, la descripción de los mismos y modelo de dominio, desarrollar sobre lenguaje C# y servicios dentro de un API, además de trabajar en equipo dentro del departamento.",
                Responsibilities = "Cumplir con las funciones y actividades que sean asignadas " +
                        "Cumplir en tiempo y forma con las entregas de prototipos y productos " +
                        "Desarrollar en un ambiente colaborativo " +
                        "Trabajar de acuerdo a los estándares establecidos",
                DaysHours = "A acordar con el estudiante (en horario de oficina)",
                Duration = 480,
                Term = "FEBRERO-JULIO 2021",
                QuantityPracticing = 3,
                StaffNumberCoordinator = "1515151"
            };
            unitOfWork.Projects.Add(newProject);
            int expected = 1;
            int actual = projects.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RegisterSchedulingActivities_Exists()
        {
            List<SchedulingActivity> schedulingActivities = new List<SchedulingActivity>() {
                new SchedulingActivity
                {
                    Month = "Febrero",
                    Activity = "Análisis de requerimientos, recopilación de información, documentación y revisión de procedimientos.Capacitación en el IDE de desarrollo y metodología.",
                    IdProject = 3
                },
                new SchedulingActivity
                {
                    Month = "Mayo",
                    Activity = "Pruebas y ajustes",
                    IdProject = 3
                }
            };
            DbSet<SchedulingActivity> mockSet = DbContextMock.GetQueryableMockDbSet(schedulingActivities, s => s.IdSchedulingActivity);
            ProfessionalPracticesContext mockContext = DbContextMock.GetContext(mockSet);
            UnitOfWork unitOfWork = DbContextMock.GetUnitOfWork(mockContext);
            unitOfWork.SchedulingActivities.AddRange(schedulingActivities);
            int expected = 4;
            int actual = schedulingActivities.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}
