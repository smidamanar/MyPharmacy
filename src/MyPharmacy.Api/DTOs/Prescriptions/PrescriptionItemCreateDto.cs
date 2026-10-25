using System.ComponentModel.DataAnnotations;

namespace MyPharmacy.Api.DTOs.Prescriptions;

public class PrescriptionItemCreateDto
{
    [Required]
    public int MedicationID { get; set; }


    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}
