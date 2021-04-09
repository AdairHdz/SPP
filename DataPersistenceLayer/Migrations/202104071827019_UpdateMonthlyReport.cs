namespace DataPersistenceLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMonthlyReport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MonthlyReports", "HoursReported", c => c.Int(nullable: false));
            AddColumn("dbo.MonthlyReports", "HoursCumulative", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MonthlyReports", "HoursCumulative");
            DropColumn("dbo.MonthlyReports", "HoursReported");
        }
    }
}
