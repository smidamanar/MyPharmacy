using System.ComponentModel.DataAnnotations;

namespace MyPharmacy.Client.Models.Prescriptions
{

    public class PrescriptionUpdateDto
    {
        public int PrescriptionID { get; set; }
        public int PatientID { get; set; }
        public string Notes { get; set; } = string.Empty;
        public List<PrescriptionItemCreateDto> Items { get; set; } = new();
    }

}
