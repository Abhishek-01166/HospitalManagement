using HospitalManagement.Models;

namespace HospitalManagement.ViewModels;

public class DashboardViewModel
{
    public int TotalPatients { get; set; }
    public int PatientsVisitedToday { get; set; }
    public int PatientsVisitedThisWeek { get; set; }
    public int RecentPatientCount { get; set; }
    public int TotalUsers { get; set; }
    public IReadOnlyList<Patient> RecentVisits { get; set; } = [];
}
