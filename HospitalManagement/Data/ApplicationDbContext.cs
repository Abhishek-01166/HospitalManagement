using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<AppUser> AppUsers => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Patient>()
            .HasIndex(p => p.PatientCode)
            .IsUnique();

        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.Email)
            .IsUnique()
            .HasFilter("[Email] IS NOT NULL");

        modelBuilder.Entity<Patient>().HasData(GetSeedPatients());
    }

    private static List<Patient> GetSeedPatients()
    {
        var now = new DateTime(2026, 4, 24, 9, 0, 0, DateTimeKind.Utc);

        return
        [
            new() { Id = 1, PatientCode = "P1001", FirstName = "Olivia", LastName = "Johnson", Age = 34, Gender = "Female", PhoneNumber = "555-201-1001", Email = "olivia.johnson@example.com", Address = "101 Oak St", LastVisitDate = now.AddDays(-1), CreatedAt = now.AddMonths(-6), UpdatedAt = now.AddDays(-1) },
            new() { Id = 2, PatientCode = "P1002", FirstName = "Liam", LastName = "Smith", Age = 41, Gender = "Male", PhoneNumber = "555-201-1002", Email = "liam.smith@example.com", Address = "42 Pine Ave", LastVisitDate = now.AddDays(-3), CreatedAt = now.AddMonths(-8), UpdatedAt = now.AddDays(-3) },
            new() { Id = 3, PatientCode = "P1003", FirstName = "Ava", LastName = "Williams", Age = 27, Gender = "Female", PhoneNumber = "555-201-1003", Email = "ava.williams@example.com", Address = "9 River Rd", LastVisitDate = now.AddDays(-7), CreatedAt = now.AddMonths(-5), UpdatedAt = now.AddDays(-7) },
            new() { Id = 4, PatientCode = "P1004", FirstName = "Noah", LastName = "Brown", Age = 52, Gender = "Male", PhoneNumber = "555-201-1004", Email = "noah.brown@example.com", Address = "77 Cedar Dr", LastVisitDate = now.AddDays(-14), CreatedAt = now.AddMonths(-10), UpdatedAt = now.AddDays(-14) },
            new() { Id = 5, PatientCode = "P1005", FirstName = "Emma", LastName = "Davis", Age = 30, Gender = "Female", PhoneNumber = "555-201-1005", Email = "emma.davis@example.com", Address = "35 Maple Ln", LastVisitDate = now, CreatedAt = now.AddMonths(-4), UpdatedAt = now },
            new() { Id = 6, PatientCode = "P1006", FirstName = "James", LastName = "Miller", Age = 65, Gender = "Male", PhoneNumber = "555-201-1006", Email = "james.miller@example.com", Address = "82 Hill St", LastVisitDate = now.AddDays(-2), CreatedAt = now.AddMonths(-12), UpdatedAt = now.AddDays(-2) },
            new() { Id = 7, PatientCode = "P1007", FirstName = "Sophia", LastName = "Wilson", Age = 23, Gender = "Female", PhoneNumber = "555-201-1007", Email = "sophia.wilson@example.com", Address = "14 Birch Way", LastVisitDate = now.AddDays(-9), CreatedAt = now.AddMonths(-3), UpdatedAt = now.AddDays(-9) },
            new() { Id = 8, PatientCode = "P1008", FirstName = "Benjamin", LastName = "Moore", Age = 46, Gender = "Male", PhoneNumber = "555-201-1008", Email = "benjamin.moore@example.com", Address = "201 Lake View", LastVisitDate = now.AddDays(-5), CreatedAt = now.AddMonths(-9), UpdatedAt = now.AddDays(-5) },
            new() { Id = 9, PatientCode = "P1009", FirstName = "Mia", LastName = "Taylor", Age = 38, Gender = "Female", PhoneNumber = "555-201-1009", Email = "mia.taylor@example.com", Address = "91 Valley Blvd", LastVisitDate = now.AddDays(-20), CreatedAt = now.AddMonths(-11), UpdatedAt = now.AddDays(-20) },
            new() { Id = 10, PatientCode = "P1010", FirstName = "Lucas", LastName = "Anderson", Age = 55, Gender = "Male", PhoneNumber = "555-201-1010", Email = "lucas.anderson@example.com", Address = "65 Park St", LastVisitDate = now.AddDays(-6), CreatedAt = now.AddMonths(-7), UpdatedAt = now.AddDays(-6) },
            new() { Id = 11, PatientCode = "P1011", FirstName = "Charlotte", LastName = "Thomas", Age = 29, Gender = "Female", PhoneNumber = "555-201-1011", Email = "charlotte.thomas@example.com", Address = "11 Garden Ct", LastVisitDate = now.AddDays(-4), CreatedAt = now.AddMonths(-2), UpdatedAt = now.AddDays(-4) },
            new() { Id = 12, PatientCode = "P1012", FirstName = "Henry", LastName = "Jackson", Age = 60, Gender = "Male", PhoneNumber = "555-201-1012", Email = "henry.jackson@example.com", Address = "404 Sunset Dr", LastVisitDate = now.AddDays(-11), CreatedAt = now.AddMonths(-13), UpdatedAt = now.AddDays(-11) },
            new() { Id = 13, PatientCode = "P1013", FirstName = "Amelia", LastName = "White", Age = 33, Gender = "Female", PhoneNumber = "555-201-1013", Email = "amelia.white@example.com", Address = "500 Aspen Rd", LastVisitDate = now.AddDays(-8), CreatedAt = now.AddMonths(-4), UpdatedAt = now.AddDays(-8) },
            new() { Id = 14, PatientCode = "P1014", FirstName = "Ethan", LastName = "Harris", Age = 44, Gender = "Male", PhoneNumber = "555-201-1014", Email = "ethan.harris@example.com", Address = "29 Ridge St", LastVisitDate = now.AddDays(-1), CreatedAt = now.AddMonths(-6), UpdatedAt = now.AddDays(-1) },
            new() { Id = 15, PatientCode = "P1015", FirstName = "Isabella", LastName = "Martin", Age = 36, Gender = "Female", PhoneNumber = "555-201-1015", Email = "isabella.martin@example.com", Address = "730 Brook Ave", LastVisitDate = now.AddDays(-15), CreatedAt = now.AddMonths(-8), UpdatedAt = now.AddDays(-15) }
        ];
    }
}
