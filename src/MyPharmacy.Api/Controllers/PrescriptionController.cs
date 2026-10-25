using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPharmacy.Api.DTOs.Prescriptions;
using MyPharmacy.Api.Services;
using MyPharmacy.Api.Services.Interfaces;
using System.Security.Claims;

namespace MyPharmacy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Pharmacist")]
public class PrescriptionController : ControllerBase
{
    private readonly IPrescriptionService _service;

    public PrescriptionController(IPrescriptionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PrescriptionCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!TryGetCurrentPharmacistId(out var pharmacistId))
            return Unauthorized();

        try
        {
            var created = await _service.CreateAsync(dto, pharmacistId);
            return CreatedAtAction(nameof(GetById), new { id = created.PrescriptionID }, null);
        }
        catch (DomainValidationException exception) { return BadRequest(new { error = exception.Message }); }
        catch (InventoryConcurrencyException exception) { return Conflict(new { error = exception.Message }); }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var prescriptions = await _service.GetAllFullAsync(User.IsInRole("Admin") ? null : CurrentPharmacistId());

        var result = prescriptions.Select(p => new PrescriptionReadDto
        {
            PrescriptionID = p.PrescriptionID,
            Date = p.Date,
            Notes = p.Notes,
            PatientID = p.PatientID,
            PatientName = p.Patient?.FullName ?? "",
            PharmacistName = p.Pharmacist?.FullName ?? "",
            Items = p.PrescriptionItems?.Select(i => new PrescriptionItemDetailDto
            {
                MedicationID = i.MedicationID,
                MedicationName = i.Medication?.Name ?? "",
                Quantity = i.Quantity,
                UnitPrice = i.Medication?.Price ?? 0
            }).ToList() ?? new List<PrescriptionItemDetailDto>()
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var prescription = await _service.GetFullPrescriptionAsync(id);
        if (prescription == null) return NotFound();
        if (!User.IsInRole("Admin") && prescription.PharmacistID != CurrentPharmacistId()) return Forbid();

        var result = new PrescriptionReadDto
        {
            PrescriptionID = prescription.PrescriptionID,
            Date = prescription.Date,
            Notes = prescription.Notes,
            PatientID = prescription.PatientID,
            PatientName = prescription.Patient?.FullName ?? "",
            PharmacistName = prescription.Pharmacist?.FullName ?? "",
            Items = prescription.PrescriptionItems?.Select(i => new PrescriptionItemDetailDto
            {
                MedicationID = i.MedicationID,
                MedicationName = i.Medication?.Name ?? "",
                Quantity = i.Quantity,
                UnitPrice = i.Medication?.Price ?? 0
            }).ToList() ?? new List<PrescriptionItemDetailDto>()
        };

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PrescriptionCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!TryGetCurrentPharmacistId(out var pharmacistId)) return Unauthorized();
        try
        {
            await _service.UpdateAsync(id, dto, pharmacistId, User.IsInRole("Admin"));
            return NoContent();
        }
        catch (ResourceNotFoundException) { return NotFound(); }
        catch (DomainValidationException exception) { return BadRequest(new { error = exception.Message }); }
        catch (InventoryConcurrencyException exception) { return Conflict(new { error = exception.Message }); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!TryGetCurrentPharmacistId(out var pharmacistId)) return Unauthorized();
        try
        {
            await _service.DeleteAsync(id, pharmacistId, User.IsInRole("Admin"));
            return NoContent();
        }
        catch (ResourceNotFoundException) { return NotFound(); }
        catch (DomainValidationException exception) { return BadRequest(new { error = exception.Message }); }
        catch (InventoryConcurrencyException exception) { return Conflict(new { error = exception.Message }); }
    }

    private bool TryGetCurrentPharmacistId(out int pharmacistId)
    {
        return int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out pharmacistId);
    }

    private int? CurrentPharmacistId() => TryGetCurrentPharmacistId(out var id) ? id : null;
}
