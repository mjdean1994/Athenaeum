namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UnlinkGuild : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characters", "Guild", c => c.String());
            DropColumn("dbo.Characters", "GuildId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Characters", "GuildId", c => c.Int(nullable: false));
            DropColumn("dbo.Characters", "Guild");
        }
    }
}
