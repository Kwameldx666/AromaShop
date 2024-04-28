namespace Aroma.BussinesLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReviewProducts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "AverageRating", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "AverageRating");
        }
    }
}
