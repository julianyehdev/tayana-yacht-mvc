using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TayanaYachtMVC.Data;
using TayanaYachtMVC.Models.Domain;

namespace TayanaYachtMVC.Controllers
{
    public class ContactController : Controller
    {
        private readonly TayanaYachtDBContext _context;

        public ContactController()
        {
            _context = new TayanaYachtDBContext();
        }

        // GET: Contact
        public ActionResult Index()
        {
            var countries = _context.Countries.OrderBy(c => c.SortOrder).ToList();
            var yachts = _context.Yachts.OrderBy(y => y.YachtName).ToList();

            ViewBag.Countries = countries;
            ViewBag.Yachts = yachts;

            return View();
        }
    }
}