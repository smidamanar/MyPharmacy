using MyPharmacy.Api.Data;
using MyPharmacy.Api.Interfaces;
using MyPharmacy.Api.Models;

namespace MyPharmacy.Api.Repositories;

public class MedicationRepository : GenericRepository<Medication>, IMedicationRepository
{
    public MedicationRepository(AppDbContext context) : base(context) { }
}
