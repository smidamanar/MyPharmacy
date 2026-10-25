using MyPharmacy.Api.Models;

namespace MyPharmacy.Api.Interfaces;

public interface IPharmacistRepository : IGenericRepository<Pharmacist>
{
    Task<Pharmacist?> GetByEmailAsync(string email);
}
