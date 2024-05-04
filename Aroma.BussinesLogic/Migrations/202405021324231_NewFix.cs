namespace Aroma.BussinesLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewFix : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RatingUdbTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductDbTables", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RatingUdbTables", "ProductId", "dbo.ProductDbTables");
            DropIndex("dbo.RatingUdbTables", new[] { "ProductId" });
            DropTable("dbo.RatingUdbTables");
        }
    }
}
