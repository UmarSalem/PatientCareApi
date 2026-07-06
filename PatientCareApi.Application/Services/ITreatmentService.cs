using PatientCareApi.Application.Dtos.Treatments;

namespace PatientCareApi.Application.Services;

// Treatment use cases are grouped behind a service contract for thin, easy-to-test controllers.
public interface ITreatmentService
{
    Task<IReadOnlyList<TreatmentResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TreatmentResponse>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<TreatmentResponse> CreateForPatientAsync(int patientId, CreateTreatmentRequest request, CancellationToken cancellationToken = default);
}
