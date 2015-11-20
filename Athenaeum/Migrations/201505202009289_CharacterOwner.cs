namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CharacterOwner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characters", "OwnerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Characters", "OwnerId");
            AddForeignKey("dbo.Characters", "OwnerId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Characters", "OwnerId", "dbo.AspNetUsers");
            DropIndex("dbo.Characters", new[] { "OwnerId" });
            DropColumn("dbo.Characters", "OwnerId");
        }
    }
}
