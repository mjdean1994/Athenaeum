namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rsvp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rsvps",
                c => new
                    {
                        RsvpId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        EventId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RsvpId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.EventId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rsvps", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Rsvps", "EventId", "dbo.Events");
            DropIndex("dbo.Rsvps", new[] { "EventId" });
            DropIndex("dbo.Rsvps", new[] { "UserId" });
            DropTable("dbo.Rsvps");
        }
    }
}
