using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPharmacy.Api.Models;

public class Prescription
{
    [Key]
    public int PrescriptionID { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public int PatientID { get; set; }

    [ForeignKey("PatientID")]
    public Patient? Patient { get; set; }

    [Required]
    public int PharmacistID { get; set; }

    [ForeignKey("PharmacistID")]
    public virtual Pharmacist? Pharmacist { get; set; }

    [StringLength(1_000)]
    public string? Notes { get; set; }


    public virtual ICollection<PrescriptionItem>? PrescriptionItems { get; set; } = new List<PrescriptionItem>();
}
