namespace DataPersistenceLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActivityPracticioner : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Documents", "Enrollment", "dbo.Practicioners");
            DropIndex("dbo.Documents", new[] { "Enrollment" });
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
                    })
                .PrimaryKey(t => t.IdActivity)
                .ForeignKey("dbo.Teachers", t => t.StaffNumberTeacher, cascadeDelete: true)
                .Index(t => t.StaffNumberTeacher);
            
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
                        IdDocument = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdActivityPracticioner)
                .ForeignKey("dbo.Activities", t => t.IdActivity, cascadeDelete: true)
                .ForeignKey("dbo.Documents", t => t.IdDocument, cascadeDelete: true)
                .ForeignKey("dbo.Practicioners", t => t.Enrollment)
                .Index(t => t.Enrollment)
                .Index(t => t.IdActivity)
                .Index(t => t.IdDocument);
            
            AlterColumn("dbo.Documents", "Name", c => c.String(maxLength: 100));
            DropColumn("dbo.Documents", "Enrollment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "Enrollment", c => c.String(maxLength: 10));
            DropForeignKey("dbo.ActivityPracticioners", "Enrollment", "dbo.Practicioners");
            DropForeignKey("dbo.ActivityPracticioners", "IdDocument", "dbo.Documents");
            DropForeignKey("dbo.ActivityPracticioners", "IdActivity", "dbo.Activities");
            DropForeignKey("dbo.Activities", "StaffNumberTeacher", "dbo.Teachers");
            DropIndex("dbo.ActivityPracticioners", new[] { "IdDocument" });
            DropIndex("dbo.ActivityPracticioners", new[] { "IdActivity" });
            DropIndex("dbo.ActivityPracticioners", new[] { "Enrollment" });
            DropIndex("dbo.Activities", new[] { "StaffNumberTeacher" });
            AlterColumn("dbo.Documents", "Name", c => c.String(maxLength: 50));
            DropTable("dbo.ActivityPracticioners");
            DropTable("dbo.Activities");
            CreateIndex("dbo.Documents", "Enrollment");
            AddForeignKey("dbo.Documents", "Enrollment", "dbo.Practicioners", "Enrollment");
        }
    }
}
