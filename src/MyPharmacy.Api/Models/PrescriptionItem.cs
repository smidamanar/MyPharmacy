using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPharmacy.Api.Models;

public class PrescriptionItem
{
    [Required]
    public int PrescriptionID { get; set; }

    [ForeignKey("PrescriptionID")]
    public Prescription? Prescription { get; set; }

    [Required]
    public int MedicationID { get; set; }

    [ForeignKey("MedicationID")]
    public virtual Medication? Medication { get; set; }

    [Required]
    public int Quantity { get; set; }
}
