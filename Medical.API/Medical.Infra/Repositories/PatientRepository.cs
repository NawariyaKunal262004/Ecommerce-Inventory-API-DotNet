namespace Medical.Infra.Repositories;
public class PatientRepository : RepositoryBase<PatientEntity>, IPatientRepository
{
    public PatientRepository(ApplicationDbContext context, ILogger<RepositoryBase<PatientEntity>> logger) : base(context, logger)
    {
    }

    public async Task<PatientEntity?> GetPatientWithBillsAsync(Guid patientId)
    {
        return await _context.Patients
            .Include(p => p.Bills)
            .FirstOrDefaultAsync(p => p.PatientId == patientId);
    }

    public async Task<IReadOnlyList<BillEntity>> GetPatientBillsAsync(Guid patientId)
    {
        return await _context.Bills
            .Include(b => b.BillItems)
            .ThenInclude(bi => bi.Medicine)
            .AsNoTracking()
            .Where(b => b.PatientId == patientId)
            .OrderByDescending(b => b.CreatedDate)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<PatientEntity>> GetAllPatientsAsync()
    {
        return await _context.Patients
            .OrderBy(p => p.PatientName)
            .ToListAsync();
    }
}
