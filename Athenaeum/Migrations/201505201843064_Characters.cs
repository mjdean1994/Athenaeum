namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Characters : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Characters",
                c => new
                    {
                        CharacterId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FullName = c.String(),
                        Race = c.String(),
                        Class = c.String(),
                        GuildId = c.Int(nullable: false),
                        Introduction = c.String(),
                        Personality = c.String(),
                        Appearance = c.String(),
                        History = c.String(),
                        ImageUrl = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CharacterId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Characters");
        }
    }
}
