namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForumThreadUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ForumThreads", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.ForumThreads", "IsSticky", c => c.Boolean(nullable: false));
            AddColumn("dbo.ForumThreads", "IsLocked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ForumThreads", "IsLocked");
            DropColumn("dbo.ForumThreads", "IsSticky");
            DropColumn("dbo.ForumThreads", "UpdatedDate");
        }
    }
}
