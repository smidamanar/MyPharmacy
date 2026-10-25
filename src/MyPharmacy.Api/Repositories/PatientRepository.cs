using MyPharmacy.Api.Data;
using MyPharmacy.Api.Interfaces;
using MyPharmacy.Api.Models;

namespace MyPharmacy.Api.Repositories;

public class PatientRepository : GenericRepository<Patient>, IPatientRepository
{
    public PatientRepository(AppDbContext context) : base(context) { }
}
