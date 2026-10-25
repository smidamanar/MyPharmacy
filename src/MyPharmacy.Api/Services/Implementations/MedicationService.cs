using MyPharmacy.Api.Models;
using MyPharmacy.Api.Interfaces;
using MyPharmacy.Api.Services.Interfaces;

namespace MyPharmacy.Api.Services.Implementations;

public class MedicationService : IMedicationService
{
    private readonly IMedicationRepository _repo;

    public MedicationService(IMedicationRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Medication>> GetAllAsync()
        => await _repo.GetAllAsync();

    public async Task<Medication?> GetByIdAsync(int id)
        => await _repo.GetByIdAsync(id);

    public async Task<Medication> CreateAsync(Medication medication)
    {
        await _repo.AddAsync(medication);
        await _repo.SaveChangesAsync();
        return medication;
    }

    public async Task<bool> UpdateAsync(Medication medication)
    {
        _repo.Update(medication);
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
