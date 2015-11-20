namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForumAudit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Title", c => c.String());
            AddColumn("dbo.ForumPosts", "IsEdited", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ForumPosts", "IsEdited");
            DropColumn("dbo.AspNetUsers", "Title");
        }
    }
}
