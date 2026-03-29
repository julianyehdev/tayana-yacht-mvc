namespace TayanaYachtMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewsCategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewsCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        SortOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.NewsArticles", "CategoryId", c => c.Int());
            CreateIndex("dbo.NewsArticles", "CategoryId");
            AddForeignKey("dbo.NewsArticles", "CategoryId", "dbo.NewsCategories", "Id");
            DropColumn("dbo.NewsArticles", "Summary");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NewsArticles", "Summary", c => c.String(maxLength: 500));
            DropForeignKey("dbo.NewsArticles", "CategoryId", "dbo.NewsCategories");
            DropIndex("dbo.NewsArticles", new[] { "CategoryId" });
            DropColumn("dbo.NewsArticles", "CategoryId");
            DropTable("dbo.NewsCategories");
        }
    }
}
