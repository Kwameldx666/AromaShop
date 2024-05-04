namespace Aroma.BussinesLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class review2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "RatingUdbTable_Id", c => c.Int());
            CreateIndex("dbo.Orders", "RatingUdbTable_Id");
            AddForeignKey("dbo.Orders", "RatingUdbTable_Id", "dbo.RatingUdbTables", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "RatingUdbTable_Id", "dbo.RatingUdbTables");
            DropIndex("dbo.Orders", new[] { "RatingUdbTable_Id" });
            DropColumn("dbo.Orders", "RatingUdbTable_Id");
        }
    }
}
