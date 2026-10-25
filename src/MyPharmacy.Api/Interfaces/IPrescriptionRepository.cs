using MyPharmacy.Api.Models;

namespace MyPharmacy.Api.Interfaces;

// Retained for read-model queries. Transactional prescription writes are owned by PrescriptionService.
public interface IPrescriptionRepository
{
    Task<Prescription> CreateAsync(Prescription prescription);
    Task<bool> UpdateAsync(Prescription prescription);
    Task<bool> DeleteAsync(int id);

    Task<Prescription?> GetFullPrescriptionAsync(int id);
    Task<IEnumerable<Prescription>> GetAllFullAsync();
}
