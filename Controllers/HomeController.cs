using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using TayanaYachtMVC.Data;
using TayanaYachtMVC.Models.ViewModels;

namespace TayanaYachtMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly TayanaYachtDBContext _db = new TayanaYachtDBContext();

        public ActionResult Index()
        {
            var yachts = _db.Yachts
                .Include(y => y.YachtPhotos)
                .OrderBy(y => y.YachtID)
                .ToList()
                .Select(y => new YachtBannerItem
                {
                    YachtID = y.YachtID,
                    YachtName = y.YachtName,
                    ModelNumber = y.ModelNumber,
                    IsLatest = y.IsLatest,
                    FirstPhotoUrl = y.YachtPhotos
                        .OrderBy(p => p.SortOrder)
                        .Select(p => p.PhotoUrl)
                        .FirstOrDefault()
                })
                .ToList();

            var news = _db.NewsArticles
                .Where(n => n.IsPublished)
                .OrderByDescending(n => n.IsPinned)
                .ThenByDescending(n => n.PublishDate)
                .Take(3)
                .ToList()
                .Select(n => new NewsArticleItem
                {
                    Title = n.Title,
                    CoverImageUrl = n.CoverImageUrl,
                    // 用 Regex 去除所有 HTML 標籤
                    PlainTextContent = Regex.Replace(n.Content ?? "", "<[^>]+>", "").Trim()
                })
                .ToList();

            var viewModel = new HomeIndexViewModel
            {
                Yachts = yachts,
                News = news
            };
            return View(viewModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
