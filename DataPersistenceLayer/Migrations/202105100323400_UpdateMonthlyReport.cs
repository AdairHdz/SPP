namespace DataPersistenceLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMonthlyReport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdvanceQuestions", "IdMonthlyReport", c => c.Int(nullable: false));
            CreateIndex("dbo.AdvanceQuestions", "IdMonthlyReport");
            AddForeignKey("dbo.AdvanceQuestions", "IdMonthlyReport", "dbo.MonthlyReports", "IdMonthlyReport", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AdvanceQuestions", "IdMonthlyReport", "dbo.MonthlyReports");
            DropIndex("dbo.AdvanceQuestions", new[] { "IdMonthlyReport" });
            DropColumn("dbo.AdvanceQuestions", "IdMonthlyReport");
        }
    }
}
