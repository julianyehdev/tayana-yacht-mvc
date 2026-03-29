using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TayanaYachtMVC.Data;

namespace TayanaYachtMVC.Controllers
{
    public class YachtController : Controller
    {
        private readonly TayanaYachtDBContext _context;

        public YachtController()
        {
            _context = new TayanaYachtDBContext();
        }

        // GET: Yacht
        public ActionResult Overview(string id)
        {
            // id = "Tayana 58" or "Tayana 78" or "Tayana 84"
            var yacht = string.IsNullOrEmpty(id)
                ? _context.Yachts.Include(y => y.YachtPhotos).FirstOrDefault()
                : _context.Yachts.Include(y => y.YachtPhotos).FirstOrDefault(y => y.YachtName == id);

            if (yacht == null)
                return HttpNotFound();

            ViewBag.AllYachts = _context.Yachts.OrderBy(y => y.YachtID).ToList();
            return View(yacht);
        }

        public ActionResult Layout(string _id)
        {
            // _id = "Tayana 58" or "Tayana 78" or "Tayana 84"
            return View();
        }

        public ActionResult Specification(string _id)
        {
            // _id = "Tayana 58" or "Tayana 78" or "Tayana 84"
            return View();
        }

    }
}