using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TayanaYachtMVC.Models.Domain
{
    [Table("YachtsLayoutPhotos")]
    public class YachtLayoutPhoto
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Yacht")]
        public int YachtId { get; set; }
        public virtual Yacht Yacht { get; set; }

        [Required]
        [StringLength(500)]
        public string LayoutImgUrl { get; set; }

        [Display(Name = "排序")]
        [Range(0, int.MaxValue, ErrorMessage = "排序必須大於等於 0")]
        public int SortOrder { get; set; }
    }
}
