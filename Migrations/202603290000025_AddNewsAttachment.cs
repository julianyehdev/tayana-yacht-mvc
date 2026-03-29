namespace TayanaYachtMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewsAttachment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewsAttachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NewsArticleId = c.Int(nullable: false),
                        FileUrl = c.String(nullable: false, maxLength: 500),
                        FileName = c.String(nullable: false, maxLength: 200),
                        SortOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NewsArticles", t => t.NewsArticleId, cascadeDelete: true)
                .Index(t => t.NewsArticleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NewsAttachments", "NewsArticleId", "dbo.NewsArticles");
            DropIndex("dbo.NewsAttachments", new[] { "NewsArticleId" });
            DropTable("dbo.NewsAttachments");
        }
    }
}
