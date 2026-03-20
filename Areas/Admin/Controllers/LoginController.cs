using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using TayanaYachtMVC.Areas.Admin.Models;
using TayanaYachtMVC.Data;

namespace TayanaYachtMVC.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private readonly TayanaYachtDBContext _db = new TayanaYachtDBContext();

        // GET: /Admin/Login
        [HttpGet]
        public ActionResult Index()
        {
            // 若已登入，直接跳到後台首頁
            if (Session["AdminUserId"] != null)
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

            return View();
        }

        // POST: /Admin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // 將輸入密碼做 SHA256 Hash，再和資料庫比對
            var hashedInput = HashPassword(model.Password);

            var user = _db.AdminUsers.FirstOrDefault(u =>
                u.Username == model.Username &&
                u.PasswordHash == hashedInput &&
                u.IsActive);

            if (user == null)
            {
                ModelState.AddModelError("", "帳號或密碼錯誤");
                return View(model);
            }

            // 登入成功：寫入 Session
            Session["AdminUserId"] = user.Id;
            Session["AdminUsername"] = user.Username;
            Session["AdminDisplayName"] = user.DisplayName;

            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }

        // GET: /Admin/Login/Logout
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login", new { area = "Admin" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }

        // ── 私有輔助方法 ───────────────────────────
        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant(); // e.g. "5e884898..."
            }
        }
    }
}
