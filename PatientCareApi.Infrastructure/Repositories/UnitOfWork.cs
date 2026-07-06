using PatientCareApi.Application.Repositories;
using PatientCareApi.Infrastructure.Data;

namespace PatientCareApi.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly PatientCareDbContext _context;

    public UnitOfWork(PatientCareDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // One save point makes it easier for a service to coordinate multiple repository changes.
        await _context.SaveChangesAsync(cancellationToken);
    }
}
