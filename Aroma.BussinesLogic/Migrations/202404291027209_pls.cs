namespace Aroma.BussinesLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pls : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductDbTables", "Discount", c => c.Int(nullable: false));
            AlterColumn("dbo.ProductDbTables", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductDbTables", "Price", c => c.Int(nullable: false));
            DropColumn("dbo.ProductDbTables", "Discount");
        }
    }
}
