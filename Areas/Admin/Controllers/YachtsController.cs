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
        public ActionResult Create([Bind(Include = "YachtID,YachtName,IsLatest,Overview,Dimensions,DetailSpecification,SpecSheetFileName")] Yacht yacht, HttpPostedFileBase dimensionsImg, HttpPostedFileBase specSheet, IEnumerable<HttpPostedFileBase> photos, int[] photoSortOrders, IEnumerable<HttpPostedFileBase> layoutPhotos, int[] layoutSortOrders)
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
                // 第一段寫入：先存 Yacht，取得新產生的 YachtID
                db.Yachts.Add(yacht);
                db.SaveChanges();

                // 第二段寫入：依排序順序存相簿照片，關聯到剛建立的 YachtID
                if (photos != null)
                {
                    var allowedPhotoTypes = new[] { "image/jpeg", "image/png", "image/webp" };
                    var photoSaveDir = Server.MapPath("~/Content/uploads/yachts/photos/");

                    // 確保目錄存在
                    if (!System.IO.Directory.Exists(photoSaveDir))
                        System.IO.Directory.CreateDirectory(photoSaveDir);

                    var photoList = photos.Where(p => p != null && p.ContentLength > 0).ToList();
                    for (int i = 0; i < photoList.Count; i++)
                    {
                        var photo = photoList[i];

                        // 略過不支援的格式
                        if (!allowedPhotoTypes.Contains(photo.ContentType)) continue;

                        // 略過超過 5MB 的圖片
                        if (photo.ContentLength > 5 * 1024 * 1024) continue;

                        // 用 GUID 產生唯一檔名，避免重複
                        var ext = System.IO.Path.GetExtension(photo.FileName);
                        var fileName = Guid.NewGuid().ToString() + ext;
                        photo.SaveAs(System.IO.Path.Combine(photoSaveDir, fileName));

                        // 取得前端傳來的排序值，若無則以索引代替
                        int sortOrder = (photoSortOrders != null && i < photoSortOrders.Length)
                            ? photoSortOrders[i]
                            : i;

                        db.YachtPhotos.Add(new YachtPhoto
                        {
                            YachtID = yacht.YachtID,
                            PhotoUrl = "/Content/uploads/yachts/photos/" + fileName,
                            SortOrder = sortOrder
                        });
                    }

                    db.SaveChanges();
                }

                // 第三段寫入：Layout & Deck Plan 照片
                if (layoutPhotos != null)
                {
                    var allowedLayoutTypes = new[] { "image/jpeg", "image/png", "image/webp" };
                    var layoutSaveDir = Server.MapPath("~/Content/uploads/yachts/layouts/");

                    if (!System.IO.Directory.Exists(layoutSaveDir))
                        System.IO.Directory.CreateDirectory(layoutSaveDir);

                    var layoutList = layoutPhotos.Where(p => p != null && p.ContentLength > 0).ToList();
                    for (int i = 0; i < layoutList.Count; i++)
                    {
                        var photo = layoutList[i];

                        if (!allowedLayoutTypes.Contains(photo.ContentType)) continue;
                        if (photo.ContentLength > 5 * 1024 * 1024) continue;

                        var ext = System.IO.Path.GetExtension(photo.FileName);
                        var fileName = Guid.NewGuid().ToString() + ext;
                        photo.SaveAs(System.IO.Path.Combine(layoutSaveDir, fileName));

                        int sortOrder = (layoutSortOrders != null && i < layoutSortOrders.Length)
                            ? layoutSortOrders[i]
                            : i;

                        db.YachtLayoutPhotos.Add(new YachtLayoutPhoto
                        {
                            YachtId = yacht.YachtID,
                            LayoutImgUrl = "/Content/uploads/yachts/layouts/" + fileName,
                            SortOrder = sortOrder
                        });
                    }

                    db.SaveChanges();
                }

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

            // 使用 Include 預先載入相簿圖片，避免 View 中 lazy load 失敗
            Yacht yacht = db.Yachts
                .Include(y => y.YachtPhotos)
                .SingleOrDefault(y => y.YachtID == id);

            if (yacht == null)
            {
                return HttpNotFound();
            }
            return View(yacht);
        }

        // POST: Admin/Yachts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "YachtID,YachtName,IsLatest")] Yacht yacht,
            int[] deletePhotoIds,
            IEnumerable<HttpPostedFileBase> photos,
            int[] photoSortOrders)
        {
            if (ModelState.IsValid)
            {
                yacht.UpdatedAt = DateTime.Now;
                db.Entry(yacht).State = EntityState.Modified;

                // ① 刪除標記的舊照片（檔案 + DB 記錄）
                if (deletePhotoIds != null && deletePhotoIds.Length > 0)
                {
                    foreach (var photoId in deletePhotoIds)
                    {
                        var photo = db.YachtPhotos.Find(photoId);
                        if (photo == null || photo.YachtID != yacht.YachtID) continue;

                        // 刪除實體檔案
                        var filePath = Server.MapPath("~" + photo.PhotoUrl);
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);

                        db.YachtPhotos.Remove(photo);
                    }
                }

                // ② 新增新上傳的照片
                if (photos != null)
                {
                    var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
                    var saveDir = Server.MapPath("~/Content/uploads/yachts/photos/");
                    if (!System.IO.Directory.Exists(saveDir))
                        System.IO.Directory.CreateDirectory(saveDir);

                    // 計算現有最大 SortOrder，新圖接在後面
                    int existingMax = db.YachtPhotos
                        .Where(p => p.YachtID == yacht.YachtID)
                        .Select(p => (int?)p.SortOrder)
                        .Max() ?? 0;

                    var photoList = photos.Where(p => p != null && p.ContentLength > 0).ToList();
                    for (int i = 0; i < photoList.Count; i++)
                    {
                        var photo = photoList[i];
                        if (!allowedTypes.Contains(photo.ContentType)) continue;
                        if (photo.ContentLength > 5 * 1024 * 1024) continue;

                        var ext = System.IO.Path.GetExtension(photo.FileName);
                        var fileName = Guid.NewGuid().ToString() + ext;
                        photo.SaveAs(System.IO.Path.Combine(saveDir, fileName));

                        int sortOrder = (photoSortOrders != null && i < photoSortOrders.Length)
                            ? existingMax + photoSortOrders[i]
                            : existingMax + i + 1;

                        db.YachtPhotos.Add(new YachtPhoto
                        {
                            YachtID = yacht.YachtID,
                            PhotoUrl = "/Content/uploads/yachts/photos/" + fileName,
                            SortOrder = sortOrder
                        });
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // 驗證失敗時重新載入相簿圖片
            yacht.YachtPhotos = db.YachtPhotos.Where(p => p.YachtID == yacht.YachtID).ToList();
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
