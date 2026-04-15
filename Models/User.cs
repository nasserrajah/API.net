using System.ComponentModel.DataAnnotations;

namespace core_first.API.Models
{
    public enum UserRole
    {
        Admin,
        Accountant,
        User
    }

    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}