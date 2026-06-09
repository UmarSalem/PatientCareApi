using PatientCareApi.Domain.Entities;

namespace PatientCareApi.Application.Repositories;

// Infrastructure will implement this interface with EF Core in a later feature.
public interface ITreatmentRepository
{
    Task<IReadOnlyList<Treatment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Treatment>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task AddAsync(Treatment treatment, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
