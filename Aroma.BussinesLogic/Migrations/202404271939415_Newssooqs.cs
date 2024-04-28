namespace Aroma.BussinesLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Newssooqs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Feedback", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Feedback");
        }
    }
}
