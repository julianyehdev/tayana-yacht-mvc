using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace TayanaYachtMVC.Helpers
{
    public static class HtmlHelpers
    {
        /// <summary>
        /// 移除 HTML 標籤，只保留純文字內容
        /// </summary>
        public static string StripHtmlTags(this HtmlHelper html, string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // 移除所有 HTML 標籤
            return Regex.Replace(input, "<[^>]*>", "");
        }
    }
}
