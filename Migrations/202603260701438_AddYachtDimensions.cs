namespace TayanaYachtMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddYachtDimensions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Yachts", "Dimensions", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Yachts", "Dimensions");
        }
    }
}
