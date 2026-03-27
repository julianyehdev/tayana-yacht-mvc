namespace TayanaYachtMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveLayoutTitle : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.YachtsLayoutPhotos", "LayoutTitle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.YachtsLayoutPhotos", "LayoutTitle", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
