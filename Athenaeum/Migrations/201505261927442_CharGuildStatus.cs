namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CharGuildStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characters", "Status", c => c.String());
            AddColumn("dbo.Guilds", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Guilds", "Status");
            DropColumn("dbo.Characters", "Status");
        }
    }
}
