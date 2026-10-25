using Microsoft.EntityFrameworkCore;
using MyPharmacy.Api.Models;

namespace MyPharmacy.Api.Data;

/// <summary>
/// Creates the initial administrator only when explicitly configured through secrets.
/// It never overwrites an existing account.
/// </summary>
public static class DatabaseBootstrapper
{
    public static async Task SeedAdminAsync(IServiceProvider services, IConfiguration configuration)
    {
        var email = configuration["BootstrapAdmin:Email"]?.Trim();
        var password = configuration["BootstrapAdmin:Password"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return;

        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (await db.Pharmacists.AnyAsync(p => p.Email == email))
            return;

        db.Pharmacists.Add(new Pharmacist
        {
            FullName = "System Administrator",
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = "Admin"
        });

        await db.SaveChangesAsync();
    }
}
