using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TayanaYachtMVC.Models.Domain
{
    [Table("Yachts")]
    public class Yacht
    {
        public Yacht()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        [Key]
        public int YachtID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "遊艇名稱")]
        public string YachtName { get; set; }

        [Display(Name = "最新款")]
        public bool IsLatest { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; }
    }
}
