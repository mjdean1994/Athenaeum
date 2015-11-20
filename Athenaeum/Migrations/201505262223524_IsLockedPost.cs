namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsLockedPost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ForumPosts", "IsLocked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ForumPosts", "IsLocked");
        }
    }
}
