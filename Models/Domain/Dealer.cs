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

        [ForeignKey("Region")]
        public int RegionId { get; set; }
        public virtual Region Region { get; set; }

        [StringLength(260)]
        public string MainImageUrl { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string DescriptionHtml { get; set; }

        public bool IsPublished { get; set; }

        public int SortOrder { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
