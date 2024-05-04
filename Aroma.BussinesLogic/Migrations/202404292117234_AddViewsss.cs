namespace Aroma.BussinesLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddViewsss : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductDbTables", "Code", c => c.Int(nullable: false));
            AddColumn("dbo.UDbTables", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UDbTables", "Code");
            DropColumn("dbo.ProductDbTables", "Code");
        }
    }
}
