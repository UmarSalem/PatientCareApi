namespace PatientCareApi.Application.Repositories;

// Unit of Work represents committing all repository changes as one database save operation.
public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
