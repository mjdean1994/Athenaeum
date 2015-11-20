namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategorySortOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ForumCategories", "SortOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ForumCategories", "SortOrder");
        }
    }
}
