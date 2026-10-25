using MyPharmacy.Api.Models;
using MyPharmacy.Api.Interfaces;
using MyPharmacy.Api.Services.Interfaces;

namespace MyPharmacy.Api.Services.Implementations;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _repo;

    public PatientService(IPatientRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Patient>> GetAllAsync()
        => await _repo.GetAllAsync();

    public async Task<Patient?> GetByIdAsync(int id)
        => await _repo.GetByIdAsync(id);

    public async Task<Patient> CreateAsync(Patient patient)
    {
        await _repo.AddAsync(patient);
        await _repo.SaveChangesAsync();
        return patient;
    }

    public async Task<bool> UpdateAsync(Patient patient)
    {
        _repo.Update(patient);
        return await _repo.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;

        _repo.Remove(entity);
        return await _repo.SaveChangesAsync() > 0;
    }
}
