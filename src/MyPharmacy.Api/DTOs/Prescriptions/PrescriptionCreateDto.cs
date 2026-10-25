using System.ComponentModel.DataAnnotations;

namespace MyPharmacy.Api.DTOs.Prescriptions;

public class PrescriptionCreateDto
{
    [Required]
    public int PatientID { get; set; }

    [StringLength(1000)]
    public string Notes { get; set; } = string.Empty;

    [Required]
    public List<PrescriptionItemCreateDto> Items { get; set; } = new();
}
