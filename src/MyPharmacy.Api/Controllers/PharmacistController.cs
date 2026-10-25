using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPharmacy.Api.DTOs.Pharmacists;
using MyPharmacy.Api.Models;
using MyPharmacy.Api.Services.Interfaces;
using System.Security.Claims;

namespace MyPharmacy.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Pharmacist")]
    public class PharmacistController : ControllerBase
    {
        private readonly IPharmacistService _service;

        public PharmacistController(IPharmacistService service)
        {
            _service = service;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pharmacists = await _service.GetAllAsync();
            var result = pharmacists.Select(p => new PharmacistReadDto
            {
                PharmacistID = p.PharmacistID,
                FullName = p.FullName,
                Email = p.Email,
                Address = p.Address,
                Phone = p.Phone,
                Role = p.Role
            });

            return Ok(result);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!User.IsInRole("Admin") && (!TryGetCurrentPharmacistId(out var currentId) || currentId != id))
                return Forbid();
            var p = await _service.GetByIdAsync(id);
            if (p == null)
                return NotFound("Pharmacist not found");

            return Ok(new PharmacistReadDto
            {
                PharmacistID = p.PharmacistID,
                FullName = p.FullName,
                Email = p.Email,
                Address = p.Address,
                Phone = p.Phone,
                Role = p.Role
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PharmacistCreateDto dto)
        {
            var pharmacist = new Pharmacist
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Address = dto.Address,
                Phone = dto.Phone,
                Role = dto.Role ?? "Pharmacist"
            };

            await _service.CreateAsync(pharmacist);

            return CreatedAtAction(nameof(GetById), new { id = pharmacist.PharmacistID }, null);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, [FromBody] PharmacistReadDto dto)
        {
            // Vérifier que le pharmacien existe
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            // Mettre à jour uniquement les champs autorisés
            existing.FullName = dto.FullName;
            existing.Email = dto.Email;
            existing.Address = dto.Address;
            existing.Phone = dto.Phone;
            existing.Role = dto.Role ?? existing.Role;


            var updated = await _service.UpdateAsync(existing);
            if (!updated) return StatusCode(500, "Erreur lors de la mise à jour");

            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound("Pharmacist not found");

            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return StatusCode(500, "Error deleting pharmacist");

            return NoContent(); // 204 OK
        }
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            if (!TryGetCurrentPharmacistId(out var pharmacistId))
                return Unauthorized();

            var p = await _service.GetByIdAsync(pharmacistId);
            if (p == null) return NotFound();

            return Ok(new PharmacistReadDto
            {
                FullName = p.FullName,
                Email = p.Email,
                Address = p.Address ?? "",
                Phone = p.Phone ?? "",
                Role = p.Role
            });
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] PharmacistReadDto dto)
        {
            if (!TryGetCurrentPharmacistId(out var pharmacistId))
                return Unauthorized();

            var p = await _service.GetByIdAsync(pharmacistId);
            if (p == null) return NotFound();

            p.FullName = dto.FullName;
            p.Address = dto.Address;
            p.Phone = dto.Phone;
            // Role et Email ne changent pas ici

            var updated = await _service.UpdateAsync(p);
            if (!updated) return StatusCode(500, "Error updating profile");

            return NoContent();
        }

        private bool TryGetCurrentPharmacistId(out int pharmacistId)
        {
            return int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out pharmacistId);
        }
    }
}
