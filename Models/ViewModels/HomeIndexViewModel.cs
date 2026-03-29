using System.Collections.Generic;

namespace TayanaYachtMVC.Models.ViewModels
{
    public class YachtBannerItem
    {
        public int YachtID { get; set; }
        public string YachtName { get; set; }
        public bool IsLatest { get; set; }
        public string FirstPhotoUrl { get; set; }
    }

    public class HomeIndexViewModel
    {
        public List<YachtBannerItem> Yachts { get; set; }
    }
}
