namespace Aroma.BussinesLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class low : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductDbTables", "PriceWithDiscount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductDbTables", "PriceWithDiscount");
        }
    }
}
