using PatientCareApi.Application.Dtos.Patients;
using PatientCareApi.Application.Exceptions;
using PatientCareApi.Application.Mapping;
using PatientCareApi.Application.Repositories;
using PatientCareApi.Application.Validation;
using PatientCareApi.Domain.Entities;

namespace PatientCareApi.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PatientService(IPatientRepository patientRepository, IUnitOfWork unitOfWork)
    {
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<PatientResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var patients = await _patientRepository.GetAllAsync(cancellationToken);
        return patients.Select(patient => patient.ToResponse()).ToList();
    }

    public async Task<PatientResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var patient = await GetPatientOrThrowAsync(id, cancellationToken);
        return patient.ToResponse();
    }

    public async Task<PatientResponse> CreateAsync(CreatePatientRequest request, CancellationToken cancellationToken = default)
    {
        RequestValidator.Validate(request);
        RequestValidator.EnsureDateOfBirthIsInPast(request.DateOfBirth);

        // The domain constructor enforces core patient rules before the entity can exist.
        var patient = new Patient(
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.Email,
            request.PhoneNumber);

        await _patientRepository.AddAsync(patient, cancellationToken);

        // Repositories stage data changes; UnitOfWork commits them with one database save.
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return patient.ToResponse();
    }

    public async Task<PatientResponse> UpdateAsync(int id, UpdatePatientRequest request, CancellationToken cancellationToken = default)
    {
        RequestValidator.Validate(request);
        RequestValidator.EnsureDateOfBirthIsInPast(request.DateOfBirth);

        var patient = await GetPatientOrThrowAsync(id, cancellationToken);

        // Update is a domain method so validation and UpdatedAt changes stay with the entity.
        patient.Update(
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.Email,
            request.PhoneNumber);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return patient.ToResponse();
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var patient = await GetPatientOrThrowAsync(id, cancellationToken);

        await _patientRepository.DeleteAsync(patient, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<Patient> GetPatientOrThrowAsync(int id, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetByIdAsync(id, cancellationToken);

        return patient ?? throw new NotFoundException($"Patient with id {id} was not found.");
    }
}
