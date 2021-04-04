namespace DataPersistenceLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProject : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "StaffNumberCoordinator", "dbo.Coordinators");
            DropIndex("dbo.Projects", new[] { "StaffNumberCoordinator" });
            AddColumn("dbo.Projects", "QuantityPracticingAssing", c => c.Int(nullable: false));
            AlterColumn("dbo.Projects", "NameProject", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Projects", "Description", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.Projects", "ObjectiveGeneral", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.Projects", "ObjectiveImmediate", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.Projects", "ObjectiveMediate", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.Projects", "Methodology", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.Projects", "Resources", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.Projects", "Activities", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.Projects", "Responsibilities", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.Projects", "Term", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Projects", "StaffNumberCoordinator", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.SchedulingActivities", "Activity", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.SchedulingActivities", "Month", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.Projects", "StaffNumberCoordinator");
            AddForeignKey("dbo.Projects", "StaffNumberCoordinator", "dbo.Coordinators", "StaffNumber", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "StaffNumberCoordinator", "dbo.Coordinators");
            DropIndex("dbo.Projects", new[] { "StaffNumberCoordinator" });
            AlterColumn("dbo.SchedulingActivities", "Month", c => c.String());
            AlterColumn("dbo.SchedulingActivities", "Activity", c => c.String());
            AlterColumn("dbo.Projects", "StaffNumberCoordinator", c => c.String(maxLength: 20));
            AlterColumn("dbo.Projects", "Term", c => c.String());
            AlterColumn("dbo.Projects", "Responsibilities", c => c.String(nullable: false));
            AlterColumn("dbo.Projects", "Activities", c => c.String(nullable: false));
            AlterColumn("dbo.Projects", "Resources", c => c.String(nullable: false, maxLength: 254));
            AlterColumn("dbo.Projects", "Methodology", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Projects", "ObjectiveMediate", c => c.String(nullable: false, maxLength: 254));
            AlterColumn("dbo.Projects", "ObjectiveImmediate", c => c.String(nullable: false, maxLength: 254));
            AlterColumn("dbo.Projects", "ObjectiveGeneral", c => c.String(nullable: false, maxLength: 254));
            AlterColumn("dbo.Projects", "Description", c => c.String(nullable: false, maxLength: 254));
            AlterColumn("dbo.Projects", "NameProject", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Projects", "QuantityPracticingAssing");
            CreateIndex("dbo.Projects", "StaffNumberCoordinator");
            AddForeignKey("dbo.Projects", "StaffNumberCoordinator", "dbo.Coordinators", "StaffNumber");
        }
    }
}
