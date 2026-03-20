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
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        [Required]
        [StringLength(100)]
        public string RegionName { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
