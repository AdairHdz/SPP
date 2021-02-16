namespace DataPersistenceLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetDateTimesToNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Coordinators", "DischargeDate", c => c.DateTime());
            AlterColumn("dbo.Reports", "CompletionDate", c => c.DateTime());
            AlterColumn("dbo.Reports", "DeliverDate", c => c.DateTime());
            AlterColumn("dbo.Teachers", "RegistrationDate", c => c.DateTime());
            AlterColumn("dbo.Teachers", "DischargeDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Teachers", "DischargeDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Teachers", "RegistrationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Reports", "DeliverDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Reports", "CompletionDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Coordinators", "DischargeDate", c => c.DateTime(nullable: false));
        }
    }
}
