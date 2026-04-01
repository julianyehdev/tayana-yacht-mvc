using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TayanaYachtMVC.Data;
using TayanaYachtMVC.Models.Domain;

namespace TayanaYachtMVC.Controllers
{
    public class NewsController : Controller
    {
        private readonly TayanaYachtDBContext _context;

        public NewsController()
        {
            _context = new TayanaYachtDBContext();
        }

        // GET: News
        public ActionResult Index(int page = 1)
        {
            const int pageSize = 5;

            var query = _context.NewsArticles
                .Where(x => x.IsPublished)
                .OrderByDescending(x => x.IsPinned)
                .ThenByDescending(x => x.PublishDate);

            var totalCount = query.Count();
            var totalPages = (totalCount + pageSize - 1) / pageSize;

            // 驗證頁碼有效性
            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

            var articles = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalCount = totalCount;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return View(articles);
        }

        public ActionResult Article(int? id)
        {
            if (!id.HasValue)
            {
                // 沒有 id 時，取得第一筆已發布的文章（置頂優先，然後最新）
                var firstArticle = _context.NewsArticles
                    .Where(x => x.IsPublished)
                    .OrderByDescending(x => x.IsPinned)
                    .ThenByDescending(x => x.PublishDate)
                    .FirstOrDefault();

                if (firstArticle == null)
                    return HttpNotFound();

                return RedirectToAction("Article", new { id = firstArticle.Id });
            }

            var article = _context.NewsArticles
                .Include(x => x.Attachments)
                .FirstOrDefault(x => x.Id == id && x.IsPublished);

            if (article == null)
                return HttpNotFound();

            return View(article);
        }


    }
}