using System.Web.Mvc;

namespace TayanaYachtMVC.Filters
{
    /// <summary>
    /// 套用此 Attribute 的 Controller / Action，
    /// 會在執行前檢查 Session["AdminUserId"] 是否存在。
    /// 未登入者一律導回後台登入頁。
    /// </summary>
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;

            if (session["AdminUserId"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "area",       "Admin" },
                        { "controller", "Login" },
                        { "action",     "Index" }
                    });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
