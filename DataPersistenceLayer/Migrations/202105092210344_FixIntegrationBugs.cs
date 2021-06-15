namespace DataPersistenceLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixIntegrationBugs : DbMigration
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
                        Salt = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.IdAccount);
            
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        IdActivity = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        ActivityType = c.Int(nullable: false),
                        ActivityStatus = c.Int(nullable: false),
                        Description = c.String(maxLength: 255),
                        ValueActivity = c.Double(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        FinishDate = c.DateTime(nullable: false),
                        StaffNumberTeacher = c.String(nullable: false, maxLength: 20),
                        IdGroup = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdActivity)
                .ForeignKey("dbo.Groups", t => t.IdGroup, cascadeDelete: true)
                .ForeignKey("dbo.Teachers", t => t.StaffNumberTeacher, cascadeDelete: true)
                .Index(t => t.StaffNumberTeacher)
                .Index(t => t.IdGroup);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        IdGroup = c.Int(nullable: false, identity: true),
                        Nrc = c.String(nullable: false),
                        GroupStatus = c.Int(nullable: false),
                        Term = c.String(),
                        StaffNumber = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.IdGroup)
                .ForeignKey("dbo.Teachers", t => t.StaffNumber, cascadeDelete: false)
                .Index(t => t.StaffNumber);
            
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
                "dbo.ActivityMades",
                c => new
                    {
                        IdActivity = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 300),
                        PlannedWeek = c.String(nullable: false, maxLength: 100),
                        RealWeek = c.String(nullable: false, maxLength: 100),
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
                        NumberReport = c.String(nullable: false, maxLength: 20),
                        ResultsObtained = c.String(nullable: false, maxLength: 500),
                        HoursCovered = c.Int(nullable: false),
                        Observations = c.String(maxLength: 500),
                        DeliveryDate = c.DateTime(nullable: false),
                        IdProject = c.Int(nullable: false),
                        Enrollment = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.IdParcialReport)
                .ForeignKey("dbo.Practicioners", t => t.Enrollment, cascadeDelete: true)
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
                        IdGroup = c.Int(),
                    })
                .PrimaryKey(t => t.Enrollment)
                .ForeignKey("dbo.Groups", t => t.IdGroup)
                .ForeignKey("dbo.Users", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdUser)
                .Index(t => t.IdGroup);
            
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
                        NameProject = c.String(nullable: false, maxLength: 150),
                        DaysHours = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 300),
                        ObjectiveGeneral = c.String(nullable: false, maxLength: 300),
                        ObjectiveImmediate = c.String(nullable: false, maxLength: 300),
                        ObjectiveMediate = c.String(nullable: false, maxLength: 300),
                        Methodology = c.String(nullable: false, maxLength: 300),
                        Resources = c.String(nullable: false, maxLength: 300),
                        Status = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                        Activities = c.String(nullable: false, maxLength: 300),
                        Responsibilities = c.String(nullable: false, maxLength: 300),
                        QuantityPracticing = c.Int(nullable: false),
                        QuantityPracticingAssing = c.Int(nullable: false),
                        Term = c.String(nullable: false, maxLength: 50),
                        IdLinkedOrganization = c.Int(nullable: false),
                        StaffNumberCoordinator = c.String(nullable: false, maxLength: 20),
                        IdResponsibleProject = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdProject)
                .ForeignKey("dbo.Coordinators", t => t.StaffNumberCoordinator, cascadeDelete: true)
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
                .ForeignKey("dbo.Users", t => t.IdUser, cascadeDelete: false)
                .Index(t => t.IdUser);
            
            CreateTable(
                "dbo.LinkedOrganizations",
                c => new
                    {
                        IdLinkedOrganization = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        DirectUsers = c.String(nullable: false, maxLength: 254),
                        IndirectUsers = c.String(nullable: false, maxLength: 254),
                        Email = c.String(nullable: false, maxLength: 254),
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
                        State_IdState = c.Int(),
                    })
                .PrimaryKey(t => t.IdCity)
                .ForeignKey("dbo.States", t => t.State_IdState)
                .Index(t => t.State_IdState);
            
            CreateTable(
                "dbo.Phones",
                c => new
                    {
                        IdPhoneNumber = c.Int(nullable: false, identity: true),
                        Extension = c.String(maxLength: 3),
                        PhoneNumber = c.String(maxLength: 10),
                        IdLinkedOrganization = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPhoneNumber)
                .ForeignKey("dbo.LinkedOrganizations", t => t.IdLinkedOrganization, cascadeDelete: true)
                .Index(t => t.IdLinkedOrganization);
            
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
                "dbo.SchedulingActivities",
                c => new
                    {
                        IdSchedulingActivity = c.Int(nullable: false, identity: true),
                        Activity = c.String(nullable: false, maxLength: 300),
                        Month = c.String(nullable: false, maxLength: 50),
                        IdProject = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdSchedulingActivity)
                .ForeignKey("dbo.Projects", t => t.IdProject, cascadeDelete: true)
                .Index(t => t.IdProject);
            
            CreateTable(
                "dbo.ActivityPracticioners",
                c => new
                    {
                        IdActivityPracticioner = c.Int(nullable: false, identity: true),
                        Qualification = c.Double(nullable: false),
                        Observation = c.String(maxLength: 255),
                        Answer = c.String(maxLength: 255),
                        ActivityPracticionerStatus = c.Int(nullable: false),
                        Enrollment = c.String(maxLength: 10),
                        IdActivity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdActivityPracticioner)
                .ForeignKey("dbo.Activities", t => t.IdActivity, cascadeDelete: true)
                .ForeignKey("dbo.Practicioners", t => t.Enrollment)
                .Index(t => t.Enrollment)
                .Index(t => t.IdActivity);
            
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
                        Name = c.String(maxLength: 100),
                        DeliveryDate = c.DateTime(),
                        TypeDocument = c.String(maxLength: 50),
                        RouteSave = c.String(),
                        IdActivityPracticioner = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdDocument)
                .ForeignKey("dbo.ActivityPracticioners", t => t.IdActivityPracticioner, cascadeDelete: true)
                .Index(t => t.IdActivityPracticioner);
            
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
                        ResultsObtained = c.String(),
                        HoursReported = c.Int(nullable: false),
                        HoursCumulative = c.Int(nullable: false),
                        Enrollment = c.String(maxLength: 10),
                        IdProject = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdMonthlyReport)
                .ForeignKey("dbo.Practicioners", t => t.Enrollment)
                .ForeignKey("dbo.Projects", t => t.IdProject, cascadeDelete: true)
                .Index(t => t.Enrollment)
                .Index(t => t.IdProject);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MonthlyReports", "IdProject", "dbo.Projects");
            DropForeignKey("dbo.MonthlyReports", "Enrollment", "dbo.Practicioners");
            DropForeignKey("dbo.Managers", "IdUser", "dbo.Users");
            DropForeignKey("dbo.Documents", "IdActivityPracticioner", "dbo.ActivityPracticioners");
            DropForeignKey("dbo.Assignments", "IdProject", "dbo.Projects");
            DropForeignKey("dbo.Assignments", "Enrollment", "dbo.Practicioners");
            DropForeignKey("dbo.Assignments", "IdOfficeOfAcceptance", "dbo.OfficeOfAcceptances");
            DropForeignKey("dbo.ActivityPracticioners", "Enrollment", "dbo.Practicioners");
            DropForeignKey("dbo.ActivityPracticioners", "IdActivity", "dbo.Activities");
            DropForeignKey("dbo.ActivityMades", "IdPartialReport", "dbo.PartialReports");
            DropForeignKey("dbo.PartialReports", "IdProject", "dbo.Projects");
            DropForeignKey("dbo.PartialReports", "Enrollment", "dbo.Practicioners");
            DropForeignKey("dbo.Practicioners", "IdUser", "dbo.Users");
            DropForeignKey("dbo.RequestProjects", "IdProject", "dbo.Projects");
            DropForeignKey("dbo.SchedulingActivities", "IdProject", "dbo.Projects");
            DropForeignKey("dbo.Projects", "IdResponsibleProject", "dbo.ResponsibleProjects");
            DropForeignKey("dbo.LinkedOrganizations", "IdState", "dbo.States");
            DropForeignKey("dbo.Cities", "State_IdState", "dbo.States");
            DropForeignKey("dbo.LinkedOrganizations", "IdSector", "dbo.Sectors");
            DropForeignKey("dbo.Projects", "IdLinkedOrganization", "dbo.LinkedOrganizations");
            DropForeignKey("dbo.Phones", "IdLinkedOrganization", "dbo.LinkedOrganizations");
            DropForeignKey("dbo.LinkedOrganizations", "IdCity", "dbo.Cities");
            DropForeignKey("dbo.Projects", "StaffNumberCoordinator", "dbo.Coordinators");
            DropForeignKey("dbo.Coordinators", "IdUser", "dbo.Users");
            DropForeignKey("dbo.RequestProjects", "Enrollment", "dbo.Practicioners");
            DropForeignKey("dbo.Practicioners", "IdGroup", "dbo.Groups");
            DropForeignKey("dbo.Activities", "StaffNumberTeacher", "dbo.Teachers");
            DropForeignKey("dbo.Activities", "IdGroup", "dbo.Groups");
            DropForeignKey("dbo.Groups", "StaffNumber", "dbo.Teachers");
            DropForeignKey("dbo.Teachers", "IdUser", "dbo.Users");
            DropForeignKey("dbo.Users", "IdAccount", "dbo.Accounts");
            DropIndex("dbo.MonthlyReports", new[] { "IdProject" });
            DropIndex("dbo.MonthlyReports", new[] { "Enrollment" });
            DropIndex("dbo.Managers", new[] { "IdUser" });
            DropIndex("dbo.Documents", new[] { "IdActivityPracticioner" });
            DropIndex("dbo.Assignments", new[] { "Enrollment" });
            DropIndex("dbo.Assignments", new[] { "IdProject" });
            DropIndex("dbo.Assignments", new[] { "IdOfficeOfAcceptance" });
            DropIndex("dbo.ActivityPracticioners", new[] { "IdActivity" });
            DropIndex("dbo.ActivityPracticioners", new[] { "Enrollment" });
            DropIndex("dbo.SchedulingActivities", new[] { "IdProject" });
            DropIndex("dbo.Phones", new[] { "IdLinkedOrganization" });
            DropIndex("dbo.Cities", new[] { "State_IdState" });
            DropIndex("dbo.LinkedOrganizations", new[] { "IdSector" });
            DropIndex("dbo.LinkedOrganizations", new[] { "IdState" });
            DropIndex("dbo.LinkedOrganizations", new[] { "IdCity" });
            DropIndex("dbo.Coordinators", new[] { "IdUser" });
            DropIndex("dbo.Projects", new[] { "IdResponsibleProject" });
            DropIndex("dbo.Projects", new[] { "StaffNumberCoordinator" });
            DropIndex("dbo.Projects", new[] { "IdLinkedOrganization" });
            DropIndex("dbo.RequestProjects", new[] { "Enrollment" });
            DropIndex("dbo.RequestProjects", new[] { "IdProject" });
            DropIndex("dbo.Practicioners", new[] { "IdGroup" });
            DropIndex("dbo.Practicioners", new[] { "IdUser" });
            DropIndex("dbo.PartialReports", new[] { "Enrollment" });
            DropIndex("dbo.PartialReports", new[] { "IdProject" });
            DropIndex("dbo.ActivityMades", new[] { "IdPartialReport" });
            DropIndex("dbo.Users", new[] { "IdAccount" });
            DropIndex("dbo.Teachers", new[] { "IdUser" });
            DropIndex("dbo.Groups", new[] { "StaffNumber" });
            DropIndex("dbo.Activities", new[] { "IdGroup" });
            DropIndex("dbo.Activities", new[] { "StaffNumberTeacher" });
            DropTable("dbo.MonthlyReports");
            DropTable("dbo.Managers");
            DropTable("dbo.Documents");
            DropTable("dbo.OfficeOfAcceptances");
            DropTable("dbo.Assignments");
            DropTable("dbo.AdvanceQuestions");
            DropTable("dbo.ActivityPracticioners");
            DropTable("dbo.SchedulingActivities");
            DropTable("dbo.ResponsibleProjects");
            DropTable("dbo.States");
            DropTable("dbo.Sectors");
            DropTable("dbo.Phones");
            DropTable("dbo.Cities");
            DropTable("dbo.LinkedOrganizations");
            DropTable("dbo.Coordinators");
            DropTable("dbo.Projects");
            DropTable("dbo.RequestProjects");
            DropTable("dbo.Practicioners");
            DropTable("dbo.PartialReports");
            DropTable("dbo.ActivityMades");
            DropTable("dbo.Users");
            DropTable("dbo.Teachers");
            DropTable("dbo.Groups");
            DropTable("dbo.Activities");
            DropTable("dbo.Accounts");
        }
    }
}
