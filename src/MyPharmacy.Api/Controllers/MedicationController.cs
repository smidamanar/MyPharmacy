using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPharmacy.Api.DTOs.Medications;
using MyPharmacy.Api.Models;
using MyPharmacy.Api.Services.Interfaces;

namespace MyPharmacy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Pharmacist")]

public class MedicationController : ControllerBase
{
    private readonly IMedicationService _service;

    public MedicationController(IMedicationService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var meds = await _service.GetAllAsync();

        var result = meds.Select(m => new MedicationReadDto
        {
            MedicationID = m.MedicationID,
            Name = m.Name,
            Description = m.Description,
            Price = m.Price,
            StockQuantity = m.StockQuantity
        });

        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var med = await _service.GetByIdAsync(id);
        if (med == null) return NotFound();

        return Ok(new MedicationReadDto
        {
            MedicationID = med.MedicationID,
            Name = med.Name,
            Description = med.Description,
            Price = med.Price,
            StockQuantity = med.StockQuantity
        });
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MedicationCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var medication = new Medication
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity
        };

        await _service.CreateAsync(medication);

        return CreatedAtAction(nameof(GetById),
            new { id = medication.MedicationID },
            new MedicationReadDto
            {
                MedicationID = medication.MedicationID,
                Name = medication.Name,
                Description = medication.Description,
                Price = medication.Price,
                StockQuantity = medication.StockQuantity
            });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MedicationCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Vérifier que le médicament existe
        var existing = await _service.GetByIdAsync(id);
        if (existing == null) return NotFound();

        // Mettre à jour les champs autorisés
        existing.Name = dto.Name;
        existing.Description = dto.Description;
        existing.Price = dto.Price;
        existing.StockQuantity = dto.StockQuantity;

        var updated = await _service.UpdateAsync(existing);
        if (!updated) return StatusCode(500, "Erreur lors de la mise à jour");

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
