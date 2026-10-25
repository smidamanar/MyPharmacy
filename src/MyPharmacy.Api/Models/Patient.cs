using System.ComponentModel.DataAnnotations;
namespace MyPharmacy.Api.Models;

public class Patient
{
    [Key]
    public int PatientID { get; set; }
    [Required]
    [StringLength(50)]
    public string FullName { get; set; } = string.Empty;
    [Required]
    [StringLength(50)]
    public string NationalId { get; set; } = string.Empty;

    [Required]
    public DateTime BirthDate { get; set; }

    [StringLength(150)]
    public string? Address { get; set; }

    public virtual ICollection<Prescription>? Prescriptions { get; set; }
}
