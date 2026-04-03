using System.ComponentModel.DataAnnotations;

namespace TayanaYachtMVC.Models.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "請輸入姓名")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "請輸入電子郵件")]
        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "請輸入電話")]
        [StringLength(20)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "請選擇國家")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "請選擇遊艇")]
        public int YachtId { get; set; }

        [StringLength(500)]
        public string Comments { get; set; }
    }
}
