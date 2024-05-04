namespace Aroma.BussinesLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewFix1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.USupportForms", "SupportUserId", "dbo.UDbTables");
            AddColumn("dbo.RatingUdbTables", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.USupportForms", "UDbTable_Id", c => c.Int());
            AddColumn("dbo.USupportForms", "UDbTable_Id1", c => c.Int());
            CreateIndex("dbo.RatingUdbTables", "UserId");
            CreateIndex("dbo.USupportForms", "UDbTable_Id");
            CreateIndex("dbo.USupportForms", "UDbTable_Id1");
            AddForeignKey("dbo.RatingUdbTables", "UserId", "dbo.UDbTables", "Id", cascadeDelete: true);
            AddForeignKey("dbo.USupportForms", "UDbTable_Id1", "dbo.UDbTables", "Id");
            AddForeignKey("dbo.USupportForms", "UDbTable_Id", "dbo.UDbTables", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.USupportForms", "UDbTable_Id", "dbo.UDbTables");
            DropForeignKey("dbo.USupportForms", "UDbTable_Id1", "dbo.UDbTables");
            DropForeignKey("dbo.RatingUdbTables", "UserId", "dbo.UDbTables");
            DropIndex("dbo.USupportForms", new[] { "UDbTable_Id1" });
            DropIndex("dbo.USupportForms", new[] { "UDbTable_Id" });
            DropIndex("dbo.RatingUdbTables", new[] { "UserId" });
            DropColumn("dbo.USupportForms", "UDbTable_Id1");
            DropColumn("dbo.USupportForms", "UDbTable_Id");
            DropColumn("dbo.RatingUdbTables", "UserId");
            AddForeignKey("dbo.USupportForms", "SupportUserId", "dbo.UDbTables", "Id", cascadeDelete: true);
        }
    }
}
