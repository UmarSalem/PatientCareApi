using Microsoft.EntityFrameworkCore;
using PatientCareApi.Application.Repositories;
using PatientCareApi.Domain.Entities;
using PatientCareApi.Infrastructure.Data;

namespace PatientCareApi.Infrastructure.Repositories;

public class TreatmentRepository : ITreatmentRepository
{
    private readonly PatientCareDbContext _context;

    public TreatmentRepository(PatientCareDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Treatment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Treatments
            .AsNoTracking()
            .OrderByDescending(treatment => treatment.TreatmentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Treatment>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        // Filtering in the database avoids loading all treatments into memory.
        return await _context.Treatments
            .AsNoTracking()
            .Where(treatment => treatment.PatientId == patientId)
            .OrderByDescending(treatment => treatment.TreatmentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Treatment treatment, CancellationToken cancellationToken = default)
    {
        await _context.Treatments.AddAsync(treatment, cancellationToken);
    }
}
