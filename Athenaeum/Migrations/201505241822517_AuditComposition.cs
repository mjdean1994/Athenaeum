namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuditComposition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Compositions", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Compositions", "UpdatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Compositions", "UpdatedDate");
            DropColumn("dbo.Compositions", "CreatedDate");
        }
    }
}
