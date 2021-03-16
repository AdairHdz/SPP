namespace DataPersistenceLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseRestore : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        IdAccount = c.Int(nullable: false, identity: true),
                        Username = c.String(maxLength: 50),
                        Password = c.String(maxLength: 120),
                        FirstLogin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdAccount);
            
            CreateTable(
                "dbo.ActivityMades",
                c => new
                    {
                        IdActivity = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PlannedWeek = c.String(),
                        PlannedMonth = c.String(),
                        RealMonth = c.String(),
                        RealWeek = c.String(),
                        IdPartialReport = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdActivity)
                .ForeignKey("dbo.PartialReports", t => t.IdPartialReport, cascadeDelete: true)
                .Index(t => t.IdPartialReport);
            
            CreateTable(
                "dbo.PartialReports",
                c => new
                    {
                        IdParcialReport = c.Int(nullable: false, identity: true),
                        NumberReport = c.Int(nullable: false),
                        ResultsObtained = c.String(nullable: false, maxLength: 254),
                        HoursCovered = c.Int(nullable: false),
                        Observations = c.String(maxLength: 254),
                        DeliveryDate = c.DateTime(),
                        IdProject = c.Int(nullable: false),
                        Enrollment = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.IdParcialReport)
                .ForeignKey("dbo.Practicioners", t => t.Enrollment)
                .ForeignKey("dbo.Projects", t => t.IdProject, cascadeDelete: true)
                .Index(t => t.IdProject)
                .Index(t => t.Enrollment);
            
            CreateTable(
                "dbo.Practicioners",
                c => new
                    {
                        Enrollment = c.String(nullable: false, maxLength: 10),
                        Term = c.String(),
                        Credits = c.Int(nullable: false),
                        IdUser = c.Int(nullable: false),
                        Nrc = c.String(maxLength: 5),
                    })
                .PrimaryKey(t => t.Enrollment)
                .ForeignKey("dbo.Groups", t => t.Nrc)
                .ForeignKey("dbo.Users", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdUser)
                .Index(t => t.Nrc);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Nrc = c.String(nullable: false, maxLength: 5),
                        GroupStatus = c.Int(nullable: false),
                        Term = c.String(),
                    })
                .PrimaryKey(t => t.Nrc);
            
            CreateTable(
                "dbo.RequestProjects",
                c => new
                    {
                        IdRequestProject = c.Int(nullable: false, identity: true),
                        RequestDate = c.DateTime(nullable: false),
                        IdProject = c.Int(nullable: false),
                        Enrollment = c.String(maxLength: 10),
                        RequestStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdRequestProject)
                .ForeignKey("dbo.Practicioners", t => t.Enrollment)
                .ForeignKey("dbo.Projects", t => t.IdProject, cascadeDelete: true)
                .Index(t => t.IdProject)
                .Index(t => t.Enrollment);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        IdProject = c.Int(nullable: false, identity: true),
                        NameProject = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 254),
                        ObjectiveGeneral = c.String(nullable: false, maxLength: 254),
                        ObjectiveImmediate = c.String(nullable: false, maxLength: 254),
                        ObjectiveMediate = c.String(nullable: false, maxLength: 254),
                        Methodology = c.String(nullable: false, maxLength: 100),
                        Resources = c.String(nullable: false, maxLength: 254),
                        Status = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                        Activities = c.String(nullable: false),
                        Responsibilities = c.String(nullable: false),
                        QuantityPracticing = c.Int(nullable: false),
                        Term = c.String(),
                        IdLinkedOrganization = c.Int(nullable: false),
                        StaffNumberCoordinator = c.String(maxLength: 20),
                        IdResponsibleProject = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdProject)
                .ForeignKey("dbo.Coordinators", t => t.StaffNumberCoordinator)
                .ForeignKey("dbo.LinkedOrganizations", t => t.IdLinkedOrganization, cascadeDelete: true)
                .ForeignKey("dbo.ResponsibleProjects", t => t.IdResponsibleProject, cascadeDelete: true)
                .Index(t => t.IdLinkedOrganization)
                .Index(t => t.StaffNumberCoordinator)
                .Index(t => t.IdResponsibleProject);
            
            CreateTable(
                "dbo.Coordinators",
                c => new
                    {
                        StaffNumber = c.String(nullable: false, maxLength: 20),
                        RegistrationDate = c.DateTime(nullable: false),
                        DischargeDate = c.DateTime(),
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
                        Gender = c.Int(nullable: false),
                        UserStatus = c.Int(nullable: false),
                        Email = c.String(nullable: false, maxLength: 254),
                        AlternateEmail = c.String(maxLength: 254),
                        PhoneNumber = c.String(maxLength: 10),
                        UserType = c.Int(nullable: false),
                        IdAccount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdUser)
                .ForeignKey("dbo.Accounts", t => t.IdAccount, cascadeDelete: true)
                .Index(t => t.IdAccount);
            
            CreateTable(
                "dbo.LinkedOrganizations",
                c => new
                    {
                        IdLinkedOrganization = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        DirectUsers = c.Int(nullable: false),
                        IndirectUsers = c.Int(nullable: false),
                        Email = c.String(nullable: false, maxLength: 254),
                        PhoneNumber = c.String(nullable: false, maxLength: 15),
                        Address = c.String(nullable: false, maxLength: 100),
                        IdCity = c.Int(nullable: false),
                        IdState = c.Int(nullable: false),
                        IdSector = c.Int(nullable: false),
                        LinkedOrganizationStatus = c.Int(nullable: false),
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
                        NameSector = c.String(maxLength: 50),
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
                        Charge = c.String(nullable: false, maxLength: 50),
                        ResponsibleProjectStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdResponsibleProject);
            
            CreateTable(
                "dbo.AdvanceQuestions",
                c => new
                    {
                        IdAdvanceQuestion = c.Int(nullable: false, identity: true),
                        Question = c.String(),
                        Reasons = c.String(),
                        Reply = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdAdvanceQuestion);
            
            CreateTable(
                "dbo.Assignments",
                c => new
                    {
                        IdAssignment = c.Int(nullable: false, identity: true),
                        CompletionTerm = c.String(),
                        DateAssignment = c.DateTime(nullable: false),
                        RouteSave = c.String(maxLength: 250),
                        StartTerm = c.String(),
                        Status = c.Int(nullable: false),
                        IdOfficeOfAcceptance = c.Int(nullable: false),
                        IdProject = c.Int(nullable: false),
                        Enrollment = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.IdAssignment)
                .ForeignKey("dbo.OfficeOfAcceptances", t => t.IdOfficeOfAcceptance, cascadeDelete: true)
                .ForeignKey("dbo.Practicioners", t => t.Enrollment)
                .ForeignKey("dbo.Projects", t => t.IdProject, cascadeDelete: true)
                .Index(t => t.IdOfficeOfAcceptance)
                .Index(t => t.IdProject)
                .Index(t => t.Enrollment);
            
            CreateTable(
                "dbo.OfficeOfAcceptances",
                c => new
                    {
                        IdOfAcceptance = c.Int(nullable: false, identity: true),
                        DateOfAcceptance = c.DateTime(nullable: false),
                        RouteSave = c.String(),
                    })
                .PrimaryKey(t => t.IdOfAcceptance);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        IdDocument = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        DeliveryDate = c.DateTime(),
                        TypeDocument = c.String(maxLength: 50),
                        RouteSave = c.String(),
                        Enrollment = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.IdDocument)
                .ForeignKey("dbo.Practicioners", t => t.Enrollment)
                .Index(t => t.Enrollment);
            
            CreateTable(
                "dbo.Managers",
                c => new
                    {
                        StaffNumber = c.String(nullable: false, maxLength: 128),
                        IdUser = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StaffNumber)
                .ForeignKey("dbo.Users", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdUser);
            
            CreateTable(
                "dbo.MonthlyReports",
                c => new
                    {
                        IdMonthlyReport = c.Int(nullable: false, identity: true),
                        DeliveryDate = c.DateTime(nullable: false),
                        PerformedActivities = c.String(),
                        resultsObtained = c.String(),
                        Enrollment = c.String(maxLength: 10),
                        IdProject = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdMonthlyReport)
                .ForeignKey("dbo.Practicioners", t => t.Enrollment)
                .ForeignKey("dbo.Projects", t => t.IdProject, cascadeDelete: true)
                .Index(t => t.Enrollment)
                .Index(t => t.IdProject);
            
            CreateTable(
                "dbo.Phones",
                c => new
                    {
                        IdPhoneNumber = c.Int(nullable: false, identity: true),
                        Extension = c.String(),
                        PhoneNumber = c.String(),
                        IdLinkedOrganization = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPhoneNumber)
                .ForeignKey("dbo.LinkedOrganizations", t => t.IdLinkedOrganization, cascadeDelete: true)
                .Index(t => t.IdLinkedOrganization);
            
            CreateTable(
                "dbo.SchedulingActivities",
                c => new
                    {
                        IdSchedulingActivity = c.Int(nullable: false, identity: true),
                        Activity = c.String(),
                        Month = c.String(),
                        IdProject = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdSchedulingActivity)
                .ForeignKey("dbo.Projects", t => t.IdProject, cascadeDelete: true)
                .Index(t => t.IdProject);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        StaffNumber = c.String(nullable: false, maxLength: 20),
                        RegistrationDate = c.DateTime(),
                        DischargeDate = c.DateTime(),
                        IdUser = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StaffNumber)
                .ForeignKey("dbo.Users", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdUser);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Teachers", "IdUser", "dbo.Users");
            DropForeignKey("dbo.SchedulingActivities", "IdProject", "dbo.Projects");
            DropForeignKey("dbo.Phones", "IdLinkedOrganization", "dbo.LinkedOrganizations");
            DropForeignKey("dbo.MonthlyReports", "IdProject", "dbo.Projects");
            DropForeignKey("dbo.MonthlyReports", "Enrollment", "dbo.Practicioners");
            DropForeignKey("dbo.Managers", "IdUser", "dbo.Users");
            DropForeignKey("dbo.Documents", "Enrollment", "dbo.Practicioners");
            DropForeignKey("dbo.Assignments", "IdProject", "dbo.Projects");
            DropForeignKey("dbo.Assignments", "Enrollment", "dbo.Practicioners");
            DropForeignKey("dbo.Assignments", "IdOfficeOfAcceptance", "dbo.OfficeOfAcceptances");
            DropForeignKey("dbo.ActivityMades", "IdPartialReport", "dbo.PartialReports");
            DropForeignKey("dbo.PartialReports", "IdProject", "dbo.Projects");
            DropForeignKey("dbo.PartialReports", "Enrollment", "dbo.Practicioners");
            DropForeignKey("dbo.Practicioners", "IdUser", "dbo.Users");
            DropForeignKey("dbo.RequestProjects", "IdProject", "dbo.Projects");
            DropForeignKey("dbo.Projects", "IdResponsibleProject", "dbo.ResponsibleProjects");
            DropForeignKey("dbo.LinkedOrganizations", "IdState", "dbo.States");
            DropForeignKey("dbo.LinkedOrganizations", "IdSector", "dbo.Sectors");
            DropForeignKey("dbo.Projects", "IdLinkedOrganization", "dbo.LinkedOrganizations");
            DropForeignKey("dbo.LinkedOrganizations", "IdCity", "dbo.Cities");
            DropForeignKey("dbo.Projects", "StaffNumberCoordinator", "dbo.Coordinators");
            DropForeignKey("dbo.Coordinators", "IdUser", "dbo.Users");
            DropForeignKey("dbo.Users", "IdAccount", "dbo.Accounts");
            DropForeignKey("dbo.RequestProjects", "Enrollment", "dbo.Practicioners");
            DropForeignKey("dbo.Practicioners", "Nrc", "dbo.Groups");
            DropIndex("dbo.Teachers", new[] { "IdUser" });
            DropIndex("dbo.SchedulingActivities", new[] { "IdProject" });
            DropIndex("dbo.Phones", new[] { "IdLinkedOrganization" });
            DropIndex("dbo.MonthlyReports", new[] { "IdProject" });
            DropIndex("dbo.MonthlyReports", new[] { "Enrollment" });
            DropIndex("dbo.Managers", new[] { "IdUser" });
            DropIndex("dbo.Documents", new[] { "Enrollment" });
            DropIndex("dbo.Assignments", new[] { "Enrollment" });
            DropIndex("dbo.Assignments", new[] { "IdProject" });
            DropIndex("dbo.Assignments", new[] { "IdOfficeOfAcceptance" });
            DropIndex("dbo.LinkedOrganizations", new[] { "IdSector" });
            DropIndex("dbo.LinkedOrganizations", new[] { "IdState" });
            DropIndex("dbo.LinkedOrganizations", new[] { "IdCity" });
            DropIndex("dbo.Users", new[] { "IdAccount" });
            DropIndex("dbo.Coordinators", new[] { "IdUser" });
            DropIndex("dbo.Projects", new[] { "IdResponsibleProject" });
            DropIndex("dbo.Projects", new[] { "StaffNumberCoordinator" });
            DropIndex("dbo.Projects", new[] { "IdLinkedOrganization" });
            DropIndex("dbo.RequestProjects", new[] { "Enrollment" });
            DropIndex("dbo.RequestProjects", new[] { "IdProject" });
            DropIndex("dbo.Practicioners", new[] { "Nrc" });
            DropIndex("dbo.Practicioners", new[] { "IdUser" });
            DropIndex("dbo.PartialReports", new[] { "Enrollment" });
            DropIndex("dbo.PartialReports", new[] { "IdProject" });
            DropIndex("dbo.ActivityMades", new[] { "IdPartialReport" });
            DropTable("dbo.Teachers");
            DropTable("dbo.SchedulingActivities");
            DropTable("dbo.Phones");
            DropTable("dbo.MonthlyReports");
            DropTable("dbo.Managers");
            DropTable("dbo.Documents");
            DropTable("dbo.OfficeOfAcceptances");
            DropTable("dbo.Assignments");
            DropTable("dbo.AdvanceQuestions");
            DropTable("dbo.ResponsibleProjects");
            DropTable("dbo.States");
            DropTable("dbo.Sectors");
            DropTable("dbo.Cities");
            DropTable("dbo.LinkedOrganizations");
            DropTable("dbo.Users");
            DropTable("dbo.Coordinators");
            DropTable("dbo.Projects");
            DropTable("dbo.RequestProjects");
            DropTable("dbo.Groups");
            DropTable("dbo.Practicioners");
            DropTable("dbo.PartialReports");
            DropTable("dbo.ActivityMades");
            DropTable("dbo.Accounts");
        }
    }
}
