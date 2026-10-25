using System.ComponentModel.DataAnnotations;

namespace MyPharmacy.Client.Models.Prescriptions;

public class PrescriptionCreateDto
{
    [Required]
    public int PatientID { get; set; }

    [Required]
    public string Notes { get; set; } = string.Empty;

    public List<PrescriptionItemCreateDto> Items { get; set; } = new();
}