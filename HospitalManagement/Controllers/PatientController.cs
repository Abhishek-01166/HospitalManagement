using HospitalManagement.Data;
using HospitalManagement.Models;
using HospitalManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Controllers;

[Authorize]
public class PatientController(ApplicationDbContext dbContext) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? searchTerm)
    {
        var query = BuildSearchQuery(searchTerm);
        var patients = await query.OrderByDescending(p => p.LastVisitDate).ToListAsync();
        ViewData["SearchTerm"] = searchTerm;
        return View(patients);
    }

    [HttpGet]
    public async Task<IActionResult> Search(string? searchTerm)
    {
        var query = BuildSearchQuery(searchTerm);

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

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var patient = await dbContext.Patients.FirstOrDefaultAsync(p => p.Id == id);
        return patient is null ? NotFound() : View(patient);
    }

    [HttpGet]
    public IActionResult Create() => View(new PatientCreateViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PatientCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (await dbContext.Patients.AnyAsync(p => p.PatientCode == model.PatientCode.Trim()))
        {
            ModelState.AddModelError(nameof(model.PatientCode), "Patient code already exists.");
            return View(model);
        }

        var now = DateTime.UtcNow;
        var patient = new Patient
        {
            PatientCode = model.PatientCode.Trim(),
            FirstName = model.FirstName.Trim(),
            LastName = model.LastName.Trim(),
            Age = model.Age,
            Gender = model.Gender.Trim(),
            PhoneNumber = model.PhoneNumber.Trim(),
            Email = string.IsNullOrWhiteSpace(model.Email) ? null : model.Email.Trim(),
            Address = string.IsNullOrWhiteSpace(model.Address) ? null : model.Address.Trim(),
            LastVisitDate = model.LastVisitDate ?? now,
            CreatedAt = now,
            UpdatedAt = now
        };

        dbContext.Patients.Add(patient);
        await dbContext.SaveChangesAsync();

        TempData["SuccessMessage"] = "Patient created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var patient = await dbContext.Patients.FindAsync(id);
        if (patient is null)
        {
            return NotFound();
        }

        var model = new PatientEditViewModel
        {
            Id = patient.Id,
            PatientCode = patient.PatientCode,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Age = patient.Age,
            Gender = patient.Gender,
            PhoneNumber = patient.PhoneNumber,
            Email = patient.Email,
            Address = patient.Address,
            LastVisitDate = patient.LastVisitDate
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PatientEditViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var patient = await dbContext.Patients.FindAsync(id);
        if (patient is null)
        {
            return NotFound();
        }

        patient.FirstName = model.FirstName.Trim();
        patient.LastName = model.LastName.Trim();
        patient.Age = model.Age;
        patient.Gender = model.Gender.Trim();
        patient.PhoneNumber = model.PhoneNumber.Trim();
        patient.Email = string.IsNullOrWhiteSpace(model.Email) ? null : model.Email.Trim();
        patient.Address = string.IsNullOrWhiteSpace(model.Address) ? null : model.Address.Trim();
        patient.LastVisitDate = model.LastVisitDate ?? patient.LastVisitDate;
        patient.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Patient updated successfully.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var patient = await dbContext.Patients.FirstOrDefaultAsync(p => p.Id == id);
        return patient is null ? NotFound() : View(patient);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var patient = await dbContext.Patients.FindAsync(id);
        if (patient is null)
        {
            return NotFound();
        }

        dbContext.Patients.Remove(patient);
        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Patient deleted successfully.";

        return RedirectToAction(nameof(Index));
    }

    private IQueryable<Patient> BuildSearchQuery(string? searchTerm)
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
                (p.Email != null && p.Email.Contains(term)) ||
                (p.FirstName + " " + p.LastName).ToLower().Contains(normalized));
        }

        return query;
    }
}
