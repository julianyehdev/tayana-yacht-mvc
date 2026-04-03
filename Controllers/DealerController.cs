using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TayanaYachtMVC.Data;
using TayanaYachtMVC.Models.Domain;

namespace TayanaYachtMVC.Controllers
{
    public class DealerController : Controller
    {
        private readonly TayanaYachtDBContext _context;

        public DealerController()
        {
            _context = new TayanaYachtDBContext();
        }

        // GET: Dealer
        public ActionResult Index(int? countryId = null, int page = 1)
        {
            const int pageSize = 5;

            IQueryable<Dealer> query = _context.Dealers
                .Include(d => d.Region)
                .Include(d => d.Region.Country)
                .Where(d => d.IsPublished);

            if (countryId.HasValue)
            {
                query = query.Where(d => d.Region.CountryId == countryId);
            }

            var orderedQuery = query.OrderBy(d => d.SortOrder);

            var totalCount = orderedQuery.Count();
            var totalPages = (totalCount + pageSize - 1) / pageSize;

            // 驗證頁碼有效性
            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

            var dealers = orderedQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.AllCountries = _context.Countries.OrderBy(c => c.SortOrder).ToList();
            ViewBag.CountryId = countryId;
            ViewBag.TotalCount = totalCount;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            // 動態設定麵包屑顯示的國家名稱
            if (countryId.HasValue)
            {
                var country = _context.Countries.FirstOrDefault(c => c.Id == countryId);
                ViewBag.CurrentCountryName = country?.CountryName ?? "Dealers";
            }
            else
            {
                ViewBag.CurrentCountryName = "Dealers";
            }

            return View(dealers);
        }
    }
}