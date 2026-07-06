using Microsoft.EntityFrameworkCore;
using PatientCareApi.Application.Repositories;
using PatientCareApi.Domain.Entities;
using PatientCareApi.Infrastructure.Data;

namespace PatientCareApi.Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly PatientCareDbContext _context;

    public AppointmentRepository(PatientCareDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Appointment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Appointments
            .AsNoTracking()
            .OrderBy(appointment => appointment.AppointmentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Appointment>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Appointments
            .AsNoTracking()
            .Where(appointment => appointment.PatientId == patientId)
            .OrderBy(appointment => appointment.AppointmentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Appointment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        // This tracked entity lets AppointmentService call Complete or Cancel and then save through UnitOfWork.
        return await _context.Appointments
            .FirstOrDefaultAsync(appointment => appointment.Id == id, cancellationToken);
    }

    public async Task AddAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        await _context.Appointments.AddAsync(appointment, cancellationToken);
    }
}
