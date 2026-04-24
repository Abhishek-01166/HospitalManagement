using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models;

public class AppUser
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? FullName { get; set; }

    [EmailAddress, MaxLength(100)]
    public string? Email { get; set; }

    [Phone, MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(20)]
    public string Role { get; set; } = "User";

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
