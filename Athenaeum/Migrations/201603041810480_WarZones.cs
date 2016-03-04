namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WarZones : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Battles",
                c => new
                    {
                        BattleId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AllianceCommander = c.String(),
                        HordeCommander = c.String(),
                        AllianceForces = c.Int(nullable: false),
                        HordeForces = c.Int(nullable: false),
                        Outcome = c.String(),
                        Date = c.DateTime(nullable: false),
                        WarZoneId = c.Int(nullable: false),
                        NewInfluence = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BattleId)
                .ForeignKey("dbo.WarZones", t => t.WarZoneId, cascadeDelete: true)
                .Index(t => t.WarZoneId);
            
            CreateTable(
                "dbo.WarZones",
                c => new
                    {
                        WarZoneId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AllianceForce = c.String(),
                        HordeForce = c.String(),
                        Influence = c.Int(nullable: false),
                        Limit = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.WarZoneId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Battles", "WarZoneId", "dbo.WarZones");
            DropIndex("dbo.Battles", new[] { "WarZoneId" });
            DropTable("dbo.WarZones");
            DropTable("dbo.Battles");
        }
    }
}
