using PatientCareApi.Application.Dtos.Appointments;
using PatientCareApi.Application.Exceptions;
using PatientCareApi.Application.Mapping;
using PatientCareApi.Application.Repositories;
using PatientCareApi.Application.Validation;
using PatientCareApi.Domain.Entities;

namespace PatientCareApi.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IPatientRepository patientRepository,
        IUnitOfWork unitOfWork)
    {
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<AppointmentResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var appointments = await _appointmentRepository.GetAllAsync(cancellationToken);
        return appointments.Select(appointment => appointment.ToResponse()).ToList();
    }

    public async Task<IReadOnlyList<AppointmentResponse>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        await EnsurePatientExistsAsync(patientId, cancellationToken);

        var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId, cancellationToken);
        return appointments.Select(appointment => appointment.ToResponse()).ToList();
    }

    public async Task<AppointmentResponse> CreateAsync(
        CreateAppointmentRequest request,
        CancellationToken cancellationToken = default)
    {
        RequestValidator.Validate(request);
        await EnsurePatientExistsAsync(request.PatientId, cancellationToken);

        var appointment = new Appointment(
            request.PatientId,
            request.AppointmentDate,
            request.Reason);

        await _appointmentRepository.AddAsync(appointment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return appointment.ToResponse();
    }

    public async Task<AppointmentResponse> CompleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var appointment = await GetAppointmentOrThrowAsync(id, cancellationToken);

        // Business transition is delegated to the domain entity, not duplicated in the service.
        appointment.Complete();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return appointment.ToResponse();
    }

    public async Task<AppointmentResponse> CancelAsync(int id, CancellationToken cancellationToken = default)
    {
        var appointment = await GetAppointmentOrThrowAsync(id, cancellationToken);

        appointment.Cancel();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return appointment.ToResponse();
    }

    private async Task<Appointment> GetAppointmentOrThrowAsync(int id, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id, cancellationToken);

        return appointment ?? throw new NotFoundException($"Appointment with id {id} was not found.");
    }

    private async Task EnsurePatientExistsAsync(int patientId, CancellationToken cancellationToken)
    {
        if (!await _patientRepository.ExistsAsync(patientId, cancellationToken))
        {
            throw new NotFoundException($"Patient with id {patientId} was not found.");
        }
    }
}
