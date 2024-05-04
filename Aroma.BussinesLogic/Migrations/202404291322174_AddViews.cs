namespace Aroma.BussinesLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddViews : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductDbTables", "Quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductDbTables", "Quantity");
        }
    }
}
