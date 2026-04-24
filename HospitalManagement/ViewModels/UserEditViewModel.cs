using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.ViewModels;

public class UserEditViewModel
{
    public int Id { get; set; }

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

    public bool IsActive { get; set; } = true;

    [DataType(DataType.Password), MinLength(6)]
    public string? NewPassword { get; set; }
}
