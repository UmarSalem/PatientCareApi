using PatientCareApi.Application.Dtos.Appointments;

namespace PatientCareApi.Application.Services;

// Appointment state changes go through the service, which will call domain methods like Complete and Cancel.
public interface IAppointmentService
{
    Task<IReadOnlyList<AppointmentResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AppointmentResponse>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<AppointmentResponse> CreateAsync(CreateAppointmentRequest request, CancellationToken cancellationToken = default);
    Task<AppointmentResponse> CompleteAsync(int id, CancellationToken cancellationToken = default);
    Task<AppointmentResponse> CancelAsync(int id, CancellationToken cancellationToken = default);
}
