using Microsoft.AspNetCore.Mvc;
using PatientCareApi.Application.Dtos.Appointments;
using PatientCareApi.Application.Services;

namespace PatientCareApi.Api.Controllers;

[ApiController]
[Route("api")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet("appointments")]
    public async Task<ActionResult<IReadOnlyList<AppointmentResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var appointments = await _appointmentService.GetAllAsync(cancellationToken);
        return Ok(appointments);
    }

    [HttpGet("patients/{patientId:int}/appointments")]
    public async Task<ActionResult<IReadOnlyList<AppointmentResponse>>> GetByPatient(
        int patientId,
        CancellationToken cancellationToken)
    {
        var appointments = await _appointmentService.GetByPatientIdAsync(patientId, cancellationToken);
        return Ok(appointments);
    }

    [HttpPost("appointments")]
    public async Task<ActionResult<AppointmentResponse>> Create(
        CreateAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        var appointment = await _appointmentService.CreateAsync(request, cancellationToken);
        return Created($"/api/appointments/{appointment.Id}", appointment);
    }

    [HttpPatch("appointments/{id:int}/complete")]
    public async Task<ActionResult<AppointmentResponse>> Complete(int id, CancellationToken cancellationToken)
    {
        // The controller does not set status directly; the service calls the Appointment domain method.
        var appointment = await _appointmentService.CompleteAsync(id, cancellationToken);
        return Ok(appointment);
    }

    [HttpPatch("appointments/{id:int}/cancel")]
    public async Task<ActionResult<AppointmentResponse>> Cancel(int id, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentService.CancelAsync(id, cancellationToken);
        return Ok(appointment);
    }
}
