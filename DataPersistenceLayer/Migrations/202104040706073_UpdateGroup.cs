namespace DataPersistenceLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateGroup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Practicioners", "Nrc", "dbo.Groups");
            DropIndex("dbo.Practicioners", new[] { "Nrc" });
            RenameColumn(table: "dbo.Practicioners", name: "Nrc", newName: "IdGroup");
            DropPrimaryKey("dbo.Groups");
            AddColumn("dbo.Groups", "IdGroup", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Groups", "StaffNumber", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Practicioners", "IdGroup", c => c.Int(nullable: true));
            AlterColumn("dbo.Groups", "Nrc", c => c.String(nullable: false));
            AddPrimaryKey("dbo.Groups", "IdGroup");
            CreateIndex("dbo.Practicioners", "IdGroup");
            CreateIndex("dbo.Groups", "StaffNumber");
            AddForeignKey("dbo.Groups", "StaffNumber", "dbo.Teachers", "StaffNumber", cascadeDelete: true);
            AddForeignKey("dbo.Practicioners", "IdGroup", "dbo.Groups", "IdGroup", cascadeDelete: false);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Practicioners", "IdGroup", "dbo.Groups");
            DropForeignKey("dbo.Groups", "StaffNumber", "dbo.Teachers");
            DropIndex("dbo.Groups", new[] { "StaffNumber" });
            DropIndex("dbo.Practicioners", new[] { "IdGroup" });
            DropPrimaryKey("dbo.Groups");
            AlterColumn("dbo.Groups", "Nrc", c => c.String(nullable: false, maxLength: 5));
            AlterColumn("dbo.Practicioners", "IdGroup", c => c.String(maxLength: 5));
            DropColumn("dbo.Groups", "StaffNumber");
            DropColumn("dbo.Groups", "IdGroup");
            AddPrimaryKey("dbo.Groups", "Nrc");
            RenameColumn(table: "dbo.Practicioners", name: "IdGroup", newName: "Nrc");
            CreateIndex("dbo.Practicioners", "Nrc");
            AddForeignKey("dbo.Practicioners", "Nrc", "dbo.Groups", "Nrc");
        }
    }
}
