namespace DataPersistenceLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateActvity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ActivityPracticioners", "IdDocument", "dbo.Documents");
            DropIndex("dbo.ActivityPracticioners", new[] { "IdDocument" });
            AddColumn("dbo.Documents", "IdActivityPracticioner", c => c.Int(nullable: false));
            CreateIndex("dbo.Documents", "IdActivityPracticioner");
            AddForeignKey("dbo.Documents", "IdActivityPracticioner", "dbo.ActivityPracticioners", "IdActivityPracticioner", cascadeDelete: true);
            DropColumn("dbo.ActivityPracticioners", "IdDocument");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ActivityPracticioners", "IdDocument", c => c.Int(nullable: false));
            DropForeignKey("dbo.Documents", "IdActivityPracticioner", "dbo.ActivityPracticioners");
            DropIndex("dbo.Documents", new[] { "IdActivityPracticioner" });
            DropColumn("dbo.Documents", "IdActivityPracticioner");
            CreateIndex("dbo.ActivityPracticioners", "IdDocument");
            AddForeignKey("dbo.ActivityPracticioners", "IdDocument", "dbo.Documents", "IdDocument", cascadeDelete: true);
        }
    }
}
