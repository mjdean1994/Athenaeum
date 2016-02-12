namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GetWorkingForPhase3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArmoryCharacters",
                c => new
                    {
                        ArmoryCharacterId = c.Int(nullable: false, identity: true),
                        AchievementPoints = c.Int(nullable: false),
                        Class = c.String(),
                        PrimarySpec = c.String(),
                        SecondarySpec = c.String(),
                        Rating2v2 = c.Int(nullable: false),
                        Rating3v3 = c.Int(nullable: false),
                        Rating5v5 = c.Int(nullable: false),
                        RatingRbg = c.Int(nullable: false),
                        CharacterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ArmoryCharacterId)
                .ForeignKey("dbo.Characters", t => t.CharacterId, cascadeDelete: true)
                .Index(t => t.CharacterId);
            
            AddColumn("dbo.Characters", "ArmoryCharacterId", c => c.Int());
            AddColumn("dbo.Characters", "LastPull", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArmoryCharacters", "CharacterId", "dbo.Characters");
            DropIndex("dbo.ArmoryCharacters", new[] { "CharacterId" });
            DropColumn("dbo.Characters", "LastPull");
            DropColumn("dbo.Characters", "ArmoryCharacterId");
            DropTable("dbo.ArmoryCharacters");
        }
    }
}
