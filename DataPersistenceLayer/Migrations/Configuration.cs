using System.Data.Entity.Migrations;

namespace DataPersistenceLayer.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<ProfessionalPracticesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ProfessionalPracticesContext context)
        {
            //try
            //{
            //    context.Cities.AddOrUpdate(x => x.IdCity,
            //        new City { IdCity = 1, NameCity = "Coatepec" },
            //        new City { IdCity = 2, NameCity = "Xalapa" },
            //        new City { IdCity = 3, NameCity = "Veracruz" }
            //    );

            //    context.States.AddOrUpdate(x => x.IdState,
            //        new State { IdState = 1, NameState = "Veracruz" }
            //    );

            //    context.Sectors.AddOrUpdate(x => x.IdSector,
            //        new Sector { IdSector = 1, NameSector = "Económico" },
            //        new Sector { IdSector = 2, NameSector = "Industrial" }
            //    );

            //    context.UserStatuses.AddOrUpdate(x => x.UserStatusId,
            //        new UserStatus { UserStatusId = 1, Status = "Aceptado" }
            //    );

            //    context.Periods.AddOrUpdate(x => x.IdPeriod,
            //        new Period { IdPeriod = 1, PeriodName = "Febrero-Julio" },
            //        new Period { IdPeriod = 2, PeriodName = "Agosto-Enero" }
            //    );

            //    context.Positions.AddOrUpdate(x => x.IdPosition,
            //        new Position { IdPosition = 1, NamePosition = "CEO" },
            //        new Position { IdPosition = 2, NamePosition = "Analista de negocios" }
            //    );

            //    context.Genders.AddOrUpdate(x => x.IdGender,
            //        new Gender { IdGender = 1, GenderName = "Hombre" },
            //        new Gender { IdGender = 2, GenderName = "Mujer" }
            //    );

            //    context.ProjectStatuses.AddOrUpdate(x => x.IdProjectStatus,
            //        new ProjectStatus { IdProjectStatus = 1, Status = "En proceso" }
            //    );

            //    context.Turns.AddOrUpdate(x => x.IdTurn,
            //        new Turn { IdTurn = 1, TurnName = "Matutino" },
            //        new Turn { IdTurn = 2, TurnName = "Vespertino" }
            //    );

            //    context.RequestStatuses.AddOrUpdate(x => x.IdRequestStatus,
            //        new RequestStatus { IdRequestStatus = 1, Status = "Aprobada" },
            //        new RequestStatus { IdRequestStatus = 2, Status = "Rechazada" }
            //    );

            //    context.LinkedOrganizations.AddOrUpdate(x => x.IdLinkedOrganization,
            //        new LinkedOrganization
            //        {
            //            IdLinkedOrganization = 1,
            //            Name = "Softech",
            //            DirectUsers = 5,
            //            IndirectUsers = 100,
            //            Email = "softech@gmail.com",
            //            Address = "Av. Xalapa #21",
            //            IdCity = 1,
            //            IdState = 1,
            //            IdSector = 1
            //        }
            //    );
                
            //    context.Users.AddOrUpdate(x => x.IdUser,
            //        new User
            //        {
            //            IdUser = 1,
            //            Name = "Adair Benjamín",
            //            LastName = "Hernández Ortiz",
            //            IdGender = 1,
            //            IdStatus = 1,
            //            Email = "zS18012122@uv.com.mx",
            //            AlternateEmail = "adairho16@gmail.com",
            //            PhoneNumber = "2281244285"
            //        },
            //         new User
            //         {
            //             IdUser = 2,
            //             Name = "Yazmín Alejandra",
            //             LastName = "Luna Herrera",
            //             IdGender = 2,
            //             IdStatus = 1,
            //             Email = "zS18012067@uv.com.mx",
            //             AlternateEmail = "luna.yazmin@gmail.com",
            //             PhoneNumber = "2281249071"
            //         },
            //         new User
            //         {
            //             IdUser = 3,
            //             Name = "Ángel José",
            //             LastName = "Calderón Ortega",
            //             IdGender = 1,
            //             IdStatus = 1,
            //             Email = "zS18012001@uv.com.mx",
            //             AlternateEmail = "angel@gmail.com",
            //             PhoneNumber = "2281091542"
            //         },
            //         new User
            //         {
            //             IdUser = 4,
            //             Name = "Martha Miroslava",
            //             LastName = "Ortiz López",
            //             IdGender = 2,
            //             IdStatus = 1,
            //             Email = "zS18012156@uv.com.mx",
            //             AlternateEmail = "martha_ortia@gmail.com",
            //             PhoneNumber = "2281091456"
            //         }
            //    );

            //    context.LoginAccounts.AddOrUpdate(x => x.IdLoginAccount,
            //        new Account
            //        {
            //            IdLoginAccount = 1,
            //            Username = "YazLuna",
            //            Password = "adadsnvcldsfk474",
            //            FirstLogin = false,
            //            IdUser = 2
            //        });

            //    context.Coordinators.AddOrUpdate(x => x.StaffNumber,
            //        new Coordinator
            //        {
            //            StaffNumber = "ABC123",
            //            RegistrationDate = DateTime.Today,
            //            IdUser = 2
            //        }
            //    );

            //    context.Teachers.AddOrUpdate(x => x.StaffNumber,
            //        new Teacher
            //        {
            //            StaffNumber = "DEF456",
            //            RegistrationDate = DateTime.Now,
            //            IdTurn = 1,
            //            IdUser = 3
            //        }
            //    );

            //    context.ResponsibleProjects.AddOrUpdate(x => x.IdResponsibleProject,
            //        new ResponsibleProject
            //        {
            //            IdResponsibleProject = 1,
            //            Name = "Carlos",
            //            LastName = "Esparza Rodriguez",
            //            EmailAddress = "carlos.esparza@gmail.com",
            //            IdPosition = 1
            //        }
            //    );

            //    context.Projects.AddOrUpdate(x => x.IdProject,
            //        new Project
            //        {
            //            IdProject = 1,
            //            NameProject = "Desarrollo de inventario",
            //            Description = "Se debe desarrollar un inventario para una red inmobiliaria",
            //            ObjectiveGeneral = "El objetivo general",
            //            ObjectiveImmediate = "El objetivo inmediato",
            //            ObjectiveMediate = "El objetivo mediato",
            //            Methodology = "SCRUM",
            //            Resources = "Recursos",
            //            IdProjectStatus = 1,
            //            Duration = 480,
            //            Activities = "Actividades",
            //            Responsibilities = "Responsabilidades",
            //            QuantityPracticing = 2,
            //            IdLinkedOrganization = 1,
            //            StaffNumberCoordinator = "ABC123",
            //            IdResponsibleProject = 1
            //        }
            //    );

            //    context.Practicings.AddOrUpdate(x => x.Enrollment,
            //        new Practicioner
            //        {
            //            Enrollment = "S18012122",
            //            IdTurn = 1,
            //            IdPeriod = 1,
            //            IdUser = 1,
            //            IdProject = 1
            //        }
            //    );

            //    context.Activities.AddOrUpdate(x => x.IdActivity,
            //        new Activity
            //        {
            //            IdActivity = 1,
            //            Value = 5,
            //            Name = "Actividad 1",
            //            DeliverDate = DateTime.Now,
            //            EnrollmentPracticing = "S18012122",
            //            StaffNumber = "DEF456"
            //        }
            //    );

            //    context.Reports.AddOrUpdate(x => x.IdReport,
            //        new Report
            //        {
            //            IdReport = 1,
            //            Activities = "Actividades realizadas",
            //            EnrollmentPracticing = "S18012122"
            //        },
            //        new Report
            //        {
            //            IdReport = 2,
            //            Activities = "Actividades realizadas",
            //            EnrollmentPracticing = "S18012122"
            //        }
            //    );

            //    context.ReportPartials.AddOrUpdate(x => x.IdReportParcial,
            //        new PartialReport
            //        {
            //            IdReportParcial = 2,
            //            NumberReport = 1,
            //            ResultsObtained = "Buenos resultados",
            //            HoursCovered = 100,
            //            IdReport = 2
            //        }
            //    );
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            Console.WriteLine(validationError.PropertyName, validationError.ErrorMessage);

            //        }
            //    }
            //}


        }
    }
}
