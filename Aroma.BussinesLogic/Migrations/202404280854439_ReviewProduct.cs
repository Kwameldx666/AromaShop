namespace Aroma.BussinesLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReviewProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Reting", c => c.Int(nullable: false));
            AddColumn("dbo.ProductDbTables", "Reting", c => c.Int(nullable: false));
            AddColumn("dbo.ProductDbTables", "AverageRating", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductDbTables", "AverageRating");
            DropColumn("dbo.ProductDbTables", "Reting");
            DropColumn("dbo.Orders", "Reting");
        }
    }
}
