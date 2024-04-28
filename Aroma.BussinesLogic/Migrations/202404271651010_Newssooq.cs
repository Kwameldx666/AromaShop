namespace Aroma.BussinesLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Newssooq : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductDbTables", "FeedBack", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductDbTables", "FeedBack");
        }
    }
}
