using Microsoft.AspNetCore.Mvc;
using PatientCareApi.Application.Dtos.Patients;
using PatientCareApi.Application.Services;

namespace PatientCareApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PatientResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var patients = await _patientService.GetAllAsync(cancellationToken);
        return Ok(patients);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PatientResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var patient = await _patientService.GetByIdAsync(id, cancellationToken);
        return Ok(patient);
    }

    [HttpPost]
    public async Task<ActionResult<PatientResponse>> Create(
        CreatePatientRequest request,
        CancellationToken cancellationToken)
    {
        var patient = await _patientService.CreateAsync(request, cancellationToken);

        // 201 Created tells the client a new resource was created and where it can be fetched.
        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PatientResponse>> Update(
        int id,
        UpdatePatientRequest request,
        CancellationToken cancellationToken)
    {
        var patient = await _patientService.UpdateAsync(id, request, cancellationToken);
        return Ok(patient);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _patientService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
