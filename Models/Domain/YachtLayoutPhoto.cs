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

        public int SortOrder { get; set; }
    }
}
