using Microsoft.AspNetCore.Mvc;
using PatientCareApi.Application.Dtos.Treatments;
using PatientCareApi.Application.Services;

namespace PatientCareApi.Api.Controllers;

[ApiController]
[Route("api")]
public class TreatmentsController : ControllerBase
{
    private readonly ITreatmentService _treatmentService;

    public TreatmentsController(ITreatmentService treatmentService)
    {
        _treatmentService = treatmentService;
    }

    [HttpGet("treatments")]
    public async Task<ActionResult<IReadOnlyList<TreatmentResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var treatments = await _treatmentService.GetAllAsync(cancellationToken);
        return Ok(treatments);
    }

    [HttpGet("patients/{patientId:int}/treatments")]
    public async Task<ActionResult<IReadOnlyList<TreatmentResponse>>> GetByPatient(
        int patientId,
        CancellationToken cancellationToken)
    {
        var treatments = await _treatmentService.GetByPatientIdAsync(patientId, cancellationToken);
        return Ok(treatments);
    }

    [HttpPost("patients/{patientId:int}/treatments")]
    public async Task<ActionResult<TreatmentResponse>> CreateForPatient(
        int patientId,
        CreateTreatmentRequest request,
        CancellationToken cancellationToken)
    {
        // Patient id comes from the route so the client cannot attach a treatment to a different patient in the body.
        var treatment = await _treatmentService.CreateForPatientAsync(patientId, request, cancellationToken);
        return CreatedAtAction(nameof(GetByPatient), new { patientId }, treatment);
    }
}
