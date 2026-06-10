using PatientCareApi.Domain.Entities;

namespace PatientCareApi.Application.Repositories;

// This interface gives appointment services the data operations they need without exposing DbContext.
public interface IAppointmentRepository
{
    Task<IReadOnlyList<Appointment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Appointment>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<Appointment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Appointment appointment, CancellationToken cancellationToken = default);
}
