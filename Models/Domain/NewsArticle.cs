using System;
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
        public string Title { get; set; }

        [StringLength(500)]
        public string CoverImageUrl { get; set; }

        [StringLength(500)]
        public string Summary { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Content { get; set; }

        public DateTime PublishDate { get; set; }

        public bool IsPublished { get; set; }

        public bool IsPinned { get; set; }
    }
}
