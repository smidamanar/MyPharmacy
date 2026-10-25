using System.ComponentModel.DataAnnotations;
namespace MyPharmacy.Api.Models;

public class Pharmacist
{
    [Key]
    public int PharmacistID { get; set; }

    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [StringLength(150)]
    public string? Address { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    public string Role { get; set; } = "Pharmacist";

    public virtual ICollection<Prescription>? Prescriptions { get; set; }
}
