using System.Data.Entity;
using System.Linq;
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
                    IsLatest = y.IsLatest,
                    FirstPhotoUrl = y.YachtPhotos
                        .OrderBy(p => p.SortOrder)
                        .Select(p => p.PhotoUrl)
                        .FirstOrDefault()
                })
                .ToList();

            var viewModel = new HomeIndexViewModel { Yachts = yachts };
            return View(viewModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
