using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TayanaYachtMVC.Data;
using TayanaYachtMVC.Models.Domain;

namespace TayanaYachtMVC.Areas.Admin.Controllers
{
    public class DealersController : Controller
    {
        private TayanaYachtDBContext db = new TayanaYachtDBContext();

        // ==================== INDEX ====================
        // 列出所有經銷商，支援依國家、地區、關鍵字篩選
        public ActionResult Index(int? countryId, int? regionId, string keyword)
        {
            // 提供國家下拉選單（供篩選用）
            ViewBag.Countries = new SelectList(db.Countries.OrderBy(c => c.SortOrder), "Id", "CountryName", countryId);
            ViewBag.CountryIdValue = countryId;
            ViewBag.RegionIdValue = regionId;
            ViewBag.Keyword = keyword;

            // 讀取時一併 eager load Region 和 Region.Country（顯示國家名稱用）
            var dealers = db.Dealers.Include(d => d.Region).Include(d => d.Region.Country).AsQueryable();

            if (countryId.HasValue)
                dealers = dealers.Where(d => d.Region.CountryId == countryId);
            if (regionId.HasValue)
                dealers = dealers.Where(d => d.RegionId == regionId);
            if (!string.IsNullOrWhiteSpace(keyword))
                dealers = dealers.Where(d => d.Name.Contains(keyword));

            return View(dealers.OrderBy(d => d.SortOrder).ThenByDescending(d => d.CreateDate).ToList());
        }

        // ==================== CREATE GET ====================
        // 顯示新增表單，預載國家下拉（地區由前端 AJAX 動態載入）
        public ActionResult Create()
        {
            ViewBag.Countries = new SelectList(db.Countries.OrderBy(c => c.SortOrder), "Id", "CountryName");
            return View();
        }

        // ==================== CREATE POST ====================
        // 儲存新經銷商：處理圖片上傳後存入 DB
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] // 允許 CKEditor 的 HTML 內容
        public ActionResult Create([Bind(Include = "Id,Name,RegionId,MainImageUrl,DescriptionHtml,IsPublished,SortOrder")] Dealer dealer, HttpPostedFileBase mainImage)
        {
            // 處理圖片上傳
            if (mainImage != null && mainImage.ContentLength > 0)
            {
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp", "image/gif" };
                if (Array.Exists(allowedTypes, t => t == mainImage.ContentType))
                {
                    var ext = System.IO.Path.GetExtension(mainImage.FileName);
                    var fileName = Guid.NewGuid().ToString() + ext;
                    var saveDir = Server.MapPath("~/Content/uploads/dealers/");
                    if (!System.IO.Directory.Exists(saveDir))
                        System.IO.Directory.CreateDirectory(saveDir);
                    mainImage.SaveAs(System.IO.Path.Combine(saveDir, fileName));
                    dealer.MainImageUrl = "/Content/uploads/dealers/" + fileName;
                }
            }

            if (ModelState.IsValid)
            {
                db.Dealers.Add(dealer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // 驗證失敗時重新載入國家 + 地區下拉
            var selectedRegion = db.Regions.Find(dealer.RegionId);
            int selectedCountryId = selectedRegion?.CountryId ?? 0;
            ViewBag.Countries = new SelectList(db.Countries.OrderBy(c => c.SortOrder), "Id", "CountryName", selectedCountryId);
            ViewBag.RegionId = new SelectList(db.Regions.Where(r => r.CountryId == selectedCountryId), "Id", "RegionName", dealer.RegionId);
            return View(dealer);
        }

        // ==================== EDIT GET ====================
        // 顯示編輯表單，讀取現有資料並預載對應的國家 + 地區下拉
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // Eager load Region 才能得知目前選的是哪個國家
            Dealer dealer = db.Dealers.Include(d => d.Region).FirstOrDefault(d => d.Id == id);
            if (dealer == null)
                return HttpNotFound();

            int currentCountryId = dealer.Region?.CountryId ?? 0;
            ViewBag.Countries = new SelectList(db.Countries.OrderBy(c => c.SortOrder), "Id", "CountryName", currentCountryId);
            // 只載入目前國家的地區，讓 <select> 預選正確值
            ViewBag.RegionId = new SelectList(db.Regions.Where(r => r.CountryId == currentCountryId), "Id", "RegionName", dealer.RegionId);
            return View(dealer);
        }

        // ==================== EDIT POST ====================
        // 儲存編輯：若有新圖片則刪除舊檔再換新，否則保留原 URL
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Name,RegionId,MainImageUrl,DescriptionHtml,IsPublished,SortOrder,CreateDate")] Dealer dealer, HttpPostedFileBase mainImage)
        {
            if (mainImage != null && mainImage.ContentLength > 0)
            {
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp", "image/gif" };
                if (Array.Exists(allowedTypes, t => t == mainImage.ContentType))
                {
                    // Delete Before Remove：先刪舊檔
                    if (!string.IsNullOrEmpty(dealer.MainImageUrl))
                    {
                        var oldPath = Server.MapPath("~" + dealer.MainImageUrl);
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    var ext = System.IO.Path.GetExtension(mainImage.FileName);
                    var fileName = Guid.NewGuid().ToString() + ext;
                    var saveDir = Server.MapPath("~/Content/uploads/dealers/");
                    if (!System.IO.Directory.Exists(saveDir))
                        System.IO.Directory.CreateDirectory(saveDir);
                    mainImage.SaveAs(System.IO.Path.Combine(saveDir, fileName));
                    dealer.MainImageUrl = "/Content/uploads/dealers/" + fileName;
                }
            }

            if (ModelState.IsValid)
            {
                db.Entry(dealer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var selectedRegion = db.Regions.Find(dealer.RegionId);
            int selectedCountryId = selectedRegion?.CountryId ?? 0;
            ViewBag.Countries = new SelectList(db.Countries.OrderBy(c => c.SortOrder), "Id", "CountryName", selectedCountryId);
            ViewBag.RegionId = new SelectList(db.Regions.Where(r => r.CountryId == selectedCountryId), "Id", "RegionName", dealer.RegionId);
            return View(dealer);
        }

        // ==================== DELETE GET ====================
        // 顯示刪除確認頁，顯示經銷商資料（含國家、地區）供確認
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Dealer dealer = db.Dealers.Include(d => d.Region).Include(d => d.Region.Country).FirstOrDefault(d => d.Id == id);
            if (dealer == null)
                return HttpNotFound();

            return View(dealer);
        }

        // ==================== DELETE POST ====================
        // 確認刪除：先刪圖片實體檔，再從 DB 移除記錄
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dealer dealer = db.Dealers.Find(id);

            // Delete Before Remove：刪圖片實體檔
            if (!string.IsNullOrEmpty(dealer.MainImageUrl))
            {
                var imagePath = Server.MapPath("~" + dealer.MainImageUrl);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            db.Dealers.Remove(dealer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // ==================== GetRegionsByCountry (JSON API) ====================
        // 供前端 AJAX 呼叫，傳入 countryId 回傳該國家下的所有地區
        // 用於 Create/Edit 表單的國家→地區串聯下拉選單
        public JsonResult GetRegionsByCountry(int countryId)
        {
            var regions = db.Regions
                .Where(r => r.CountryId == countryId)
                .OrderBy(r => r.RegionName)
                .Select(r => new { r.Id, r.RegionName })
                .ToList();
            return Json(regions, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
