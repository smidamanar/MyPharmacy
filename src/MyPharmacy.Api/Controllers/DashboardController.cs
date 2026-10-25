using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPharmacy.Api.Data;
using MyPharmacy.Api.DTOs.Dashboard;

namespace MyPharmacy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Pharmacist")]
public class DashboardController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<DashboardStatsDto>> Get()
    {
        var isAdmin = User.IsInRole("Admin");
        var pharmacistId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0;
        var prescriptions = db.Prescriptions.AsNoTracking();
        if (!isAdmin) prescriptions = prescriptions.Where(p => p.PharmacistID == pharmacistId);

        var today = DateTime.UtcNow.Date;
        return Ok(new DashboardStatsDto
        {
            TotalPatients = await db.Patients.CountAsync(),
            TotalMedications = await db.Medications.CountAsync(),
            TotalPrescriptions = await prescriptions.CountAsync(),
            TodayPrescriptions = await prescriptions.CountAsync(p => p.Date >= today && p.Date < today.AddDays(1)),
            LowStockMedications = await db.Medications.CountAsync(m => m.StockQuantity <= 10)
        });
    }
}
