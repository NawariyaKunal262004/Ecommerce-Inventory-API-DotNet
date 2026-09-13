namespace Medical.Core.IRepositories;
public interface IPatientRepository : IAsyncRepository<PatientEntity>
{
    Task<PatientEntity?> GetPatientWithBillsAsync(Guid patientId);

    Task<IReadOnlyList<BillEntity>> GetPatientBillsAsync(Guid patientId);

    Task<IReadOnlyList<PatientEntity>> GetAllPatientsAsync();
}
