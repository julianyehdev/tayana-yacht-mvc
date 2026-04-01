using System.Collections.Generic;

namespace TayanaYachtMVC.Models.ViewModels
{
    public class YachtBannerItem
    {
        public int YachtID { get; set; }
        public string YachtName { get; set; }
        public string ModelNumber { get; set; }
        public bool IsLatest { get; set; }
        public string FirstPhotoUrl { get; set; }
    }

    public class NewsArticleItem
    {
        public string Title { get; set; }
        public string CoverImageUrl { get; set; }
        // 已去除 HTML 標籤的純文字內容
        public string PlainTextContent { get; set; }
    }

    public class HomeIndexViewModel
    {
        public List<YachtBannerItem> Yachts { get; set; }
        public List<NewsArticleItem> News { get; set; }
    }
}
