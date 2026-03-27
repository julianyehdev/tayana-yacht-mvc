namespace TayanaYachtMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddYachtSpecSheet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Yachts", "SpecSheetUrl", c => c.String());
            AddColumn("dbo.Yachts", "SpecSheetFileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Yachts", "SpecSheetFileName");
            DropColumn("dbo.Yachts", "SpecSheetUrl");
        }
    }
}
