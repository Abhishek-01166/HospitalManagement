using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.ViewModels;

public class PatientEditViewModel
{
    public int Id { get; set; }

    [Required, MaxLength(20)]
    public string PatientCode { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required, Range(0, 130)]
    public int Age { get; set; }

    [Required, MaxLength(20)]
    public string Gender { get; set; } = string.Empty;

    [Required, Phone, MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [EmailAddress, MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(200)]
    public string? Address { get; set; }

    [DataType(DataType.Date)]
    public DateTime? LastVisitDate { get; set; }
}
