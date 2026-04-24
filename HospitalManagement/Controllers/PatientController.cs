using HospitalManagement.Data;
using HospitalManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Controllers;

[Authorize]
public class PatientController(ApplicationDbContext dbContext) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Search(string? searchTerm)
    {
        var query = dbContext.Patients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim();
            var normalized = term.ToLower();

            query = query.Where(p =>
                p.PatientCode.Contains(term) ||
                p.FirstName.Contains(term) ||
                p.LastName.Contains(term) ||
                p.PhoneNumber.Contains(term) ||
                p.Email.Contains(term) ||
                (p.FirstName + " " + p.LastName).ToLower().Contains(normalized));
        }

        var results = await query
            .OrderByDescending(p => p.LastVisitDate)
            .Take(string.IsNullOrWhiteSpace(searchTerm) ? 10 : 100)
            .ToListAsync();

        var model = new PatientSearchViewModel
        {
            SearchTerm = searchTerm,
            Results = results
        };

        return View(model);
    }
}
