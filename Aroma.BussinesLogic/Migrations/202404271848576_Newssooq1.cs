namespace Aroma.BussinesLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Newssooq1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductDbTables", "orderStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductDbTables", "orderStatus");
        }
    }
}
