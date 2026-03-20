using System.ComponentModel.DataAnnotations;

namespace TayanaYachtMVC.Models.Domain
{
    public class AdminUser
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [MaxLength(100)]
        public string DisplayName { get; set; }

        public bool IsActive { get; set; }
    }
}
