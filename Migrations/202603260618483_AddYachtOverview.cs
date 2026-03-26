namespace TayanaYachtMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddYachtOverview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Yachts", "Overview", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Yachts", "Overview");
        }
    }
}
