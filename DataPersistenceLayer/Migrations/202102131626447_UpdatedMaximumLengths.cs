namespace DataPersistenceLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedMaximumLengths : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Activities", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Projects", "NameProject", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Projects", "Description", c => c.String(nullable: false, maxLength: 254));
            AlterColumn("dbo.Projects", "ObjectiveGeneral", c => c.String(nullable: false, maxLength: 254));
            AlterColumn("dbo.Projects", "ObjectiveImmediate", c => c.String(nullable: false, maxLength: 254));
            AlterColumn("dbo.Projects", "ObjectiveMediate", c => c.String(nullable: false, maxLength: 254));
            AlterColumn("dbo.Projects", "Methodology", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Projects", "Resources", c => c.String(nullable: false, maxLength: 254));
            AlterColumn("dbo.UserStatus", "Status", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.LinkedOrganizations", "Address", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Sectors", "NameSector", c => c.String(maxLength: 50));
            AlterColumn("dbo.Positions", "NamePosition", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.RequestStatus", "Status", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.ReportPartials", "ResultsObtained", c => c.String(nullable: false, maxLength: 254));
            AlterColumn("dbo.ReportPartials", "Observations", c => c.String(maxLength: 254));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReportPartials", "Observations", c => c.String(maxLength: 100));
            AlterColumn("dbo.ReportPartials", "ResultsObtained", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.RequestStatus", "Status", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Positions", "NamePosition", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.Sectors", "NameSector", c => c.String(maxLength: 25));
            AlterColumn("dbo.LinkedOrganizations", "Address", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.UserStatus", "Status", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Projects", "Resources", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.Projects", "Methodology", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Projects", "ObjectiveMediate", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Projects", "ObjectiveImmediate", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Projects", "ObjectiveGeneral", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Projects", "Description", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Projects", "NameProject", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.Activities", "Name", c => c.String(nullable: false, maxLength: 25));
        }
    }
}
