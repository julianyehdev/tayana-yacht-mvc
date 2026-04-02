using System.Linq;
using System.Web.Mvc;
using TayanaYachtMVC.Data;
using TayanaYachtMVC.Models.Domain;

namespace TayanaYachtMVC.Areas.Admin.Controllers
{
    [Authorize]
    public class RegionsController : Controller
    {
        private TayanaYachtDBContext db = new TayanaYachtDBContext();

        // ==================== INDEX ====================
        // 國家與地區管理主頁（雙欄 + Modal AJAX 操作）
        public ActionResult Index()
        {
            return View();
        }

        // ==================== COUNTRY API ====================

        // GET: /Admin/Regions/GetCountries
        // 取得所有國家（供左欄列表用）
        public JsonResult GetCountries()
        {
            var countries = db.Countries
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.CountryName)
                .Select(c => new { c.Id, c.CountryName, c.SortOrder })
                .ToList();
            return Json(countries, JsonRequestBehavior.AllowGet);
        }

        // POST: /Admin/Regions/SaveCountry
        // 新增或編輯國家（id=0 代表新增）
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveCountry(int id, string countryName)
        {
            if (string.IsNullOrWhiteSpace(countryName))
                return Json(new { success = false, message = "國家名稱不能為空" });

            if (id == 0)
            {
                var maxSort = db.Countries.Any() ? db.Countries.Max(c => c.SortOrder) : 0;
                var country = new Country { CountryName = countryName, SortOrder = maxSort + 1 };
                db.Countries.Add(country);
            }
            else
            {
                var country = db.Countries.Find(id);
                if (country == null) return Json(new { success = false, message = "找不到此國家" });
                country.CountryName = countryName;
            }

            db.SaveChanges();
            return Json(new { success = true });
        }

        // POST: /Admin/Regions/SaveCountryOrder
        // 拖曳排序後儲存國家順序（ids 為新順序的 id 陣列）
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveCountryOrder(int[] ids)
        {
            if (ids == null) return Json(new { success = false });
            for (int i = 0; i < ids.Length; i++)
            {
                var country = db.Countries.Find(ids[i]);
                if (country != null) country.SortOrder = i + 1;
            }
            db.SaveChanges();
            return Json(new { success = true });
        }

        // POST: /Admin/Regions/DeleteCountry
        // 刪除國家（連動刪除旗下所有地區，但若有經銷商則拒絕）
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteCountry(int id)
        {
            var country = db.Countries.Find(id);
            if (country == null) return Json(new { success = false, message = "找不到此國家" });

            bool hasDealers = db.Dealers.Any(d => d.Region.CountryId == id);
            if (hasDealers)
                return Json(new { success = false, message = "此國家下有經銷商資料，請先移除相關經銷商後再刪除" });

            var regions = db.Regions.Where(r => r.CountryId == id).ToList();
            db.Regions.RemoveRange(regions);
            db.Countries.Remove(country);
            db.SaveChanges();
            return Json(new { success = true });
        }

        // ==================== REGION API ====================

        // GET: /Admin/Regions/GetRegionsByCountry?countryId=1
        // 取得指定國家的地區列表
        public JsonResult GetRegionsByCountry(int countryId)
        {
            var regions = db.Regions
                .Where(r => r.CountryId == countryId)
                .OrderBy(r => r.SortOrder).ThenBy(r => r.RegionName)
                .Select(r => new { r.Id, r.RegionName })
                .ToList();
            return Json(regions, JsonRequestBehavior.AllowGet);
        }

        // POST: /Admin/Regions/SaveRegion
        // 新增或編輯地區（id=0 代表新增）
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveRegion(int id, int countryId, string regionName)
        {
            if (string.IsNullOrWhiteSpace(regionName))
                return Json(new { success = false, message = "地區名稱不能為空" });

            if (id == 0)
            {
                var maxSort = db.Regions.Any(r => r.CountryId == countryId)
                    ? db.Regions.Where(r => r.CountryId == countryId).Max(r => r.SortOrder)
                    : 0;
                var region = new Region { CountryId = countryId, RegionName = regionName, SortOrder = maxSort + 1 };
                db.Regions.Add(region);
            }
            else
            {
                var region = db.Regions.Find(id);
                if (region == null) return Json(new { success = false, message = "找不到此地區" });
                region.RegionName = regionName;
            }

            db.SaveChanges();
            return Json(new { success = true });
        }

        // POST: /Admin/Regions/SaveRegionOrder
        // 拖曳排序後儲存地區順序
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveRegionOrder(int[] ids)
        {
            if (ids == null) return Json(new { success = false });
            for (int i = 0; i < ids.Length; i++)
            {
                var region = db.Regions.Find(ids[i]);
                if (region != null) region.SortOrder = i + 1;
            }
            db.SaveChanges();
            return Json(new { success = true });
        }

        // POST: /Admin/Regions/DeleteRegion
        // 刪除地區（若有經銷商則拒絕）
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteRegion(int id)
        {
            var region = db.Regions.Find(id);
            if (region == null) return Json(new { success = false, message = "找不到此地區" });

            bool hasDealers = db.Dealers.Any(d => d.RegionId == id);
            if (hasDealers)
                return Json(new { success = false, message = "此地區下有經銷商資料，請先移除相關經銷商後再刪除" });

            db.Regions.Remove(region);
            db.SaveChanges();
            return Json(new { success = true });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
