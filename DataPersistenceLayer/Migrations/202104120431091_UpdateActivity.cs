namespace DataPersistenceLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateActivity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "IdGroup", c => c.Int(nullable: false));
            CreateIndex("dbo.Activities", "IdGroup");
            AddForeignKey("dbo.Activities", "IdGroup", "dbo.Groups", "IdGroup", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activities", "IdGroup", "dbo.Groups");
            DropIndex("dbo.Activities", new[] { "IdGroup" });
            DropColumn("dbo.Activities", "IdGroup");
        }
    }
}
