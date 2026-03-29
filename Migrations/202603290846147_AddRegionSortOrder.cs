namespace TayanaYachtMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRegionSortOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Regions", "SortOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Regions", "SortOrder");
        }
    }
}
