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
        public ActionResult Overview(string yachtName, string modelNumber = null)
        {
            var yacht = string.IsNullOrEmpty(yachtName)
                ? _context.Yachts.Include(y => y.YachtPhotos).FirstOrDefault()
                : _context.Yachts.Include(y => y.YachtPhotos)
                    .FirstOrDefault(y => y.YachtName == yachtName &&
                        (string.IsNullOrEmpty(modelNumber) || y.ModelNumber == modelNumber));

            if (yacht == null)
                return HttpNotFound();

            ViewBag.AllYachts = _context.Yachts.OrderBy(y => y.YachtID).ToList();
            return View(yacht);
        }


        public ActionResult DeckPlan(string yachtName, string modelNumber = null)
        {
            var yacht = string.IsNullOrEmpty(yachtName)
                ? _context.Yachts.Include(y => y.YachtPhotos).Include(y => y.YachtLayoutPhotos).FirstOrDefault()
                : _context.Yachts.Include(y => y.YachtPhotos).Include(y => y.YachtLayoutPhotos)
                    .FirstOrDefault(y => y.YachtName == yachtName &&
                        (string.IsNullOrEmpty(modelNumber) || y.ModelNumber == modelNumber));

            if (yacht == null)
                return HttpNotFound();

            ViewBag.AllYachts = _context.Yachts.OrderBy(y => y.YachtID).ToList();
            return View(yacht);
        }




        public ActionResult Specification(string _id)
        {
            // _id = "Tayana 58" or "Tayana 78" or "Tayana 84"
            return View();
        }

    }
}