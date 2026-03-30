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
        public ActionResult Index(int? countryId = null)
        {
            IQueryable<Dealer> query = _context.Dealers
                .Include(d => d.Region)
                .Include(d => d.Region.Country)
                .Where(d => d.IsPublished);

            if (countryId.HasValue)
            {
                query = query.Where(d => d.Region.CountryId == countryId);
            }

            var dealers = query.OrderBy(d => d.SortOrder).ToList();

            ViewBag.AllCountries = _context.Countries.OrderBy(c => c.SortOrder).ToList();

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