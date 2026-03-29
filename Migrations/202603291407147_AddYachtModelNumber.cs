namespace TayanaYachtMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddYachtModelNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Yachts", "ModelNumber", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Yachts", "ModelNumber");
        }
    }
}
