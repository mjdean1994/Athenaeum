namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContinentToWarZone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WarZones", "Continent", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WarZones", "Continent");
        }
    }
}
