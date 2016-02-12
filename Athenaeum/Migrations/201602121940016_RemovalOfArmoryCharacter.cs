namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovalOfArmoryCharacter : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ArmoryCharacters", "CharacterId", "dbo.Characters");
            DropIndex("dbo.ArmoryCharacters", new[] { "CharacterId" });
            DropTable("dbo.ArmoryCharacters");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.ArmoryCharacterId);
            
            CreateIndex("dbo.ArmoryCharacters", "CharacterId");
            AddForeignKey("dbo.ArmoryCharacters", "CharacterId", "dbo.Characters", "CharacterId", cascadeDelete: true);
        }
    }
}
