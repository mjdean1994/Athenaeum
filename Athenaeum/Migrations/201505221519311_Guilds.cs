namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Guilds : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Guilds",
                c => new
                    {
                        GuildId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Faction = c.String(),
                        Tagline = c.String(),
                        Introduction = c.String(),
                        Background = c.String(),
                        OocInformation = c.String(),
                        OwnerId = c.String(maxLength: 128),
                        ImageUrl = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GuildId)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId)
                .Index(t => t.OwnerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Guilds", "OwnerId", "dbo.AspNetUsers");
            DropIndex("dbo.Guilds", new[] { "OwnerId" });
            DropTable("dbo.Guilds");
        }
    }
}
