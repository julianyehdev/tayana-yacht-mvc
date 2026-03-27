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
    public class YachtsController : Controller
    {
        private TayanaYachtDBContext db = new TayanaYachtDBContext();

        // GET: Admin/Yachts
        public ActionResult Index()
        {
            return View(db.Yachts.ToList());
        }

        // GET: Admin/Yachts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yacht yacht = db.Yachts.Find(id);
            if (yacht == null)
            {
                return HttpNotFound();
            }
            return View(yacht);
        }

        // GET: Admin/Yachts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Yachts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "YachtID,YachtName,IsLatest,Overview,Dimensions,SpecSheetFileName")] Yacht yacht, HttpPostedFileBase dimensionsImg, HttpPostedFileBase specSheet)
        {
            if (dimensionsImg != null && dimensionsImg.ContentLength > 0)
            {
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
                if (!allowedTypes.Contains(dimensionsImg.ContentType))
                {
                    ModelState.AddModelError("DimensionsImgUrl", "不支援的檔案格式。");
                }
                else if (dimensionsImg.ContentLength > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("DimensionsImgUrl", "檔案大小不能超過 5MB。");
                }
                else
                {
                    var ext = System.IO.Path.GetExtension(dimensionsImg.FileName);
                    var fileName = Guid.NewGuid().ToString() + ext;
                    var savePath = Server.MapPath("~/Content/uploads/yachts/dimensions/");
                    if (!System.IO.Directory.Exists(savePath))
                        System.IO.Directory.CreateDirectory(savePath);
                    dimensionsImg.SaveAs(System.IO.Path.Combine(savePath, fileName));
                    yacht.DimensionsImgUrl = "/Content/uploads/yachts/dimensions/" + fileName;
                }
            }

            if (specSheet != null && specSheet.ContentLength > 0)
            {
                var allowedTypes = new[] { "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" };
                if (!allowedTypes.Contains(specSheet.ContentType))
                {
                    ModelState.AddModelError("SpecSheetUrl", "不支援的檔案格式。");
                }
                else if (specSheet.ContentLength > 10 * 1024 * 1024)
                {
                    ModelState.AddModelError("SpecSheetUrl", "檔案大小不能超過 10MB。");
                }
                else
                {
                    var ext = System.IO.Path.GetExtension(specSheet.FileName);
                    var fileName = Guid.NewGuid().ToString() + ext;
                    var savePath = Server.MapPath("~/Content/uploads/yachts/specs/");
                    if (!System.IO.Directory.Exists(savePath))
                        System.IO.Directory.CreateDirectory(savePath);
                    specSheet.SaveAs(System.IO.Path.Combine(savePath, fileName));
                    yacht.SpecSheetUrl = "/Content/uploads/yachts/specs/" + fileName;
                }
            }

            if (ModelState.IsValid)
            {
                db.Yachts.Add(yacht);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(yacht);
        }

        // GET: Admin/Yachts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yacht yacht = db.Yachts.Find(id);
            if (yacht == null)
            {
                return HttpNotFound();
            }
            return View(yacht);
        }

        // POST: Admin/Yachts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "YachtID,YachtName,IsLatest")] Yacht yacht)
        {
            if (ModelState.IsValid)
            {
                yacht.UpdatedAt = DateTime.Now;
                db.Entry(yacht).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(yacht);
        }

        // GET: Admin/Yachts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yacht yacht = db.Yachts.Find(id);
            if (yacht == null)
            {
                return HttpNotFound();
            }
            return View(yacht);
        }

        // POST: Admin/Yachts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Yacht yacht = db.Yachts.Find(id);
            db.Yachts.Remove(yacht);
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
