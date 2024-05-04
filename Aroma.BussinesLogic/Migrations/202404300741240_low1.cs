namespace Aroma.BussinesLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class low1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ProductDbTables", "Code");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductDbTables", "Code", c => c.Int(nullable: false));
        }
    }
}
