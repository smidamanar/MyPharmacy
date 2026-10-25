using MyPharmacy.Api.Models;

namespace MyPharmacy.Api.Services.Interfaces;

public interface IMedicationService
{
    Task<IEnumerable<Medication>> GetAllAsync();
    Task<Medication?> GetByIdAsync(int id);
    Task<Medication> CreateAsync(Medication medication);
    Task<bool> UpdateAsync(Medication medication);
    Task<bool> DeleteAsync(int id);
}
