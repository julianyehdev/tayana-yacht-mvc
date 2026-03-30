using System.Web.Mvc;

namespace TayanaYachtMVC.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Article(string id)
        {
            return View();
        }


    }
}