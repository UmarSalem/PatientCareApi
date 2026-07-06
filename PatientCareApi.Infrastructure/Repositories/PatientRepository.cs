using Microsoft.EntityFrameworkCore;
using PatientCareApi.Application.Repositories;
using PatientCareApi.Domain.Entities;
using PatientCareApi.Infrastructure.Data;

namespace PatientCareApi.Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly PatientCareDbContext _context;

    public PatientRepository(PatientCareDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Patient>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // AsNoTracking is faster for read-only screens because EF does not need to track changes.
        return await _context.Patients
            .AsNoTracking()
            .OrderBy(patient => patient.LastName)
            .ThenBy(patient => patient.FirstName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        // This query stays tracked because update/delete service methods may change the returned entity.
        return await _context.Patients
            .FirstOrDefaultAsync(patient => patient.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .AnyAsync(patient => patient.Id == id, cancellationToken);
    }

    public async Task AddAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        await _context.Patients.AddAsync(patient, cancellationToken);
    }

    public Task DeleteAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        _context.Patients.Remove(patient);
        return Task.CompletedTask;
    }
}
