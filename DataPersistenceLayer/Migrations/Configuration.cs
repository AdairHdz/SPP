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
            context.States.AddOrUpdate(x => x.IdState,
                new State
                {
                    IdState = 1,
                    NameState = "Veracruz",
                    Cities = new List<City>
                    {
                        new City
                        {
                            IdCity = 1,
                            NameCity = "Xalapa",
                        },
                        new City
                        {
                            IdCity = 2,
                            NameCity = "Veracruz",
                        },
                        new City
                        {
                            IdCity = 3,
                            NameCity = "Coatepec",
                        },
                    }
                },
                new State
                {
                    IdState = 2,
                    NameState = "Jalisco",
                    Cities = new List<City>
                    {
                        new City
                        {
                            IdCity = 4,
                            NameCity = "Tequila",
                        },
                        new City
                        {
                            IdCity = 5,
                            NameCity = "Guadalajara",
                        },
                    }
                },
                new State
                {
                    IdState = 3,
                    NameState = "Nuevo León",
                    Cities = new List<City>
                    {
                        new City
                        {
                            IdCity = 6,
                            NameCity = "Monterrey",
                        },
                    }
                }
            );


            context.Sectors.AddOrUpdate(x => x.IdSector,
                new Sector
                {
                    IdSector = 1,
                    NameSector = "Económico"
                },
                new Sector
                {
                    IdSector = 2,
                    NameSector = "Industrial"
                },
                new Sector
                {
                    IdSector = 3,
                    NameSector = "Tecnológico"
                },
                new Sector
                {
                    IdSector = 4,
                    NameSector = "Educativo"
                }
            );

            context.Practicioners.AddOrUpdate(x => x.Enrollment,
                new Practicioner
                {
                    Enrollment = "S18012122",
                    Term = "Febrero - Julio 2021",
                    Credits = 300,
                    User = new User
                    {
                        IdUser = 1,
                        Name = "Adair Benjamin",
                        LastName = "Hernandez Ortiz",
                        Gender = Gender.MALE,
                        UserStatus = UserStatus.ACTIVE,
                        Email = "adairho16@gmail.com",
                        AlternateEmail = "adair_lis.uv@gmail.com",
                        PhoneNumber = "2281244285",
                        UserType = UserType.Practicioner,
                        Account = new Account
                        {
                            IdAccount = 1,
                            Username = "AdairHz",
                            Password = "$2a$08$MqktFO0I2aOa0cKYIuZQ2u/k3UXlev5EYKUuI9eB4MSuhxeubh0dq",
                            FirstLogin = false,
                            Salt = "$2a$08$MqktFO0I2aOa0cKYIuZQ2u",
                        }
                    }
                },
                new Practicioner
                {
                    Enrollment = "S18012123",
                    Term = "Febrero - Julio 2021",
                    Credits = 290,
                    User = new User
                    {
                        IdUser = 2,
                        Name = "Irving de Jesus",
                        LastName = "Lozada Rodriguez",
                        Gender = Gender.MALE,
                        UserStatus = UserStatus.ACTIVE,
                        Email = "irving_de_jesus@gmail.com",
                        AlternateEmail = "irvlozada26@gmail.com",
                        PhoneNumber = "2281973645",
                        UserType = UserType.Practicioner,
                        Account = new Account
                        {
                            IdAccount = 2,
                            Username = "IrvingLzd",
                            Password = "$2a$08$MqktFO0I2aOa0cKYIuZQ2u/k3UXlev5EYKUuI9eB4MSuhxeubh0dq",
                            FirstLogin = false,
                            Salt = "$2a$08$MqktFO0I2aOa0cKYIuZQ2u",
                        }
                    }
                },
                new Practicioner
                {
                    Enrollment = "S18012124",
                    Term = "Febrero - Julio 2021",
                    Credits = 289,
                    User = new User
                    {
                        IdUser = 3,
                        Name = "Eduardo Aldair",
                        LastName = "Hernandez Solis",
                        Gender = Gender.MALE,
                        UserStatus = UserStatus.ACTIVE,
                        Email = "aldair@gmail.com",
                        AlternateEmail = "eduardo@hotmail.com",
                        PhoneNumber = "2283178932",
                        UserType = UserType.Practicioner,
                        Account = new Account
                        {
                            IdAccount = 3,
                            Username = "AldairSls26",
                            Password = "$2a$08$MqktFO0I2aOa0cKYIuZQ2u/k3UXlev5EYKUuI9eB4MSuhxeubh0dq",
                            FirstLogin = false,
                            Salt = "$2a$08$MqktFO0I2aOa0cKYIuZQ2u",
                        }
                    }
                },
                new Practicioner
                {
                    Enrollment = "S18012125",
                    Term = "Febrero - Julio 2021",
                    Credits = 310,
                    User = new User
                    {
                        IdUser = 4,
                        Name = "Yair Emanuel",
                        LastName = "Dominguez Lopez",
                        Gender = Gender.MALE,
                        UserStatus = UserStatus.ACTIVE,
                        Email = "yair@gmail.com",
                        AlternateEmail = "emanuel@gmail.com",
                        PhoneNumber = "2287901243",
                        UserType = UserType.Practicioner,
                        Account = new Account
                        {
                            IdAccount = 4,
                            Username = "YairDomLo",
                            Password = "$2a$08$MqktFO0I2aOa0cKYIuZQ2u/k3UXlev5EYKUuI9eB4MSuhxeubh0dq",
                            FirstLogin = false,
                            Salt = "$2a$08$MqktFO0I2aOa0cKYIuZQ2u",
                        }
                    }
                }
            );

            context.Coordinators.AddOrUpdate(x => x.StaffNumber,
                new Coordinator
                {
                    StaffNumber = "1234567890",
                    RegistrationDate = DateTime.Now,
                    User = new User
                    {
                        IdUser = 5,
                        Name = "Alejandro Jafet",
                        LastName = "Rodriguez Maldonado",
                        Gender = Gender.MALE,
                        UserStatus = UserStatus.ACTIVE,
                        Email = "ale_jafet@gmail.com",
                        AlternateEmail = "maldonadoalefj@gmail.com",
                        PhoneNumber = "2280122134",
                        UserType = UserType.Coordinator,
                        Account = new Account
                        {
                            IdAccount = 5,
                            Username = "alejof",
                            Password = "$2a$08$MqktFO0I2aOa0cKYIuZQ2u/k3UXlev5EYKUuI9eB4MSuhxeubh0dq",
                            FirstLogin = false,
                            Salt = "$2a$08$MqktFO0I2aOa0cKYIuZQ2u",
                        }
                    }
                }
            );

            context.Teachers.AddOrUpdate(x => x.StaffNumber,
                new Teacher
                {
                    StaffNumber = "1122334455",
                    RegistrationDate = DateTime.Now,
                    User = new User
                    {
                        IdUser = 6,
                        Name = "Francisco",
                        LastName = "Portilla Texon",
                        Gender = Gender.MALE,
                        UserStatus = UserStatus.ACTIVE,
                        Email = "francisco@gmail.com",
                        AlternateEmail = "portillaTexon@gmail.com",
                        PhoneNumber = "2292784521",
                        UserType = UserType.Teacher,
                        Account = new Account
                        {
                            IdAccount = 6,
                            Username = "franciscotx",
                            Password = "$2a$08$MqktFO0I2aOa0cKYIuZQ2u/k3UXlev5EYKUuI9eB4MSuhxeubh0dq",
                            FirstLogin = false,
                            Salt = "$2a$08$MqktFO0I2aOa0cKYIuZQ2u",
                        }
                    }
                });

            context.Managers.AddOrUpdate(x => x.StaffNumber,
                new Manager
                {
                    StaffNumber = "0987654321",
                    User = new User
                    {
                        IdUser = 7,
                        Name = "Adrian",
                        LastName = "Cicero Lazaro",
                        Gender = Gender.MALE,
                        UserStatus = UserStatus.ACTIVE,
                        Email = "adriancicero@gmail.com",
                        AlternateEmail = "adrianlazaro@gmail.com",
                        PhoneNumber = "2283908876",
                        UserType = UserType.Manager,
                        Account = new Account
                        {
                            IdAccount = 7,
                            Username = "adriancl",
                            Password = "$2a$08$MqktFO0I2aOa0cKYIuZQ2u/k3UXlev5EYKUuI9eB4MSuhxeubh0dq",
                            FirstLogin = false,
                            Salt = "$2a$08$MqktFO0I2aOa0cKYIuZQ2u",
                        }
                    }
                });

            context.LinkedOrganizations.AddOrUpdate(x => x.IdLinkedOrganization,
                new LinkedOrganization
                {
                    IdLinkedOrganization = 1,
                    Name = "Dell",
                    DirectUsers = "Desconocido",
                    IndirectUsers = "Desconocido",
                    Email = "dell@business.com",
                    PhoneNumbers = new List<Phone>
                    {
                        new Phone
                        {
                            IdPhoneNumber = 1,
                            Extension = "521",
                            PhoneNumber = "2280977854"
                        },
                        new Phone
                        {
                            IdPhoneNumber = 2,
                            Extension = "521",
                            PhoneNumber = "2280977855"
                        }
                    },
                    Address = "Pedro Moreno #21",
                    IdCity = 1,
                    IdState = 1,
                    IdSector = 1,
                    LinkedOrganizationStatus = LinkedOrganizationStatus.ACTIVE,
                },
                new LinkedOrganization
                {
                    IdLinkedOrganization = 2,
                    Name = "IBM",
                    DirectUsers = "Comunidad tecnologica",
                    IndirectUsers = "Comundidad estudiantil",
                    Email = "ibm@business.com",
                    PhoneNumbers = new List<Phone>
                    {
                        new Phone
                        {
                            IdPhoneNumber = 3,
                            Extension = "521",
                            PhoneNumber = "2281760910"
                        },
                        new Phone
                        {
                            IdPhoneNumber = 4,
                            Extension = "521",
                            PhoneNumber = "2280977866"
                        }
                    },
                    Address = "Retorno Insurgentes #5",
                    IdCity = 2,
                    IdState = 1,
                    IdSector = 2,
                    LinkedOrganizationStatus = LinkedOrganizationStatus.ACTIVE,
                },
                new LinkedOrganization
                {
                    IdLinkedOrganization = 3,
                    Name = "Softech",
                    DirectUsers = "Comunidad estudiantil",
                    IndirectUsers = "Desconocido",
                    Email = "softech@business.com",
                    PhoneNumbers = new List<Phone>
                    {
                        new Phone
                        {
                            IdPhoneNumber = 5,
                            Extension = "553",
                            PhoneNumber = "2297167803"
                        },
                        new Phone
                        {
                            IdPhoneNumber = 6,
                            Extension = "521",
                            PhoneNumber = "2280977899"
                        },
                    },
                    Address = "Avenida Amado Nervo",
                    IdCity = 3,
                    IdState = 1,
                    IdSector = 3,
                    LinkedOrganizationStatus = LinkedOrganizationStatus.ACTIVE,
                },
                new LinkedOrganization
                {
                    IdLinkedOrganization = 4,
                    Name = "Google",
                    DirectUsers = "Desconocido",
                    IndirectUsers = "Desconocido",
                    Email = "google@business.com",
                    PhoneNumbers = new List<Phone>
                    {
                        new Phone
                        {
                            IdPhoneNumber = 7,
                            Extension = "555",
                            PhoneNumber = "5518905467"
                        },
                        new Phone
                        {
                            IdPhoneNumber = 8,
                            Extension = "521",
                            PhoneNumber = "2280977817"
                        },
                    },
                    Address = "Avenida Obrero Campesino #34",
                    IdCity = 4,
                    IdState = 2,
                    IdSector = 4,
                    LinkedOrganizationStatus = LinkedOrganizationStatus.ACTIVE,
                }
            );

            context.ResponsibleProjects.AddOrUpdate(x => x.IdResponsibleProject,
                new ResponsibleProject
                {
                    IdResponsibleProject = 1,
                    Name = "Hector",
                    LastName = "Leal",
                    EmailAddress = "hector@gmail.com",
                    Charge = "Ejecutivo"
                },
                new ResponsibleProject
                {
                    IdResponsibleProject = 2,
                    Name = "Ricardo",
                    LastName = "Anaya",
                    EmailAddress = "richi_anaya@gmail.com",
                    Charge = "CEO"
                },
                new ResponsibleProject
                {
                    IdResponsibleProject = 3,
                    Name = "Hector Ivan",
                    LastName = "Guzman Ramirez",
                    EmailAddress = "ivan_ramirez@gmail.com",
                    Charge = "Director"
                },
                new ResponsibleProject
                {
                    IdResponsibleProject = 4,
                    Name = "Jesus",
                    LastName = "Hernandez Perez",
                    EmailAddress = "jesusPerez2021@gmail.com",
                    Charge = "Asesor ejecutivo"
                }
            );

            context.Projects.AddOrUpdate(x => x.IdProject,
            new Project
            {
                IdProject = 1,
                NameProject = "Desarrollo de inventario",
                DaysHours = "Lunes a viernes de 7AM a 2PM",
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
                QuantityPracticingAssing = 0,
                Term = "Febbrero - Julio 2021",
                IdLinkedOrganization = 1,
                StaffNumberCoordinator = "1234567890",
                IdResponsibleProject = 1,
            },
            new Project
            {
                IdProject = 2,
                NameProject = "Sistema bibliotecario",
                DaysHours = "Lunes a viernes de 7AM a 2PM",
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
                QuantityPracticingAssing = 0,
                Term = "Febbrero - Julio 2021",
                IdLinkedOrganization = 2,
                StaffNumberCoordinator = "1234567890",
                IdResponsibleProject = 2
            },
            new Project
            {
                IdProject = 3,
                NameProject = "Gestion de lavanderia",
                DaysHours = "Lunes a viernes de 7AM a 2PM",
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
                QuantityPracticingAssing = 0,
                Term = "Febbrero - Julio 2021",
                IdLinkedOrganization = 3,
                StaffNumberCoordinator = "1234567890",
                IdResponsibleProject = 3,
            },
            new Project
            {
                IdProject = 4,
                NameProject = "Chatbot",
                DaysHours = "Lunes a viernes de 7AM a 2PM",
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
                QuantityPracticing = 3,
                QuantityPracticingAssing = 0,
                Term = "Febbrero - Julio 2021",
                IdLinkedOrganization = 4,
                StaffNumberCoordinator = "1234567890",
                IdResponsibleProject = 4
            });

        }
    }
}
