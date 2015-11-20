namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pictures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        PictureId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        OwnerId = c.String(maxLength: 128),
                        ImageUrl = c.String(),
                        Type = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PictureId)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId)
                .Index(t => t.OwnerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pictures", "OwnerId", "dbo.AspNetUsers");
            DropIndex("dbo.Pictures", new[] { "OwnerId" });
            DropTable("dbo.Pictures");
        }
    }
}
