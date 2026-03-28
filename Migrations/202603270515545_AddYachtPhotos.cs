namespace TayanaYachtMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddYachtPhotos : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.YachtAlbums", "YachtID", "dbo.Yachts");
            DropForeignKey("dbo.YachtPhotos", "AlbumID", "dbo.YachtAlbums");
            DropIndex("dbo.YachtAlbums", new[] { "YachtID" });
            DropIndex("dbo.YachtPhotos", new[] { "AlbumID" });
            AddColumn("dbo.YachtPhotos", "YachtID", c => c.Int(nullable: false));
            AddColumn("dbo.YachtPhotos", "PhotoUrl", c => c.String(nullable: false, maxLength: 500));
            AddColumn("dbo.YachtPhotos", "SortOrder", c => c.Int(nullable: false));
            CreateIndex("dbo.YachtPhotos", "YachtID");
            AddForeignKey("dbo.YachtPhotos", "YachtID", "dbo.Yachts", "YachtID", cascadeDelete: true);
            DropColumn("dbo.YachtPhotos", "AlbumID");
            DropColumn("dbo.YachtPhotos", "FilePath");
            DropColumn("dbo.YachtPhotos", "CreatedAt");
            DropColumn("dbo.YachtPhotos", "UpdatedAt");
            DropTable("dbo.YachtAlbums");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.YachtAlbums",
                c => new
                    {
                        AlbumID = c.Int(nullable: false, identity: true),
                        YachtID = c.Int(nullable: false),
                        AlbumName = c.String(nullable: false, maxLength: 100),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AlbumID);
            
            AddColumn("dbo.YachtPhotos", "UpdatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.YachtPhotos", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.YachtPhotos", "FilePath", c => c.String(nullable: false, maxLength: 500));
            AddColumn("dbo.YachtPhotos", "AlbumID", c => c.Int(nullable: false));
            DropForeignKey("dbo.YachtPhotos", "YachtID", "dbo.Yachts");
            DropIndex("dbo.YachtPhotos", new[] { "YachtID" });
            DropColumn("dbo.YachtPhotos", "SortOrder");
            DropColumn("dbo.YachtPhotos", "PhotoUrl");
            DropColumn("dbo.YachtPhotos", "YachtID");
            CreateIndex("dbo.YachtPhotos", "AlbumID");
            CreateIndex("dbo.YachtAlbums", "YachtID", unique: true);
            AddForeignKey("dbo.YachtPhotos", "AlbumID", "dbo.YachtAlbums", "AlbumID", cascadeDelete: true);
            AddForeignKey("dbo.YachtAlbums", "YachtID", "dbo.Yachts", "YachtID", cascadeDelete: true);
        }
    }
}
