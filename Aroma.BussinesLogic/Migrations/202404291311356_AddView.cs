namespace Aroma.BussinesLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddView : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductDbTables", "View", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductDbTables", "View");
        }
    }
}
