using MyPharmacy.Api.Models;

namespace MyPharmacy.Api.Services.Interfaces;

public interface IPharmacistService
{
    Task<Pharmacist?> GetByIdAsync(int id);
    Task<IEnumerable<Pharmacist>> GetAllAsync();
    Task<Pharmacist?> GetByEmailAsync(string email);
    Task<Pharmacist> CreateAsync(Pharmacist pharmacist);
    Task<bool> UpdateAsync(Pharmacist pharmacist);
    Task<bool> DeleteAsync(int id);
}
