namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Forums : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ForumCategories",
                c => new
                    {
                        ForumCategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ForumCategoryId);
            
            CreateTable(
                "dbo.ForumSections",
                c => new
                    {
                        ForumSectionId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        ForumCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ForumSectionId)
                .ForeignKey("dbo.ForumCategories", t => t.ForumCategoryId, cascadeDelete: true)
                .Index(t => t.ForumCategoryId);
            
            CreateTable(
                "dbo.ForumThreads",
                c => new
                    {
                        ForumThreadId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ForumSectionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ForumThreadId)
                .ForeignKey("dbo.ForumSections", t => t.ForumSectionId, cascadeDelete: true)
                .Index(t => t.ForumSectionId);
            
            CreateTable(
                "dbo.ForumPosts",
                c => new
                    {
                        ForumPostId = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                        OwnerId = c.String(maxLength: 128),
                        ForumThreadId = c.Int(nullable: false),
                        PostedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ForumPostId)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId)
                .ForeignKey("dbo.ForumThreads", t => t.ForumThreadId, cascadeDelete: true)
                .Index(t => t.OwnerId)
                .Index(t => t.ForumThreadId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ForumThreads", "ForumSectionId", "dbo.ForumSections");
            DropForeignKey("dbo.ForumPosts", "ForumThreadId", "dbo.ForumThreads");
            DropForeignKey("dbo.ForumPosts", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ForumSections", "ForumCategoryId", "dbo.ForumCategories");
            DropIndex("dbo.ForumThreads", new[] { "ForumSectionId" });
            DropIndex("dbo.ForumPosts", new[] { "ForumThreadId" });
            DropIndex("dbo.ForumPosts", new[] { "OwnerId" });
            DropIndex("dbo.ForumSections", new[] { "ForumCategoryId" });
            DropTable("dbo.ForumPosts");
            DropTable("dbo.ForumThreads");
            DropTable("dbo.ForumSections");
            DropTable("dbo.ForumCategories");
        }
    }
}
