namespace TayanaYachtMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDealerNameField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dealers", "Name", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dealers", "Name");
        }
    }
}
