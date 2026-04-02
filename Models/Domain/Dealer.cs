using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TayanaYachtMVC.Models.Domain
{
    [Table("Dealers")]
    public class Dealer
    {
        public Dealer()
        {
            CreateDate = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "經銷商名稱")]
        public string Name { get; set; }

        [ForeignKey("Region")]
        [Display(Name = "地區")]
        public int RegionId { get; set; }
        public virtual Region Region { get; set; }

        [StringLength(260)]
        [Display(Name = "圖片")]
        public string MainImageUrl { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        [Display(Name = "說明")]
        public string DescriptionHtml { get; set; }

        [Display(Name = "已發布")]
        public bool IsPublished { get; set; }

        [Display(Name = "排序")]
        [Range(0, int.MaxValue, ErrorMessage = "排序必須大於等於 0")]
        public int SortOrder { get; set; }

        [Display(Name = "建立日期")]
        public DateTime CreateDate { get; set; }
    }
}
