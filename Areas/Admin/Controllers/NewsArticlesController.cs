using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TayanaYachtMVC.Data;
using TayanaYachtMVC.Models.Domain;

namespace TayanaYachtMVC.Areas.Admin.Controllers
{
    public class NewsArticlesController : Controller
    {
        private TayanaYachtDBContext db = new TayanaYachtDBContext();

        // GET: Admin/NewsArticles
        public ActionResult Index(int? categoryId, string keyword)
        {
            ViewBag.CategoryId = new SelectList(db.NewsCategories.OrderBy(c => c.SortOrder), "Id", "Name", categoryId);
            ViewBag.Keyword = keyword;

            var newsArticles = db.NewsArticles.Include(n => n.Category);
            if (categoryId.HasValue)
                newsArticles = newsArticles.Where(n => n.CategoryId == categoryId);
            if (!string.IsNullOrWhiteSpace(keyword))
                newsArticles = newsArticles.Where(n => n.Title.Contains(keyword));

            return View(newsArticles.OrderByDescending(n => n.PublishDate).ToList());
        }

        // GET: Admin/NewsArticles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsArticle newsArticle = db.NewsArticles.Find(id);
            if (newsArticle == null)
            {
                return HttpNotFound();
            }
            return View(newsArticle);
        }

        // GET: Admin/NewsArticles/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.NewsCategories, "Id", "Name");
            return View();
        }

        // POST: Admin/NewsArticles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,CoverImageUrl,Content,PublishDate,IsPublished,IsPinned,CategoryId")] NewsArticle newsArticle)
        {
            if (ModelState.IsValid)
            {
                db.NewsArticles.Add(newsArticle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.NewsCategories, "Id", "Name", newsArticle.CategoryId);
            return View(newsArticle);
        }

        // GET: Admin/NewsArticles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsArticle newsArticle = db.NewsArticles.Find(id);
            if (newsArticle == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.NewsCategories, "Id", "Name", newsArticle.CategoryId);
            return View(newsArticle);
        }

        // POST: Admin/NewsArticles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,CoverImageUrl,Content,PublishDate,IsPublished,IsPinned,CategoryId")] NewsArticle newsArticle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newsArticle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.NewsCategories, "Id", "Name", newsArticle.CategoryId);
            return View(newsArticle);
        }

        // GET: Admin/NewsArticles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsArticle newsArticle = db.NewsArticles.Find(id);
            if (newsArticle == null)
            {
                return HttpNotFound();
            }
            return View(newsArticle);
        }

        // POST: Admin/NewsArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NewsArticle newsArticle = db.NewsArticles.Find(id);
            db.NewsArticles.Remove(newsArticle);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
