namespace TayanaYachtMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryName = c.String(nullable: false, maxLength: 100),
                        SortOrder = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Dealers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegionId = c.Int(nullable: false),
                        MainImageUrl = c.String(maxLength: 260),
                        DescriptionHtml = c.String(),
                        IsPublished = c.Boolean(nullable: false),
                        SortOrder = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Regions", t => t.RegionId, cascadeDelete: true)
                .Index(t => t.RegionId);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryId = c.Int(nullable: false),
                        RegionName = c.String(nullable: false, maxLength: 100),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.NewsArticles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        CoverImageUrl = c.String(maxLength: 500),
                        Summary = c.String(maxLength: 500),
                        Content = c.String(),
                        PublishDate = c.DateTime(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        IsPinned = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.AlbumID)
                .ForeignKey("dbo.Yachts", t => t.YachtID, cascadeDelete: true)
                .Index(t => t.YachtID, unique: true);
            
            CreateTable(
                "dbo.Yachts",
                c => new
                    {
                        YachtID = c.Int(nullable: false, identity: true),
                        YachtName = c.String(nullable: false, maxLength: 100),
                        IsLatest = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.YachtID);
            
            CreateTable(
                "dbo.YachtDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        YachtId = c.Int(nullable: false),
                        DocumentName = c.String(nullable: false, maxLength: 200),
                        FilePath = c.String(nullable: false, maxLength: 500),
                        UploadTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Yachts", t => t.YachtId, cascadeDelete: true)
                .Index(t => t.YachtId);
            
            CreateTable(
                "dbo.YachtsLayoutPhotos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        YachtId = c.Int(nullable: false),
                        LayoutTitle = c.String(nullable: false, maxLength: 100),
                        LayoutImgUrl = c.String(nullable: false, maxLength: 500),
                        SortOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Yachts", t => t.YachtId, cascadeDelete: true)
                .Index(t => t.YachtId);
            
            CreateTable(
                "dbo.YachtPhotos",
                c => new
                    {
                        PhotoID = c.Int(nullable: false, identity: true),
                        AlbumID = c.Int(nullable: false),
                        FilePath = c.String(nullable: false, maxLength: 500),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PhotoID)
                .ForeignKey("dbo.YachtAlbums", t => t.AlbumID, cascadeDelete: true)
                .Index(t => t.AlbumID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.YachtPhotos", "AlbumID", "dbo.YachtAlbums");
            DropForeignKey("dbo.YachtsLayoutPhotos", "YachtId", "dbo.Yachts");
            DropForeignKey("dbo.YachtDocuments", "YachtId", "dbo.Yachts");
            DropForeignKey("dbo.YachtAlbums", "YachtID", "dbo.Yachts");
            DropForeignKey("dbo.Dealers", "RegionId", "dbo.Regions");
            DropForeignKey("dbo.Regions", "CountryId", "dbo.Countries");
            DropIndex("dbo.YachtPhotos", new[] { "AlbumID" });
            DropIndex("dbo.YachtsLayoutPhotos", new[] { "YachtId" });
            DropIndex("dbo.YachtDocuments", new[] { "YachtId" });
            DropIndex("dbo.YachtAlbums", new[] { "YachtID" });
            DropIndex("dbo.Regions", new[] { "CountryId" });
            DropIndex("dbo.Dealers", new[] { "RegionId" });
            DropTable("dbo.YachtPhotos");
            DropTable("dbo.YachtsLayoutPhotos");
            DropTable("dbo.YachtDocuments");
            DropTable("dbo.Yachts");
            DropTable("dbo.YachtAlbums");
            DropTable("dbo.NewsArticles");
            DropTable("dbo.Regions");
            DropTable("dbo.Dealers");
            DropTable("dbo.Countries");
        }
    }
}
