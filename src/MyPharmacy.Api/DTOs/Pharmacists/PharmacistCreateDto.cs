using System.ComponentModel.DataAnnotations;

namespace MyPharmacy.Api.DTOs.Pharmacists;

public class PharmacistCreateDto
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [StringLength(150)]
    public string? Address { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [RegularExpression("^(Pharmacist|Admin)$")]
    public string? Role { get; set; }

}
