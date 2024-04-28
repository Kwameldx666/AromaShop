namespace Aroma.BussinesLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Newssoo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.USupportForms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SupportUserId = c.Int(nullable: false),
                        name = c.String(),
                        email = c.String(),
                        subject = c.String(),
                        message = c.String(),
                        MessageTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UDbTables", t => t.SupportUserId, cascadeDelete: true)
                .Index(t => t.SupportUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.USupportForms", "SupportUserId", "dbo.UDbTables");
            DropIndex("dbo.USupportForms", new[] { "SupportUserId" });
            DropTable("dbo.USupportForms");
        }
    }
}
