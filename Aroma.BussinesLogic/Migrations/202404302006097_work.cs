namespace Aroma.BussinesLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class work : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ImageUrl");
        }
    }
}
