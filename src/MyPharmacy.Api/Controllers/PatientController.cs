using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPharmacy.Api.DTOs.Patients;
using MyPharmacy.Api.Models;
using MyPharmacy.Api.Services.Interfaces;

namespace MyPharmacy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientController : ControllerBase
{
    private readonly IPatientService _service;

    public PatientController(IPatientService service)
    {
        _service = service;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var patients = await _service.GetAllAsync();

        var result = patients.Select(p => new PatientReadDto
        {
            PatientID = p.PatientID,
            FullName = p.FullName,
            NationalId = p.NationalId,
            BirthDate = p.BirthDate,
            Address = p.Address
        });

        return Ok(result);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var patient = await _service.GetByIdAsync(id);
        if (patient == null)
            return NotFound("Patient not found");

        var result = new PatientReadDto
        {
            PatientID = patient.PatientID,
            FullName = patient.FullName,
            NationalId = patient.NationalId,
            BirthDate = patient.BirthDate,
            Address = patient.Address
        };

        return Ok(result);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PatientCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var patient = new Patient
        {
            FullName = dto.FullName,
            NationalId = dto.NationalId,
            BirthDate = dto.BirthDate,
            Address = dto.Address
        };

        var created = await _service.CreateAsync(patient);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.PatientID },
            null
        );
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PatientCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var patient = await _service.GetByIdAsync(id);
        if (patient == null)
            return NotFound("Patient not found");

        patient.FullName = dto.FullName;
        patient.NationalId = dto.NationalId;
        patient.BirthDate = dto.BirthDate;
        patient.Address = dto.Address;

        await _service.UpdateAsync(patient);

        return NoContent();
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound("Patient not found");

        return NoContent();
    }
}
