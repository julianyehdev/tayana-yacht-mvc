using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TayanaYachtMVC.Models.Domain
{
    [Table("YachtPhotos")]
    public class YachtPhoto
    {
        [Key]
        public int PhotoID { get; set; }

        [ForeignKey("Yacht")]
        public int YachtID { get; set; }
        public virtual Yacht Yacht { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "圖片路徑")]
        public string PhotoUrl { get; set; }

        [Display(Name = "排序")]
        public int SortOrder { get; set; }
    }
}
