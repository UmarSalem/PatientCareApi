using PatientCareApi.Application.Dtos.Treatments;
using PatientCareApi.Application.Exceptions;
using PatientCareApi.Application.Mapping;
using PatientCareApi.Application.Repositories;
using PatientCareApi.Application.Validation;
using PatientCareApi.Domain.Entities;

namespace PatientCareApi.Application.Services;

public class TreatmentService : ITreatmentService
{
    private readonly ITreatmentRepository _treatmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TreatmentService(
        ITreatmentRepository treatmentRepository,
        IPatientRepository patientRepository,
        IUnitOfWork unitOfWork)
    {
        _treatmentRepository = treatmentRepository;
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<TreatmentResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var treatments = await _treatmentRepository.GetAllAsync(cancellationToken);
        return treatments.Select(treatment => treatment.ToResponse()).ToList();
    }

    public async Task<IReadOnlyList<TreatmentResponse>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        await EnsurePatientExistsAsync(patientId, cancellationToken);

        var treatments = await _treatmentRepository.GetByPatientIdAsync(patientId, cancellationToken);
        return treatments.Select(treatment => treatment.ToResponse()).ToList();
    }

    public async Task<TreatmentResponse> CreateForPatientAsync(
        int patientId,
        CreateTreatmentRequest request,
        CancellationToken cancellationToken = default)
    {
        RequestValidator.Validate(request);
        await EnsurePatientExistsAsync(patientId, cancellationToken);

        // The URL patient id is trusted over the body so treatments cannot be attached to the wrong patient.
        var treatment = new Treatment(
            patientId,
            request.Name,
            request.Description,
            request.TreatmentDate,
            request.Notes);

        await _treatmentRepository.AddAsync(treatment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return treatment.ToResponse();
    }

    private async Task EnsurePatientExistsAsync(int patientId, CancellationToken cancellationToken)
    {
        if (!await _patientRepository.ExistsAsync(patientId, cancellationToken))
        {
            throw new NotFoundException($"Patient with id {patientId} was not found.");
        }
    }
}
