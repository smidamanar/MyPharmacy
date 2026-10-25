using MyPharmacy.Api.Models;
using MyPharmacy.Api.Interfaces;
using MyPharmacy.Api.Services.Interfaces;

namespace MyPharmacy.Api.Services.Implementations;

public class PharmacistService : IPharmacistService
{
    private readonly IPharmacistRepository _pharmacistRepo;

    public PharmacistService(IPharmacistRepository pharmacistRepo)
    {
        _pharmacistRepo = pharmacistRepo;
    }

    public async Task<Pharmacist?> GetByIdAsync(int id)
        => await _pharmacistRepo.GetByIdAsync(id);

    public async Task<IEnumerable<Pharmacist>> GetAllAsync()
        => await _pharmacistRepo.GetAllAsync();

    public async Task<Pharmacist?> GetByEmailAsync(string email)
        => await _pharmacistRepo.GetByEmailAsync(email);

    public async Task<Pharmacist> CreateAsync(Pharmacist pharmacist)
    {
        await _pharmacistRepo.AddAsync(pharmacist);
        await _pharmacistRepo.SaveChangesAsync();
        return pharmacist;
    }

    public async Task<bool> UpdateAsync(Pharmacist pharmacist)
    {
        _pharmacistRepo.Update(pharmacist);
        return await _pharmacistRepo.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _pharmacistRepo.GetByIdAsync(id);
        if (entity == null) return false;

        _pharmacistRepo.Remove(entity);
        return await _pharmacistRepo.SaveChangesAsync() > 0;
    }
}
