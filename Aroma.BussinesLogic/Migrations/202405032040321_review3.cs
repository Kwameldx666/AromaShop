namespace Aroma.BussinesLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class review3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "AverageRating", c => c.Double(nullable: false));
            AlterColumn("dbo.ProductDbTables", "AverageRating", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductDbTables", "AverageRating", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Orders", "AverageRating", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
