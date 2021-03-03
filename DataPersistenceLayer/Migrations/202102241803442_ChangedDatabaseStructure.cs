namespace DataPersistenceLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDatabaseStructure : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Activities", "EnrollmentPracticing", "dbo.Practicings");
            DropForeignKey("dbo.Practicings", "IdPeriod", "dbo.Periods");
            DropForeignKey("dbo.Users", "IdGender", "dbo.Genders");
            DropForeignKey("dbo.Users", "IdStatus", "dbo.UserStatus");
            DropForeignKey("dbo.ResponsibleProjects", "IdPosition", "dbo.Positions");
            DropForeignKey("dbo.Projects", "IdProjectStatus", "dbo.ProjectStatus");
            DropForeignKey("dbo.Practicings", "IdProject", "dbo.Projects");
            DropForeignKey("dbo.Reports", "EnrollmentPracticing", "dbo.Practicings");
            DropForeignKey("dbo.Requests", "Enrollment", "dbo.Practicings");
            DropForeignKey("dbo.Requests", "IdProject", "dbo.Projects");
            DropForeignKey("dbo.Requests", "Status", "dbo.RequestStatus");
            DropForeignKey("dbo.Practicings", "IdTurn", "dbo.Turns");
            DropForeignKey("dbo.Practicings", "IdUser", "dbo.Users");
            DropForeignKey("dbo.Teachers", "IdTurn", "dbo.Turns");
            DropForeignKey("dbo.Activities", "IdStaffNumberTeacher", "dbo.Teachers");
            DropForeignKey("dbo.ReportPartials", "IdReport", "dbo.Reports");
            DropIndex("dbo.Activities", new[] { "EnrollmentPracticing" });
            DropIndex("dbo.Activities", new[] { "IdStaffNumberTeacher" });
            DropIndex("dbo.Practicings", new[] { "IdTurn" });
            DropIndex("dbo.Practicings", new[] { "IdPeriod" });
            DropIndex("dbo.Practicings", new[] { "IdUser" });
            DropIndex("dbo.Practicings", new[] { "IdProject" });
            DropIndex("dbo.Projects", new[] { "IdProjectStatus" });
            DropIndex("dbo.Users", new[] { "IdGender" });
            DropIndex("dbo.Users", new[] { "IdStatus" });
            DropIndex("dbo.ResponsibleProjects", new[] { "IdPosition" });
            DropIndex("dbo.Reports", new[] { "EnrollmentPracticing" });
            DropIndex("dbo.Requests", new[] { "Enrollment" });
            DropIndex("dbo.Requests", new[] { "IdProject" });
            DropIndex("dbo.Requests", new[] { "Status" });
            DropIndex("dbo.Teachers", new[] { "IdTurn" });
            DropIndex("dbo.ReportPartials", new[] { "IdReport" });
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
            
            AddColumn("dbo.Projects", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Projects", "Term", c => c.String());
            AddColumn("dbo.Users", "Gender", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "UserStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "UserType", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "IdAccount", c => c.Int(nullable: false));
            AddColumn("dbo.LinkedOrganizations", "LinkedOrganizationStatus", c => c.Int(nullable: false));
            AddColumn("dbo.ResponsibleProjects", "Charge", c => c.String());
            AddColumn("dbo.ResponsibleProjects", "ResponsibleProjectStatus", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "IdAccount");
            AddForeignKey("dbo.Users", "IdAccount", "dbo.Accounts", "IdAccount", cascadeDelete: true);
            DropColumn("dbo.Projects", "IdProjectStatus");
            DropColumn("dbo.Users", "IdGender");
            DropColumn("dbo.Users", "IdStatus");
            DropColumn("dbo.ResponsibleProjects", "IdPosition");
            DropColumn("dbo.Teachers", "IdTurn");
            DropTable("dbo.Activities");
            DropTable("dbo.Practicings");
            DropTable("dbo.Periods");
            DropTable("dbo.Genders");
            DropTable("dbo.UserStatus");
            DropTable("dbo.Positions");
            DropTable("dbo.ProjectStatus");
            DropTable("dbo.Reports");
            DropTable("dbo.Requests");
            DropTable("dbo.RequestStatus");
            DropTable("dbo.Turns");
            DropTable("dbo.ReportPartials");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ReportPartials",
                c => new
                    {
                        IdReportParcial = c.Int(nullable: false, identity: true),
                        NumberReport = c.Int(nullable: false),
                        ResultsObtained = c.String(nullable: false, maxLength: 254),
                        HoursCovered = c.Int(nullable: false),
                        Observations = c.String(maxLength: 254),
                        IdReport = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdReportParcial);
            
            CreateTable(
                "dbo.Turns",
                c => new
                    {
                        IdTurn = c.Int(nullable: false, identity: true),
                        TurnName = c.String(nullable: false, maxLength: 15),
                    })
                .PrimaryKey(t => t.IdTurn);
            
            CreateTable(
                "dbo.RequestStatus",
                c => new
                    {
                        IdRequestStatus = c.Int(nullable: false, identity: true),
                        Status = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IdRequestStatus);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        IdRequest = c.Int(nullable: false, identity: true),
                        Enrollment = c.String(maxLength: 10),
                        IdProject = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdRequest);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        IdReport = c.Int(nullable: false, identity: true),
                        Activities = c.String(nullable: false),
                        Score = c.Int(nullable: false),
                        CompletionDate = c.DateTime(),
                        DeliverDate = c.DateTime(),
                        Document = c.String(),
                        EnrollmentPracticing = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.IdReport);
            
            CreateTable(
                "dbo.ProjectStatus",
                c => new
                    {
                        IdProjectStatus = c.Int(nullable: false, identity: true),
                        Status = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.IdProjectStatus);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        IdPosition = c.Int(nullable: false, identity: true),
                        NamePosition = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IdPosition);
            
            CreateTable(
                "dbo.UserStatus",
                c => new
                    {
                        UserStatusId = c.Int(nullable: false, identity: true),
                        Status = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.UserStatusId);
            
            CreateTable(
                "dbo.Genders",
                c => new
                    {
                        IdGender = c.Int(nullable: false, identity: true),
                        GenderName = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.IdGender);
            
            CreateTable(
                "dbo.Periods",
                c => new
                    {
                        IdPeriod = c.Int(nullable: false, identity: true),
                        PeriodName = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.IdPeriod);
            
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
                .PrimaryKey(t => t.Enrollment);
            
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        IdActivity = c.Int(nullable: false, identity: true),
                        Value = c.Int(nullable: false),
                        Description = c.String(maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 100),
                        DeliverDate = c.DateTime(nullable: false),
                        Document = c.String(maxLength: 255),
                        EnrollmentPracticing = c.String(nullable: false, maxLength: 10),
                        IdStaffNumberTeacher = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.IdActivity);
            
            AddColumn("dbo.Teachers", "IdTurn", c => c.Int(nullable: false));
            AddColumn("dbo.ResponsibleProjects", "IdPosition", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "IdStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "IdGender", c => c.Int(nullable: false));
            AddColumn("dbo.Projects", "IdProjectStatus", c => c.Int(nullable: false));
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
            DropForeignKey("dbo.Users", "IdAccount", "dbo.Accounts");
            DropForeignKey("dbo.RequestProjects", "Enrollment", "dbo.Practicioners");
            DropForeignKey("dbo.Practicioners", "Nrc", "dbo.Groups");
            DropIndex("dbo.SchedulingActivities", new[] { "IdProject" });
            DropIndex("dbo.Phones", new[] { "IdLinkedOrganization" });
            DropIndex("dbo.MonthlyReports", new[] { "IdProject" });
            DropIndex("dbo.MonthlyReports", new[] { "Enrollment" });
            DropIndex("dbo.Managers", new[] { "IdUser" });
            DropIndex("dbo.Documents", new[] { "Enrollment" });
            DropIndex("dbo.Assignments", new[] { "Enrollment" });
            DropIndex("dbo.Assignments", new[] { "IdProject" });
            DropIndex("dbo.Assignments", new[] { "IdOfficeOfAcceptance" });
            DropIndex("dbo.Users", new[] { "IdAccount" });
            DropIndex("dbo.RequestProjects", new[] { "Enrollment" });
            DropIndex("dbo.RequestProjects", new[] { "IdProject" });
            DropIndex("dbo.Practicioners", new[] { "Nrc" });
            DropIndex("dbo.Practicioners", new[] { "IdUser" });
            DropIndex("dbo.PartialReports", new[] { "Enrollment" });
            DropIndex("dbo.PartialReports", new[] { "IdProject" });
            DropIndex("dbo.ActivityMades", new[] { "IdPartialReport" });
            DropColumn("dbo.ResponsibleProjects", "ResponsibleProjectStatus");
            DropColumn("dbo.ResponsibleProjects", "Charge");
            DropColumn("dbo.LinkedOrganizations", "LinkedOrganizationStatus");
            DropColumn("dbo.Users", "IdAccount");
            DropColumn("dbo.Users", "UserType");
            DropColumn("dbo.Users", "UserStatus");
            DropColumn("dbo.Users", "Gender");
            DropColumn("dbo.Projects", "Term");
            DropColumn("dbo.Projects", "Status");
            DropTable("dbo.SchedulingActivities");
            DropTable("dbo.Phones");
            DropTable("dbo.MonthlyReports");
            DropTable("dbo.Managers");
            DropTable("dbo.Documents");
            DropTable("dbo.OfficeOfAcceptances");
            DropTable("dbo.Assignments");
            DropTable("dbo.AdvanceQuestions");
            DropTable("dbo.RequestProjects");
            DropTable("dbo.Groups");
            DropTable("dbo.Practicioners");
            DropTable("dbo.PartialReports");
            DropTable("dbo.ActivityMades");
            DropTable("dbo.Accounts");
            CreateIndex("dbo.ReportPartials", "IdReport");
            CreateIndex("dbo.Teachers", "IdTurn");
            CreateIndex("dbo.Requests", "Status");
            CreateIndex("dbo.Requests", "IdProject");
            CreateIndex("dbo.Requests", "Enrollment");
            CreateIndex("dbo.Reports", "EnrollmentPracticing");
            CreateIndex("dbo.ResponsibleProjects", "IdPosition");
            CreateIndex("dbo.Users", "IdStatus");
            CreateIndex("dbo.Users", "IdGender");
            CreateIndex("dbo.Projects", "IdProjectStatus");
            CreateIndex("dbo.Practicings", "IdProject");
            CreateIndex("dbo.Practicings", "IdUser");
            CreateIndex("dbo.Practicings", "IdPeriod");
            CreateIndex("dbo.Practicings", "IdTurn");
            CreateIndex("dbo.Activities", "IdStaffNumberTeacher");
            CreateIndex("dbo.Activities", "EnrollmentPracticing");
            AddForeignKey("dbo.ReportPartials", "IdReport", "dbo.Reports", "IdReport", cascadeDelete: true);
            AddForeignKey("dbo.Activities", "IdStaffNumberTeacher", "dbo.Teachers", "StaffNumber");
            AddForeignKey("dbo.Teachers", "IdTurn", "dbo.Turns", "IdTurn", cascadeDelete: true);
            AddForeignKey("dbo.Practicings", "IdUser", "dbo.Users", "IdUser", cascadeDelete: true);
            AddForeignKey("dbo.Practicings", "IdTurn", "dbo.Turns", "IdTurn", cascadeDelete: true);
            AddForeignKey("dbo.Requests", "Status", "dbo.RequestStatus", "IdRequestStatus", cascadeDelete: true);
            AddForeignKey("dbo.Requests", "IdProject", "dbo.Projects", "IdProject", cascadeDelete: true);
            AddForeignKey("dbo.Requests", "Enrollment", "dbo.Practicings", "Enrollment");
            AddForeignKey("dbo.Reports", "EnrollmentPracticing", "dbo.Practicings", "Enrollment", cascadeDelete: true);
            AddForeignKey("dbo.Practicings", "IdProject", "dbo.Projects", "IdProject", cascadeDelete: true);
            AddForeignKey("dbo.Projects", "IdProjectStatus", "dbo.ProjectStatus", "IdProjectStatus", cascadeDelete: true);
            AddForeignKey("dbo.ResponsibleProjects", "IdPosition", "dbo.Positions", "IdPosition", cascadeDelete: true);
            AddForeignKey("dbo.Users", "IdStatus", "dbo.UserStatus", "UserStatusId", cascadeDelete: true);
            AddForeignKey("dbo.Users", "IdGender", "dbo.Genders", "IdGender", cascadeDelete: true);
            AddForeignKey("dbo.Practicings", "IdPeriod", "dbo.Periods", "IdPeriod", cascadeDelete: true);
            AddForeignKey("dbo.Activities", "EnrollmentPracticing", "dbo.Practicings", "Enrollment", cascadeDelete: true);
        }
    }
}
