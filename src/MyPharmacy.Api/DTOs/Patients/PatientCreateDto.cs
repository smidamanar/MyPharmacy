using System.ComponentModel.DataAnnotations;

namespace MyPharmacy.Api.DTOs.Patients;

public class PatientCreateDto
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string NationalId { get; set; } = string.Empty;

    [Required]
    public DateTime BirthDate { get; set; }

    [StringLength(150)]
    public string? Address { get; set; }
}
