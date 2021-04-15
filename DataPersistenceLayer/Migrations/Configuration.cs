using DataPersistenceLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;

namespace DataPersistenceLayer.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<ProfessionalPracticesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ProfessionalPracticesContext context)
        {
            //context.States.AddOrUpdate(x => x.IdState,
            //    new State
            //    {
            //        IdState = 1,
            //        NameState = "Veracruz",
            //        Cities = new List<City>
            //        {
            //            new City
            //            {
            //                IdCity = 1,
            //                NameCity = "Xalapa",
            //            },
            //            new City
            //            {
            //                IdCity = 2,
            //                NameCity = "Veracruz",
            //            },
            //            new City
            //            {
            //                IdCity = 3,
            //                NameCity = "Coatepec",
            //            },
            //        }
            //    },
            //    new State
            //    {
            //        IdState = 2,
            //        NameState = "Jalisco",
            //        Cities = new List<City>
            //        {
            //            new City
            //            {
            //                IdCity = 4,
            //                NameCity = "Tequila",
            //            },
            //            new City
            //            {
            //                IdCity = 5,
            //                NameCity = "Guadalajara",
            //            },
            //        }
            //    },
            //    new State
            //    {
            //        IdState = 3,
            //        NameState = "Nuevo León",
            //        Cities = new List<City>
            //        {
            //            new City
            //            {
            //                IdCity = 6,
            //                NameCity = "Monterrey",
            //            },
            //        }
            //    }
            //);


            //context.Sectors.AddOrUpdate(x => x.IdSector,
            //    new Sector
            //    {
            //        IdSector = 1,
            //        NameSector = "Económico"
            //    },
            //    new Sector
            //    {
            //        IdSector = 2,
            //        NameSector = "Industrial"
            //    }
            //);

            //context.Accounts.AddOrUpdate(x => x.IdAccount,
            //    new Account
            //    {
            //        IdAccount = 1,
            //        Username = "AdairHz",
            //        Password = "$2a$08$Rdx7XaN.UA7dKgNoX0GKdeRyXTiIHsnSW2zYDuzPaWi9EwlKF5FEe",
            //        FirstLogin = true,
            //        Salt = "$2a",
            //    },
            //    new Account
            //    {
            //        IdAccount = 2,
            //        Username = "EduardoSl22",
            //        Password = "$2a$08$Rdx7XaN.UA7dKgNoX0GKdeRyXTiIHsnSW2zYDuzPaWi9EwlKF5FEe",
            //        FirstLogin = true,
            //        Salt = "$2a",
            //    }
            //);

            //context.Users.AddOrUpdate(x => x.IdUser,
            //    new User
            //    {
            //        IdUser = 1,
            //        Name = "Adair Benjamin",
            //        LastName = "Hernandez Ortiz",
            //        Gender = Gender.MALE,
            //        UserStatus = UserStatus.ACTIVE,
            //        Email = "adairho16@gmail.com",
            //        AlternateEmail = "adair_lis.uv@gmail.com",
            //        PhoneNumber = "2281244285",
            //        UserType = UserType.Practicioner,
            //        IdAccount = 1,
            //    },
            //    new User
            //    {
            //        IdUser = 2,
            //        Name = "Eduardo Aldair",
            //        LastName = "Hernandez Solis",
            //        Gender = Gender.MALE,
            //        UserStatus = UserStatus.ACTIVE,
            //        Email = "eduardo@gmail.com",
            //        AlternateEmail = "edr_solis@gmail.com",
            //        PhoneNumber = "2283908831",
            //        UserType = UserType.Coordinator,
            //        IdAccount = 2,
            //    }
            //);

            //context.Practicioners.AddOrUpdate(x => x.Enrollment,
            //    new Practicioner
            //    {
            //        Enrollment = "S18012122",
            //        Term = "Febrero - Julio 2021",
            //        Credits = 250,
            //        IdUser = 1,
            //        //IdGrupo
            //    }
            //);

            //    context.Coordinators.AddOrUpdate(x => x.StaffNumber,
            //        new Coordinator
            //        {
            //            StaffNumber = "ABC123",
            //            RegistrationDate = DateTime.Now,
            //            IdUser = 2
            //        }
            //    );

            //    context.LinkedOrganizations.AddOrUpdate(x => x.IdLinkedOrganization,
            //        new LinkedOrganization
            //        {
            //            IdLinkedOrganization = 1,
            //            Name = "Dell",
            //            DirectUsers = "Desconocido",
            //            IndirectUsers = "Desconocido",
            //            Email = "bussiness_dell@gmail.com",
            //            PhoneNumbers = new List<Phone>
            //            {
            //                new Phone
            //                {
            //                    IdPhoneNumber = 1,
            //                    Extension = "521",
            //                    PhoneNumber = "2280977854"
            //                }
            //            },
            //            Address = "Enrique Segoviano",
            //            IdCity = 1,
            //            IdState = 1,
            //            IdSector = 1,
            //            LinkedOrganizationStatus = LinkedOrganizationStatus.ACTIVE,
            //        }
            //    );

            //    context.ResponsibleProjects.AddOrUpdate(x => x.IdResponsibleProject,
            //        new ResponsibleProject
            //        {
            //            IdResponsibleProject = 1,
            //            Name = "Hector",
            //            LastName = "Leal",
            //            EmailAddress = "hector@gmail.com",
            //            Charge = "Ejecutivo"
            //        }
            //    );

            //    context.Projects.AddOrUpdate(x => x.IdProject,
            //    new Project
            //    {
            //        IdProject = 1,
            //        NameProject = "Desarrollo de inventario",
            //        DaysHours = "Lunes a viernes de 7AM a 2PM",
            //        Description = "Se debe desarrollar un inventario para una red inmobiliaria",
            //        ObjectiveGeneral = "El objetivo general",
            //        ObjectiveImmediate = "El objetivo inmediato",
            //        ObjectiveMediate = "El objetivo mediato",
            //        Methodology = "SCRUM",
            //        Resources = "Recursos",
            //        Status = ProjectStatus.ACTIVE,
            //        Duration = 480,
            //        Activities = "Actividades",
            //        Responsibilities = "Responsabilidades",
            //        QuantityPracticing = 2,
            //        IdLinkedOrganization = 1,
            //        StaffNumberCoordinator = "ABC123",
            //        IdResponsibleProject = 1
            //    },
            //    new Project
            //    {
            //        IdProject = 2,
            //        NameProject = "Sistema bibliotecario",
            //        DaysHours = "Lunes a viernes de 7AM a 2PM",
            //        Description = "Se debe desarrollar un sistema bibliotecario",
            //        ObjectiveGeneral = "El objetivo general",
            //        ObjectiveImmediate = "El objetivo inmediato",
            //        ObjectiveMediate = "El objetivo mediato",
            //        Methodology = "SCRUM",
            //        Resources = "Recursos",
            //        Status = ProjectStatus.ACTIVE,
            //        Duration = 360,
            //        Activities = "Actividades",
            //        Responsibilities = "Responsabilidades",
            //        QuantityPracticing = 1,
            //        IdLinkedOrganization = 1,
            //        StaffNumberCoordinator = "ABC123",
            //        IdResponsibleProject = 1
            //    }
            //);

            //context.OfficeOfAcceptances.AddOrUpdate(x => x.IdOfAcceptance,
            //    new OfficeOfAcceptance
            //    {
            //        IdOfAcceptance = 1,
            //        DateOfAcceptance = DateTime.Now,
            //        RouteSave = ""
            //    }
            // );

            //context.SchedulingActivities.AddOrUpdate(x => x.IdSchedulingActivity,
            //    new SchedulingActivity
            //    {
            //        IdSchedulingActivity = 1,
            //        Activity = "Actividad 1",
            //        Month = "Enero",
            //        IdProject = 1,
            //    },
            //    new SchedulingActivity
            //    {
            //        IdSchedulingActivity = 1,
            //        Activity = "Actividad 2",
            //        Month = "Febrero",
            //        IdProject = 1,
            //    }
            //);

            //context.Assignments.AddOrUpdate(x => x.IdAssignment,
            //     new Assignment
            //     {
            //         IdAssignment = 1,
            //         StartTerm = "Febrero - Julio 2021",
            //         CompletionTerm = "Agosto - Diciembre 2021",
            //         DateAssignment = DateTime.Now,
            //         Status = AssignmentStatus.Assigned,
            //         IdProject = 1,
            //         Enrollment = "S18012122",
            //         IdOfficeOfAcceptance = 1
            //     }
            // );


        }
    }
}
