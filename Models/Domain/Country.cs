using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TayanaYachtMVC.Models.Domain
{
    [Table("Countries")]
    public class Country
    {
        public Country()
        {
            CreateDate = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "國家名稱")]
        public string CountryName { get; set; }

        [Display(Name = "排序")]
        [Range(0, int.MaxValue, ErrorMessage = "排序必須大於等於 0")]
        public int SortOrder { get; set; }

        [Display(Name = "建立日期")]
        public DateTime CreateDate { get; set; }
    }
}
