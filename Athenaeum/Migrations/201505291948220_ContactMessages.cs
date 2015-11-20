namespace Athenaeum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactMessages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactMessages",
                c => new
                    {
                        ContactMessageId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Body = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        AnsweredDate = c.DateTime(),
                        AnsweredById = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ContactMessageId)
                .ForeignKey("dbo.AspNetUsers", t => t.AnsweredById)
                .Index(t => t.AnsweredById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContactMessages", "AnsweredById", "dbo.AspNetUsers");
            DropIndex("dbo.ContactMessages", new[] { "AnsweredById" });
            DropTable("dbo.ContactMessages");
        }
    }
}
