using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using TayanaYachtMVC.Data;
using TayanaYachtMVC.Models.Domain;
using TayanaYachtMVC.Models.ViewModels;

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

        // POST: Contact/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Submit(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // 寄信給管理員
                    SendEmailToAdmin(model);

                    // 自動回信給客戶
                    SendAutoReplyToCustomer(model);

                    TempData["SuccessMessage"] = "感謝您的來信，我們已收到您的訊息，會盡快回覆。";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"寄信失敗：{ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"[Contact Email Error] {ex}");
                }
            }

            // 表單驗證失敗，重新顯示表單
            var countries = _context.Countries.OrderBy(c => c.SortOrder).ToList();
            var yachts = _context.Yachts.OrderBy(y => y.YachtName).ToList();

            ViewBag.Countries = countries;
            ViewBag.Yachts = yachts;

            return View("Index", model);
        }

        /// <summary>
        /// 寄信給管理員
        /// </summary>
        private void SendEmailToAdmin(ContactViewModel model)
        {
            string smtpHost = ConfigurationManager.AppSettings["SmtpHost"];
            int smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            string smtpUser = ConfigurationManager.AppSettings["SmtpUser"];
            string smtpPass = ConfigurationManager.AppSettings["SmtpPass"].Replace(" ", "");
            string adminEmail = ConfigurationManager.AppSettings["ToEmail"];

            using (var smtp = new SmtpClient(smtpHost, smtpPort))
            {
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);

                var mail = new MailMessage(smtpUser, adminEmail)
                {
                    Subject = $"新的客戶查詢 - {model.Name}",
                    Body = BuildAdminEmailBody(model),
                    IsBodyHtml = true
                };

                smtp.Send(mail);
            }
        }

        /// <summary>
        /// 自動回信給客戶
        /// </summary>
        private void SendAutoReplyToCustomer(ContactViewModel model)
        {
            string smtpHost = ConfigurationManager.AppSettings["SmtpHost"];
            int smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            string smtpUser = ConfigurationManager.AppSettings["SmtpUser"];
            string smtpPass = ConfigurationManager.AppSettings["SmtpPass"].Replace(" ", "");

            using (var smtp = new SmtpClient(smtpHost, smtpPort))
            {
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);

                var mail = new MailMessage(smtpUser, model.Email)
                {
                    Subject = "我們已收到您的查詢",
                    Body = BuildCustomerEmailBody(model),
                    IsBodyHtml = true
                };

                smtp.Send(mail);
            }
        }

        /// <summary>
        /// 建立寄給管理員的郵件內容
        /// </summary>
        private string BuildAdminEmailBody(ContactViewModel model)
        {
            var yacht = _context.Yachts.FirstOrDefault(y => y.YachtID == model.YachtId);
            var yachtName = yacht != null ? yacht.YachtName : "未選擇";

            return $@"
<html>
<body style='font-family: Arial, sans-serif;'>
    <h2>新的客戶查詢</h2>
    <table border='1' cellpadding='10' cellspacing='0' style='width: 100%; border-collapse: collapse;'>
        <tr>
            <td style='font-weight: bold; width: 20%;'>姓名</td>
            <td>{model.Name}</td>
        </tr>
        <tr>
            <td style='font-weight: bold;'>電子郵件</td>
            <td>{model.Email}</td>
        </tr>
        <tr>
            <td style='font-weight: bold;'>電話</td>
            <td>{model.Phone}</td>
        </tr>
        <tr>
            <td style='font-weight: bold;'>遊艇</td>
            <td>{yachtName}</td>
        </tr>
        <tr>
            <td style='font-weight: bold;'>備註</td>
            <td>{model.Comments}</td>
        </tr>
    </table>
</body>
</html>";
        }

        /// <summary>
        /// 建立寄給客戶的自動回信內容
        /// </summary>
        private string BuildCustomerEmailBody(ContactViewModel model)
        {
            return $@"
<html>
<body style='font-family: Arial, sans-serif;'>
    <p>尊敬的 {model.Name}，</p>
    <p>感謝您對大洋遊艇的關注，我們已收到您的查詢。</p>
    <p>我們會儘快審視您的查詢，並在 2-3 個工作天內與您聯繫。</p>
    <p>如有任何疑問，歡迎致電我們的銷售部：+886(7)641 2422</p>
    <p>祝安好，</p>
    <p>
        <strong>大洋遊艇 Tayana Yachts</strong><br/>
        地址：NO.60 Haichien Rd. Chungmen Village Linyuan Kaohsiung Hsien 832 Taiwan R.O.C<br/>
        電話：+886(7)641 2422<br/>
        傳真：+886(7)642 3193<br/>
        郵箱：info@tayanaworld.com
    </p>
</body>
</html>";
        }
    }
}