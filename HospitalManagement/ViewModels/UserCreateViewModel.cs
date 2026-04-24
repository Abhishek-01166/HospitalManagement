using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.ViewModels;

public class UserCreateViewModel
{
    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? FullName { get; set; }

    [EmailAddress, MaxLength(100)]
    public string? Email { get; set; }

    [Phone, MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [Required, MaxLength(20)]
    public string Role { get; set; } = "User";

    [Required, DataType(DataType.Password), MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;
}
