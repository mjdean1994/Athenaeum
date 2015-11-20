namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FkUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        AuthorId = c.String(),
                        Body = c.String(),
                        PostedDate = c.DateTime(nullable: false),
                        EntityId = c.Int(nullable: false),
                        CommentTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.CommentTypes", t => t.CommentTypeId, cascadeDelete: true)
                .Index(t => t.CommentTypeId);
            
            CreateTable(
                "dbo.CommentTypes",
                c => new
                    {
                        CommentTypeId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.CommentTypeId);
            
            CreateTable(
                "dbo.NewsArticles",
                c => new
                    {
                        NewsArticleId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        AuthorId = c.String(maxLength: 128),
                        PostedDate = c.DateTime(nullable: false),
                        Body = c.String(),
                        NewsCategoryId = c.Int(nullable: false),
                        ImageUrl = c.String(),
                        NumberOfComments = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NewsArticleId)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId)
                .ForeignKey("dbo.NewsCategories", t => t.NewsCategoryId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.NewsCategoryId);
            
            CreateTable(
                "dbo.NewsCategories",
                c => new
                    {
                        NewsCategoryId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.NewsCategoryId);
            
            AddColumn("dbo.AspNetUsers", "BattleTag", c => c.String());
            AddColumn("dbo.AspNetUsers", "Introduction", c => c.String());
            AddColumn("dbo.AspNetUsers", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NewsArticles", "NewsCategoryId", "dbo.NewsCategories");
            DropForeignKey("dbo.NewsArticles", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "CommentTypeId", "dbo.CommentTypes");
            DropIndex("dbo.NewsArticles", new[] { "NewsCategoryId" });
            DropIndex("dbo.NewsArticles", new[] { "AuthorId" });
            DropIndex("dbo.Comments", new[] { "CommentTypeId" });
            DropColumn("dbo.AspNetUsers", "ImageUrl");
            DropColumn("dbo.AspNetUsers", "Introduction");
            DropColumn("dbo.AspNetUsers", "BattleTag");
            DropTable("dbo.NewsCategories");
            DropTable("dbo.NewsArticles");
            DropTable("dbo.CommentTypes");
            DropTable("dbo.Comments");
        }
    }
}
