using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TayanaYachtMVC.Models.Domain
{
    [Table("YachtAlbums")]
    public class YachtAlbum
    {
        public YachtAlbum()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        [Key]
        public int AlbumID { get; set; }

        [Index(IsUnique = true)]
        [ForeignKey("Yacht")]
        public int YachtID { get; set; }
        public virtual Yacht Yacht { get; set; }

        [Required]
        [StringLength(100)]
        public string AlbumName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
