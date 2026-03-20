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
        public string CountryName { get; set; }

        public int SortOrder { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
