using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TayanaYachtMVC.Models.Domain
{
    [Table("NewsAttachments")]
    public class NewsAttachment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("NewsArticle")]
        public int NewsArticleId { get; set; }
        public virtual NewsArticle NewsArticle { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "檔案路徑")]
        public string FileUrl { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "檔案名稱")]
        public string FileName { get; set; }

        [Display(Name = "排序")]
        public int SortOrder { get; set; }
    }
}
