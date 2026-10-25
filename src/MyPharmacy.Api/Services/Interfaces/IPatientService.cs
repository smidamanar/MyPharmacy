using MyPharmacy.Api.Models;

namespace MyPharmacy.Api.Services.Interfaces;

public interface IPatientService
{
    Task<Patient?> GetByIdAsync(int id);
    Task<IEnumerable<Patient>> GetAllAsync();
    Task<Patient> CreateAsync(Patient patient);
    Task<bool> UpdateAsync(Patient patient);
    Task<bool> DeleteAsync(int id);
}
