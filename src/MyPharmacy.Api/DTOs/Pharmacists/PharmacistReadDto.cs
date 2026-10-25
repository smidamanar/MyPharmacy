using System.ComponentModel.DataAnnotations;

namespace MyPharmacy.Api.DTOs.Pharmacists;

public class PharmacistReadDto
{
    public int PharmacistID { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }

    [RegularExpression("^(Pharmacist|Admin)$")]
    public string? Role { get; set; }
}
