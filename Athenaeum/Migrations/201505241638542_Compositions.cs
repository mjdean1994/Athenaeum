namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Compositions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Compositions",
                c => new
                    {
                        CompositionId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Body = c.String(),
                        Type = c.String(),
                        IsFeatured = c.Boolean(nullable: false),
                        AuthorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CompositionId)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId)
                .Index(t => t.AuthorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Compositions", "AuthorId", "dbo.AspNetUsers");
            DropIndex("dbo.Compositions", new[] { "AuthorId" });
            DropTable("dbo.Compositions");
        }
    }
}
