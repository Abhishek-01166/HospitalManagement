using HospitalManagement.Data;
using HospitalManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Controllers;

[Authorize]
public class DashboardController(ApplicationDbContext dbContext) : Controller
{
    public async Task<IActionResult> Index()
    {
        var today = DateTime.UtcNow.Date;
        var weekStart = today.AddDays(-6);
        var recentStart = today.AddDays(-30);

        var model = new DashboardViewModel
        {
            TotalPatients = await dbContext.Patients.CountAsync(),
            PatientsVisitedToday = await dbContext.Patients.CountAsync(p => p.LastVisitDate.Date == today),
            PatientsVisitedThisWeek = await dbContext.Patients.CountAsync(p => p.LastVisitDate.Date >= weekStart && p.LastVisitDate.Date <= today),
            RecentPatientCount = await dbContext.Patients.CountAsync(p => p.LastVisitDate.Date >= recentStart),
            RecentVisits = await dbContext.Patients
                .OrderByDescending(p => p.LastVisitDate)
                .Take(5)
                .ToListAsync()
        };

        return View(model);
    }
}
