namespace DataPersistenceLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePatialReport : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PartialReports", "Enrollment", "dbo.Practicioners");
            DropIndex("dbo.PartialReports", new[] { "Enrollment" });
            AlterColumn("dbo.ActivityMades", "Name", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.ActivityMades", "PlannedWeek", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.ActivityMades", "RealWeek", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.PartialReports", "NumberReport", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.PartialReports", "ResultsObtained", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.PartialReports", "Observations", c => c.String(maxLength: 500));
            AlterColumn("dbo.PartialReports", "DeliveryDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PartialReports", "Enrollment", c => c.String(nullable: false, maxLength: 10));
            CreateIndex("dbo.PartialReports", "Enrollment");
            AddForeignKey("dbo.PartialReports", "Enrollment", "dbo.Practicioners", "Enrollment", cascadeDelete: false);
            DropColumn("dbo.ActivityMades", "PlannedMonth");
            DropColumn("dbo.ActivityMades", "RealMonth");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ActivityMades", "RealMonth", c => c.String());
            AddColumn("dbo.ActivityMades", "PlannedMonth", c => c.String());
            DropForeignKey("dbo.PartialReports", "Enrollment", "dbo.Practicioners");
            DropIndex("dbo.PartialReports", new[] { "Enrollment" });
            AlterColumn("dbo.PartialReports", "Enrollment", c => c.String(maxLength: 10));
            AlterColumn("dbo.PartialReports", "DeliveryDate", c => c.DateTime());
            AlterColumn("dbo.PartialReports", "Observations", c => c.String(maxLength: 254));
            AlterColumn("dbo.PartialReports", "ResultsObtained", c => c.String(nullable: false, maxLength: 254));
            AlterColumn("dbo.PartialReports", "NumberReport", c => c.Int(nullable: false));
            AlterColumn("dbo.ActivityMades", "RealWeek", c => c.String());
            AlterColumn("dbo.ActivityMades", "PlannedWeek", c => c.String());
            AlterColumn("dbo.ActivityMades", "Name", c => c.String());
            CreateIndex("dbo.PartialReports", "Enrollment");
            AddForeignKey("dbo.PartialReports", "Enrollment", "dbo.Practicioners", "Enrollment");
        }
    }
}
