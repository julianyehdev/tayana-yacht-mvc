namespace TayanaYachtMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddYachtDetailSpecification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Yachts", "DetailSpecification", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Yachts", "DetailSpecification");
        }
    }
}
