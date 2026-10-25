using Microsoft.EntityFrameworkCore;
using MyPharmacy.Api.Data;
using MyPharmacy.Api.Interfaces;
using MyPharmacy.Api.Models;

namespace MyPharmacy.Api.Repositories;

public class PharmacistRepository : GenericRepository<Pharmacist>, IPharmacistRepository
{
    public PharmacistRepository(AppDbContext context) : base(context) { }

    public async Task<Pharmacist?> GetByEmailAsync(string email)
    {
        return await _context.Pharmacists
            .FirstOrDefaultAsync(p => p.Email == email);
    }
}
