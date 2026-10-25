using System.ComponentModel.DataAnnotations;

namespace MyPharmacy.Client.Models.Prescriptions
{
    public class PrescriptionItemCreateDto
    {
        [Required]
        public int MedicationID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
