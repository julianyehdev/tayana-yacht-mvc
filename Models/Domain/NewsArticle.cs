using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TayanaYachtMVC.Models.Domain
{
    [Table("NewsArticles")]
    public class NewsArticle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "標題")]
        public string Title { get; set; }

        [StringLength(500)]
        [Display(Name = "封面圖片")]
        public string CoverImageUrl { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        [Display(Name = "內容")]
        public string Content { get; set; }

        [Display(Name = "發布日期")]
        public DateTime PublishDate { get; set; }

        [Display(Name = "已發布")]
        public bool IsPublished { get; set; }

        [Display(Name = "置頂")]
        public bool IsPinned { get; set; }

        [Display(Name = "新聞類別")]
        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual NewsCategory Category { get; set; }

        public virtual ICollection<NewsAttachment> Attachments { get; set; }
    }
}
