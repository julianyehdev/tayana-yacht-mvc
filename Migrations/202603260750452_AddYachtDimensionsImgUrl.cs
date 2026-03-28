namespace TayanaYachtMVC.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddYachtDimensionsImgUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Yachts", "DimensionsImgUrl", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Yachts", "DimensionsImgUrl");
        }
    }
}
