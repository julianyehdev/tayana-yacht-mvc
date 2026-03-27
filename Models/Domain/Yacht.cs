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

        [Display(Name = "Overview (船型簡介)")]
        public string Overview { get; set; }

        [Display(Name = "Dimensions (規格尺寸)")]
        public string Dimensions { get; set; }

        [Display(Name = "詳細規格")]
        public string DetailSpecification { get; set; }

        [Display(Name = "DimensionsPic (規格圖)")]
        public string DimensionsImgUrl { get; set; }

        [Display(Name = "規格說明書")]
        public string SpecSheetUrl { get; set; }

        [Display(Name = "規格說明書名稱")]
        public string SpecSheetFileName { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; }
    }
}
