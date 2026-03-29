using System.Web.Mvc;

namespace TayanaYachtMVC.Controllers
{
    public class YachtController : Controller
    {
        // GET: Yacht
        public ActionResult Overview(string id)
        {
            // id = "Tayana 58" or "Tayana 78" or "Tayana 84"
            return View();
        }

        public ActionResult Layout(string id)
        {
            // id = "Tayana 58" or "Tayana 78" or "Tayana 84"
            return View();
        }

        public ActionResult Specification(string id)
        {
            // id = "Tayana 58" or "Tayana 78" or "Tayana 84"
            return View();
        }

    }
}