using PatientCareApi.Application.Dtos.Patients;

namespace PatientCareApi.Application.Services;

// Controllers call service interfaces so HTTP code stays separate from business workflow logic.
public interface IPatientService
{
    Task<IReadOnlyList<PatientResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PatientResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PatientResponse> CreateAsync(CreatePatientRequest request, CancellationToken cancellationToken = default);
    Task<PatientResponse> UpdateAsync(int id, UpdatePatientRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
