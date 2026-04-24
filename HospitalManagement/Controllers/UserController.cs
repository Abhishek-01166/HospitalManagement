using HospitalManagement.Data;
using HospitalManagement.Models;
using HospitalManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Controllers;

[Authorize(Roles = "Admin")]
public class UserController(ApplicationDbContext dbContext, IPasswordHasher<AppUser> passwordHasher) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = await dbContext.AppUsers.OrderBy(u => u.Username).ToListAsync();
        return View(users);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var user = await dbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == id);
        return user is null ? NotFound() : View(user);
    }

    [HttpGet]
    public IActionResult Create() => View(new UserCreateViewModel { Role = "User" });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (await dbContext.AppUsers.AnyAsync(u => u.Username == model.Username.Trim()))
        {
            ModelState.AddModelError(nameof(model.Username), "Username already exists.");
            return View(model);
        }

        if (!string.IsNullOrWhiteSpace(model.Email) && await dbContext.AppUsers.AnyAsync(u => u.Email == model.Email.Trim()))
        {
            ModelState.AddModelError(nameof(model.Email), "Email already exists.");
            return View(model);
        }

        var now = DateTime.UtcNow;
        var user = new AppUser
        {
            Username = model.Username.Trim(),
            FullName = string.IsNullOrWhiteSpace(model.FullName) ? null : model.FullName.Trim(),
            Email = string.IsNullOrWhiteSpace(model.Email) ? null : model.Email.Trim(),
            PhoneNumber = string.IsNullOrWhiteSpace(model.PhoneNumber) ? null : model.PhoneNumber.Trim(),
            Role = model.Role.Trim(),
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };

        user.PasswordHash = passwordHasher.HashPassword(user, model.Password);

        dbContext.AppUsers.Add(user);
        await dbContext.SaveChangesAsync();

        TempData["SuccessMessage"] = "User created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await dbContext.AppUsers.FindAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        var model = new UserEditViewModel
        {
            Id = user.Id,
            Username = user.Username,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role,
            IsActive = user.IsActive
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserEditViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await dbContext.AppUsers.FindAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        var username = model.Username.Trim();
        if (await dbContext.AppUsers.AnyAsync(u => u.Id != id && u.Username == username))
        {
            ModelState.AddModelError(nameof(model.Username), "Username already exists.");
            return View(model);
        }

        var email = string.IsNullOrWhiteSpace(model.Email) ? null : model.Email.Trim();
        if (email != null && await dbContext.AppUsers.AnyAsync(u => u.Id != id && u.Email == email))
        {
            ModelState.AddModelError(nameof(model.Email), "Email already exists.");
            return View(model);
        }

        user.Username = username;
        user.FullName = string.IsNullOrWhiteSpace(model.FullName) ? null : model.FullName.Trim();
        user.Email = email;
        user.PhoneNumber = string.IsNullOrWhiteSpace(model.PhoneNumber) ? null : model.PhoneNumber.Trim();
        user.Role = model.Role.Trim();
        user.IsActive = model.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(model.NewPassword))
        {
            user.PasswordHash = passwordHasher.HashPassword(user, model.NewPassword);
        }

        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "User updated successfully.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await dbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == id);
        return user is null ? NotFound() : View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await dbContext.AppUsers.FindAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        if (string.Equals(user.Username, "admin", StringComparison.OrdinalIgnoreCase))
        {
            TempData["ErrorMessage"] = "Default admin user cannot be deleted.";
            return RedirectToAction(nameof(Index));
        }

        dbContext.AppUsers.Remove(user);
        await dbContext.SaveChangesAsync();

        TempData["SuccessMessage"] = "User deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
