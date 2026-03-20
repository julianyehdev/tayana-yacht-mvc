using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TayanaYachtMVC.Models.Domain
{
    [Table("YachtDocuments")]
    public class YachtDocument
    {
        public YachtDocument()
        {
            UploadTime = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [ForeignKey("Yacht")]
        public int YachtId { get; set; }
        public virtual Yacht Yacht { get; set; }

        [Required]
        [StringLength(200)]
        public string DocumentName { get; set; }

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; }

        public DateTime UploadTime { get; set; }
    }
}
