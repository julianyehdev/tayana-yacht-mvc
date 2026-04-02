using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TayanaYachtMVC.Data;

namespace TayanaYachtMVC.Areas.Admin.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly TayanaYachtDBContext _db = new TayanaYachtDBContext();

        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            ViewBag.YachtCount = _db.Yachts.Count();
            ViewBag.NewsCount = _db.NewsArticles.Count();
            ViewBag.DealerCount = _db.Dealers.Count();

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }
    }
}