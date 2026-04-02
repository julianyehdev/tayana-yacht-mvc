using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using TayanaYachtMVC.Data;
using TayanaYachtMVC.Models.Domain;

namespace TayanaYachtMVC.Areas.Admin.Controllers
{
    [Authorize]
    public class NewsArticlesController : Controller
    {
        private TayanaYachtDBContext db = new TayanaYachtDBContext();

        // GET: Admin/NewsArticles
        public ActionResult Index(int? categoryId, string keyword, int? page, int? pageSize)
        {
            ViewBag.CategoryId = new SelectList(db.NewsCategories.OrderBy(c => c.SortOrder), "Id", "Name", categoryId);
            ViewBag.Keyword = keyword;
            ViewBag.CategoryIdValue = categoryId;

            int size = pageSize.GetValueOrDefault(10);
            if (size < 1) size = 1;
            if (size > 100) size = 100;
            int pageNumber = page.GetValueOrDefault(1);
            if (pageNumber < 1) pageNumber = 1;

            var newsArticles = db.NewsArticles.Include(n => n.Category);
            if (categoryId.HasValue)
                newsArticles = newsArticles.Where(n => n.CategoryId == categoryId);
            if (!string.IsNullOrWhiteSpace(keyword))
                newsArticles = newsArticles.Where(n => n.Title.Contains(keyword));

            var sorted = newsArticles.OrderByDescending(n => n.IsPinned).ThenByDescending(n => n.PublishDate);
            return View(sorted.ToPagedList(pageNumber, size));
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
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Title,CoverImageUrl,Content,PublishDate,IsPublished,IsPinned,CategoryId")] NewsArticle newsArticle, HttpPostedFileBase coverImage, IEnumerable<HttpPostedFileBase> attachments)
        {
            if (coverImage != null && coverImage.ContentLength > 0)
            {
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp", "image/gif" };
                if (Array.Exists(allowedTypes, t => t == coverImage.ContentType))
                {
                    var ext = System.IO.Path.GetExtension(coverImage.FileName);
                    var fileName = Guid.NewGuid().ToString() + ext;
                    var saveDir = Server.MapPath("~/Content/uploads/news/");
                    if (!System.IO.Directory.Exists(saveDir))
                        System.IO.Directory.CreateDirectory(saveDir);
                    coverImage.SaveAs(System.IO.Path.Combine(saveDir, fileName));
                    newsArticle.CoverImageUrl = "/Content/uploads/news/" + fileName;
                }
            }

            if (ModelState.IsValid)
            {
                db.NewsArticles.Add(newsArticle);
                db.SaveChanges();

                // 處理附件上傳
                if (attachments != null)
                {
                    var attachSaveDir = Server.MapPath("~/Content/uploads/news/attachments/");
                    if (!System.IO.Directory.Exists(attachSaveDir))
                        System.IO.Directory.CreateDirectory(attachSaveDir);

                    int sortOrder = 0;
                    foreach (var file in attachments)
                    {
                        if (file == null || file.ContentLength == 0) continue;
                        var ext = System.IO.Path.GetExtension(file.FileName);
                        var savedName = Guid.NewGuid().ToString() + ext;
                        file.SaveAs(System.IO.Path.Combine(attachSaveDir, savedName));
                        db.NewsAttachments.Add(new NewsAttachment
                        {
                            NewsArticleId = newsArticle.Id,
                            FileUrl = "/Content/uploads/news/attachments/" + savedName,
                            FileName = file.FileName,
                            SortOrder = sortOrder++
                        });
                    }
                    db.SaveChanges();
                }

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
            NewsArticle newsArticle = db.NewsArticles.Include(n => n.Attachments).FirstOrDefault(n => n.Id == id);
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
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Title,CoverImageUrl,Content,PublishDate,IsPublished,IsPinned,CategoryId")] NewsArticle newsArticle, HttpPostedFileBase coverImage, int[] deleteAttachmentIds, IEnumerable<HttpPostedFileBase> attachments)
        {
            if (coverImage != null && coverImage.ContentLength > 0)
            {
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp", "image/gif" };
                if (Array.Exists(allowedTypes, t => t == coverImage.ContentType))
                {
                    // 刪除舊圖
                    if (!string.IsNullOrEmpty(newsArticle.CoverImageUrl))
                    {
                        var oldPath = Server.MapPath("~" + newsArticle.CoverImageUrl);
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    var ext = System.IO.Path.GetExtension(coverImage.FileName);
                    var fileName = Guid.NewGuid().ToString() + ext;
                    var saveDir = Server.MapPath("~/Content/uploads/news/");
                    if (!System.IO.Directory.Exists(saveDir))
                        System.IO.Directory.CreateDirectory(saveDir);
                    coverImage.SaveAs(System.IO.Path.Combine(saveDir, fileName));
                    newsArticle.CoverImageUrl = "/Content/uploads/news/" + fileName;
                }
            }

            if (ModelState.IsValid)
            {
                db.Entry(newsArticle).State = EntityState.Modified;
                db.SaveChanges();

                // 刪除勾選的附件
                if (deleteAttachmentIds != null)
                {
                    foreach (var attachId in deleteAttachmentIds)
                    {
                        var attach = db.NewsAttachments.Find(attachId);
                        if (attach == null || attach.NewsArticleId != newsArticle.Id) continue;
                        var filePath = Server.MapPath("~" + attach.FileUrl);
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                        db.NewsAttachments.Remove(attach);
                    }
                    db.SaveChanges();
                }

                // 新增附件
                if (attachments != null)
                {
                    var attachSaveDir = Server.MapPath("~/Content/uploads/news/attachments/");
                    if (!System.IO.Directory.Exists(attachSaveDir))
                        System.IO.Directory.CreateDirectory(attachSaveDir);

                    int sortOrder = db.NewsAttachments.Where(a => a.NewsArticleId == newsArticle.Id).Count();
                    foreach (var file in attachments)
                    {
                        if (file == null || file.ContentLength == 0) continue;
                        var ext = System.IO.Path.GetExtension(file.FileName);
                        var savedName = Guid.NewGuid().ToString() + ext;
                        file.SaveAs(System.IO.Path.Combine(attachSaveDir, savedName));
                        db.NewsAttachments.Add(new NewsAttachment
                        {
                            NewsArticleId = newsArticle.Id,
                            FileUrl = "/Content/uploads/news/attachments/" + savedName,
                            FileName = file.FileName,
                            SortOrder = sortOrder++
                        });
                    }
                    db.SaveChanges();
                }

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
            NewsArticle newsArticle = db.NewsArticles.Include(n => n.Attachments).FirstOrDefault(n => n.Id == id);

            // 刪除封面圖片
            if (!string.IsNullOrEmpty(newsArticle.CoverImageUrl))
            {
                var coverPath = Server.MapPath("~" + newsArticle.CoverImageUrl);
                if (System.IO.File.Exists(coverPath))
                    System.IO.File.Delete(coverPath);
            }

            // 刪除所有附件實體檔案
            foreach (var attach in newsArticle.Attachments.ToList())
            {
                var filePath = Server.MapPath("~" + attach.FileUrl);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

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
