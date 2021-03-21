namespace DataPersistenceLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        IdActivity = c.Int(nullable: false, identity: true),
                        Value = c.Int(nullable: false),
                        Description = c.String(maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 25),
                        DeliverDate = c.DateTime(nullable: false),
                        Document = c.String(maxLength: 255),
                        EnrollmentPracticing = c.String(nullable: false, maxLength: 10),
                        IdStaffNumberTeacher = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.IdActivity)
                .ForeignKey("dbo.Practicings", t => t.EnrollmentPracticing, cascadeDelete: true)
                .ForeignKey("dbo.Teachers", t => t.IdStaffNumberTeacher)
                .Index(t => t.EnrollmentPracticing)
                .Index(t => t.IdStaffNumberTeacher);
            
            CreateTable(
                "dbo.Practicings",
                c => new
                    {
                        Enrollment = c.String(nullable: false, maxLength: 10),
                        IdTurn = c.Int(nullable: false),
                        IdPeriod = c.Int(nullable: false),
                        IdUser = c.Int(nullable: false),
                        IdProject = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Enrollment)
                .ForeignKey("dbo.Periods", t => t.IdPeriod, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.IdProject, cascadeDelete: true)
                .ForeignKey("dbo.Turns", t => t.IdTurn, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdTurn)
                .Index(t => t.IdPeriod)
                .Index(t => t.IdUser)
                .Index(t => t.IdProject);
            
            CreateTable(
                "dbo.Periods",
                c => new
                    {
                        IdPeriod = c.Int(nullable: false, identity: true),
                        PeriodName = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.IdPeriod);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        IdProject = c.Int(nullable: false, identity: true),
                        NameProject = c.String(nullable: false, maxLength: 25),
                        Description = c.String(nullable: false, maxLength: 100),
                        ObjectiveGeneral = c.String(nullable: false, maxLength: 100),
                        ObjectiveImmediate = c.String(nullable: false, maxLength: 100),
                        ObjectiveMediate = c.String(nullable: false, maxLength: 100),
                        Methodology = c.String(nullable: false, maxLength: 15),
                        Resources = c.String(nullable: false, maxLength: 25),
                        IdProjectStatus = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                        Activities = c.String(nullable: false),
                        Responsibilities = c.String(nullable: false),
                        QuantityPracticing = c.Int(nullable: false),
                        IdLinkedOrganization = c.Int(nullable: false),
                        StaffNumberCoordinator = c.String(maxLength: 20),
                        IdResponsibleProject = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdProject)
                .ForeignKey("dbo.Coordinators", t => t.StaffNumberCoordinator)
                .ForeignKey("dbo.LinkedOrganizations", t => t.IdLinkedOrganization, cascadeDelete: true)
                .ForeignKey("dbo.ResponsibleProjects", t => t.IdResponsibleProject, cascadeDelete: true)
                .ForeignKey("dbo.ProjectStatus", t => t.IdProjectStatus, cascadeDelete: true)
                .Index(t => t.IdProjectStatus)
                .Index(t => t.IdLinkedOrganization)
                .Index(t => t.StaffNumberCoordinator)
                .Index(t => t.IdResponsibleProject);
            
            CreateTable(
                "dbo.Coordinators",
                c => new
                    {
                        StaffNumber = c.String(nullable: false, maxLength: 20),
                        RegistrationDate = c.DateTime(nullable: false),
                        DischargeDate = c.DateTime(nullable: false),
                        IdUser = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StaffNumber)
                .ForeignKey("dbo.Users", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdUser);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        IdUser = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        LastName = c.String(nullable: false, maxLength: 30),
                        IdGender = c.Int(nullable: false),
                        IdStatus = c.Int(nullable: false),
                        Email = c.String(nullable: false, maxLength: 254),
                        AlternateEmail = c.String(maxLength: 254),
                        PhoneNumber = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.IdUser)
                .ForeignKey("dbo.Genders", t => t.IdGender, cascadeDelete: true)
                .ForeignKey("dbo.UserStatus", t => t.IdStatus, cascadeDelete: true)
                .Index(t => t.IdGender)
                .Index(t => t.IdStatus);
            
            CreateTable(
                "dbo.Genders",
                c => new
                    {
                        IdGender = c.Int(nullable: false, identity: true),
                        GenderName = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.IdGender);
            
            CreateTable(
                "dbo.UserStatus",
                c => new
                    {
                        UserStatusId = c.Int(nullable: false, identity: true),
                        Status = c.String(nullable: false, maxLength: 15),
                    })
                .PrimaryKey(t => t.UserStatusId);
            
            CreateTable(
                "dbo.LinkedOrganizations",
                c => new
                    {
                        IdLinkedOrganization = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        DirectUsers = c.Int(nullable: false),
                        IndirectUsers = c.Int(nullable: false),
                        Email = c.String(nullable: false, maxLength: 254),
                        //PhoneNumber = c.String(nullable: false, maxLength: 15),
                        Address = c.String(nullable: false, maxLength: 15),
                        IdCity = c.Int(nullable: false),
                        IdState = c.Int(nullable: false),
                        IdSector = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdLinkedOrganization)
                .ForeignKey("dbo.Cities", t => t.IdCity, cascadeDelete: true)
                .ForeignKey("dbo.Sectors", t => t.IdSector, cascadeDelete: true)
                .ForeignKey("dbo.States", t => t.IdState, cascadeDelete: true)
                .Index(t => t.IdCity)
                .Index(t => t.IdState)
                .Index(t => t.IdSector);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        IdCity = c.Int(nullable: false, identity: true),
                        NameCity = c.String(maxLength: 25),
                    })
                .PrimaryKey(t => t.IdCity);
            
            CreateTable(
                "dbo.Sectors",
                c => new
                    {
                        IdSector = c.Int(nullable: false, identity: true),
                        NameSector = c.String(maxLength: 25),
                    })
                .PrimaryKey(t => t.IdSector);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        IdState = c.Int(nullable: false, identity: true),
                        NameState = c.String(maxLength: 25),
                    })
                .PrimaryKey(t => t.IdState);
            
            CreateTable(
                "dbo.ResponsibleProjects",
                c => new
                    {
                        IdResponsibleProject = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        EmailAddress = c.String(nullable: false, maxLength: 254),
                        IdPosition = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdResponsibleProject)
                .ForeignKey("dbo.Positions", t => t.IdPosition, cascadeDelete: true)
                .Index(t => t.IdPosition);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        IdPosition = c.Int(nullable: false, identity: true),
                        NamePosition = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.IdPosition);
            
            CreateTable(
                "dbo.ProjectStatus",
                c => new
                    {
                        IdProjectStatus = c.Int(nullable: false, identity: true),
                        Status = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.IdProjectStatus);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        IdReport = c.Int(nullable: false, identity: true),
                        Activities = c.String(nullable: false),
                        Score = c.Int(nullable: false),
                        CompletionDate = c.DateTime(nullable: false),
                        DeliverDate = c.DateTime(nullable: false),
                        Document = c.String(),
                        EnrollmentPracticing = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.IdReport)
                .ForeignKey("dbo.Practicings", t => t.EnrollmentPracticing, cascadeDelete: true)
                .Index(t => t.EnrollmentPracticing);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        IdRequest = c.Int(nullable: false, identity: true),
                        Enrollment = c.String(maxLength: 10),
                        IdProject = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdRequest)
                .ForeignKey("dbo.Practicings", t => t.Enrollment)
                .ForeignKey("dbo.Projects", t => t.IdProject, cascadeDelete: true)
                .ForeignKey("dbo.RequestStatus", t => t.Status, cascadeDelete: true)
                .Index(t => t.Enrollment)
                .Index(t => t.IdProject)
                .Index(t => t.Status);
            
            CreateTable(
                "dbo.RequestStatus",
                c => new
                    {
                        IdRequestStatus = c.Int(nullable: false, identity: true),
                        Status = c.String(nullable: false, maxLength: 15),
                    })
                .PrimaryKey(t => t.IdRequestStatus);
            
            CreateTable(
                "dbo.Turns",
                c => new
                    {
                        IdTurn = c.Int(nullable: false, identity: true),
                        TurnName = c.String(nullable: false, maxLength: 15),
                    })
                .PrimaryKey(t => t.IdTurn);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        StaffNumber = c.String(nullable: false, maxLength: 20),
                        RegistrationDate = c.DateTime(nullable: false),
                        DischargeDate = c.DateTime(nullable: false),
                        IdTurn = c.Int(nullable: false),
                        IdUser = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StaffNumber)
                .ForeignKey("dbo.Turns", t => t.IdTurn, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdTurn)
                .Index(t => t.IdUser);
            
            CreateTable(
                "dbo.ReportPartials",
                c => new
                    {
                        IdReportParcial = c.Int(nullable: false, identity: true),
                        NumberReport = c.Int(nullable: false),
                        ResultsObtained = c.String(nullable: false, maxLength: 50),
                        HoursCovered = c.Int(nullable: false),
                        Observations = c.String(maxLength: 100),
                        IdReport = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdReportParcial)
                .ForeignKey("dbo.Reports", t => t.IdReport, cascadeDelete: true)
                .Index(t => t.IdReport);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReportPartials", "IdReport", "dbo.Reports");
            DropForeignKey("dbo.Activities", "IdStaffNumberTeacher", "dbo.Teachers");
            DropForeignKey("dbo.Teachers", "IdUser", "dbo.Users");
            DropForeignKey("dbo.Teachers", "IdTurn", "dbo.Turns");
            DropForeignKey("dbo.Practicings", "IdUser", "dbo.Users");
            DropForeignKey("dbo.Practicings", "IdTurn", "dbo.Turns");
            DropForeignKey("dbo.Requests", "Status", "dbo.RequestStatus");
            DropForeignKey("dbo.Requests", "IdProject", "dbo.Projects");
            DropForeignKey("dbo.Requests", "Enrollment", "dbo.Practicings");
            DropForeignKey("dbo.Reports", "EnrollmentPracticing", "dbo.Practicings");
            DropForeignKey("dbo.Practicings", "IdProject", "dbo.Projects");
            DropForeignKey("dbo.Projects", "IdProjectStatus", "dbo.ProjectStatus");
            DropForeignKey("dbo.Projects", "IdResponsibleProject", "dbo.ResponsibleProjects");
            DropForeignKey("dbo.ResponsibleProjects", "IdPosition", "dbo.Positions");
            DropForeignKey("dbo.LinkedOrganizations", "IdState", "dbo.States");
            DropForeignKey("dbo.LinkedOrganizations", "IdSector", "dbo.Sectors");
            DropForeignKey("dbo.Projects", "IdLinkedOrganization", "dbo.LinkedOrganizations");
            DropForeignKey("dbo.LinkedOrganizations", "IdCity", "dbo.Cities");
            DropForeignKey("dbo.Projects", "StaffNumberCoordinator", "dbo.Coordinators");
            DropForeignKey("dbo.Coordinators", "IdUser", "dbo.Users");
            DropForeignKey("dbo.Users", "IdStatus", "dbo.UserStatus");
            DropForeignKey("dbo.Users", "IdGender", "dbo.Genders");
            DropForeignKey("dbo.Practicings", "IdPeriod", "dbo.Periods");
            DropForeignKey("dbo.Activities", "EnrollmentPracticing", "dbo.Practicings");
            DropIndex("dbo.ReportPartials", new[] { "IdReport" });
            DropIndex("dbo.Teachers", new[] { "IdUser" });
            DropIndex("dbo.Teachers", new[] { "IdTurn" });
            DropIndex("dbo.Requests", new[] { "Status" });
            DropIndex("dbo.Requests", new[] { "IdProject" });
            DropIndex("dbo.Requests", new[] { "Enrollment" });
            DropIndex("dbo.Reports", new[] { "EnrollmentPracticing" });
            DropIndex("dbo.ResponsibleProjects", new[] { "IdPosition" });
            DropIndex("dbo.LinkedOrganizations", new[] { "IdSector" });
            DropIndex("dbo.LinkedOrganizations", new[] { "IdState" });
            DropIndex("dbo.LinkedOrganizations", new[] { "IdCity" });
            DropIndex("dbo.Users", new[] { "IdStatus" });
            DropIndex("dbo.Users", new[] { "IdGender" });
            DropIndex("dbo.Coordinators", new[] { "IdUser" });
            DropIndex("dbo.Projects", new[] { "IdResponsibleProject" });
            DropIndex("dbo.Projects", new[] { "StaffNumberCoordinator" });
            DropIndex("dbo.Projects", new[] { "IdLinkedOrganization" });
            DropIndex("dbo.Projects", new[] { "IdProjectStatus" });
            DropIndex("dbo.Practicings", new[] { "IdProject" });
            DropIndex("dbo.Practicings", new[] { "IdUser" });
            DropIndex("dbo.Practicings", new[] { "IdPeriod" });
            DropIndex("dbo.Practicings", new[] { "IdTurn" });
            DropIndex("dbo.Activities", new[] { "IdStaffNumberTeacher" });
            DropIndex("dbo.Activities", new[] { "EnrollmentPracticing" });
            DropTable("dbo.ReportPartials");
            DropTable("dbo.Teachers");
            DropTable("dbo.Turns");
            DropTable("dbo.RequestStatus");
            DropTable("dbo.Requests");
            DropTable("dbo.Reports");
            DropTable("dbo.ProjectStatus");
            DropTable("dbo.Positions");
            DropTable("dbo.ResponsibleProjects");
            DropTable("dbo.States");
            DropTable("dbo.Sectors");
            DropTable("dbo.Cities");
            DropTable("dbo.LinkedOrganizations");
            DropTable("dbo.UserStatus");
            DropTable("dbo.Genders");
            DropTable("dbo.Users");
            DropTable("dbo.Coordinators");
            DropTable("dbo.Projects");
            DropTable("dbo.Periods");
            DropTable("dbo.Practicings");
            DropTable("dbo.Activities");
        }
    }
}
