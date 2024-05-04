namespace Aroma.BussinesLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class review : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RatingUdbTables", "Feedback", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RatingUdbTables", "Feedback");
        }
    }
}
