using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TayanaYachtMVC.Models.Domain
{
    [Table("NewsCategories")]
    public class NewsCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "類別名稱")]
        public string Name { get; set; }

        [Display(Name = "排序")]
        public int SortOrder { get; set; }

        public virtual ICollection<NewsArticle> NewsArticles { get; set; }
    }
}
