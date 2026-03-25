namespace TayanaYachtMVC.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TayanaYachtMVC.Models.Domain;

    internal sealed class Configuration : DbMigrationsConfiguration<TayanaYachtMVC.Data.TayanaYachtDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TayanaYachtMVC.Data.TayanaYachtDBContext context)
        {
            // Countries
            context.Countries.AddOrUpdate(
                c => c.CountryName,
                new Country { CountryName = "Taiwan", SortOrder = 1 },
                new Country { CountryName = "Japan", SortOrder = 2 }
            );
            context.SaveChanges();

            var taiwan = context.Countries.First(c => c.CountryName == "Taiwan");
            var japan = context.Countries.First(c => c.CountryName == "Japan");

            // Regions
            context.Regions.AddOrUpdate(
                r => r.RegionName,
                new Region { RegionName = "North Taiwan", CountryId = taiwan.Id },
                new Region { RegionName = "South Taiwan", CountryId = taiwan.Id },
                new Region { RegionName = "Tokyo", CountryId = japan.Id }
            );
            context.SaveChanges();

            var northTaiwan = context.Regions.First(r => r.RegionName == "North Taiwan");

            // Yachts
            context.Yachts.AddOrUpdate(
                y => y.YachtName,
                new Yacht { YachtName = "Tayana 37", IsLatest = false },
                new Yacht { YachtName = "Tayana 48", IsLatest = true },
                new Yacht { YachtName = "Tayana 58", IsLatest = false }
            );

            // NewsArticles
            context.NewsArticles.AddOrUpdate(
                n => n.Title,
                new NewsArticle
                {
                    Title = "Tayana 48 正式發表",
                    Summary = "全新 Tayana 48 於台北國際遊艇展正式亮相。",
                    Content = "詳細內容...",
                    PublishDate = new DateTime(2026, 1, 10),
                    IsPublished = true,
                    IsPinned = true
                },
                new NewsArticle
                {
                    Title = "2026 遊艇展參展公告",
                    Summary = "大洋遊艇將於 3 月參加高雄遊艇展。",
                    Content = "詳細內容...",
                    PublishDate = new DateTime(2026, 2, 1),
                    IsPublished = true,
                    IsPinned = false
                },
                new NewsArticle
                {
                    Title = "春季保養優惠活動",
                    Summary = "即日起享春季保養 8 折優惠。",
                    Content = "詳細內容...",
                    PublishDate = new DateTime(2026, 3, 1),
                    IsPublished = false,
                    IsPinned = false
                }
            );

            // Dealers
            context.Dealers.AddOrUpdate(
                d => d.SortOrder,
                new Dealer { RegionId = northTaiwan.Id, IsPublished = true, SortOrder = 1 },
                new Dealer { RegionId = northTaiwan.Id, IsPublished = true, SortOrder = 2 }
            );

            context.SaveChanges();
        }
    }
}
