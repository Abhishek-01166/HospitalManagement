using HospitalManagement.Models;

namespace HospitalManagement.ViewModels;

public class PatientSearchViewModel
{
    public string? SearchTerm { get; set; }

    public IReadOnlyList<Patient> Results { get; set; } = [];
}
