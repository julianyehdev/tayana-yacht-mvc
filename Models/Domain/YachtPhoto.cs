using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TayanaYachtMVC.Models.Domain
{
    [Table("YachtPhotos")]
    public class YachtPhoto
    {
        public YachtPhoto()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        [Key]
        public int PhotoID { get; set; }

        [ForeignKey("YachtAlbum")]
        public int AlbumID { get; set; }
        public virtual YachtAlbum YachtAlbum { get; set; }

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
