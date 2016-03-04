namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreBattleFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Battles", "Description", c => c.String());
            AddColumn("dbo.Battles", "ReporterId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Battles", "ReporterId");
            DropColumn("dbo.Battles", "Description");
        }
    }
}
