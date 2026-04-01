using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TayanaYachtMVC.Models.Domain
{
    [Table("Regions")]
    public class Region
    {
        public Region()
        {
            CreateDate = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [ForeignKey("Country")]
        [Display(Name = "國家")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "地區名稱")]
        public string RegionName { get; set; }

        [Display(Name = "排序")]
        [Range(0, int.MaxValue, ErrorMessage = "排序必須大於等於 0")]
        public int SortOrder { get; set; }

        [Display(Name = "建立日期")]
        public DateTime CreateDate { get; set; }
    }
}
