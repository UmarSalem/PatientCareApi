# Architecture

PatientCareApi uses a clean layered architecture. The main idea is that business rules should live close to the center of the system, while database and web framework details stay on the outside.

## Projects

### PatientCareApi.Domain

Contains entities, enums, and domain behavior.

Current domain files:

- `Patient`
- `Treatment`
- `Appointment`
- `AppointmentStatus`

The Domain project does not depend on any other project.

### PatientCareApi.Application

Contains DTOs, repository interfaces, service interfaces, service implementations, validation, mapping, application exceptions, and the Unit of Work contract.

Application depends only on Domain.

Current Application folders:

- `Dtos`
- `Repositories`
- `Services`
- `Mapping`
- `Validation`
- `Exceptions`

Repository interfaces describe data access operations. Service interfaces describe use cases. Service implementations coordinate workflows. `IUnitOfWork` represents committing all pending repository changes.

Current Application folders:

- `Dtos`
- `Repositories`
- `Services`

The repository interfaces describe the persistence operations the Application layer needs. The service interfaces describe the use cases that the Api layer will call.

### PatientCareApi.Infrastructure

Contains EF Core `DbContext`, repository implementations, and the Unit of Work implementation.

Infrastructure depends on Application and Domain.

Current Infrastructure folders:

- `Data`
- `Repositories`

`PatientCareDbContext` maps domain entities to database tables. Repository classes use EF Core to query and stage changes. `UnitOfWork` calls `SaveChangesAsync` once a service workflow is ready to commit.

### PatientCareApi.Api

Contains controllers, dependency injection, Swagger/OpenAPI, appsettings, and global error handling middleware.

Api depends on Application and Infrastructure.

`Program.cs` is the composition root. It registers the DbContext, repositories, Unit of Work, and services. Controllers depend on service interfaces so they stay thin and do not contain business logic.

`ErrorHandlingMiddleware` converts exceptions from the Application layer into consistent HTTP responses. This keeps error response logic out of controllers.

### PatientCareApi.Tests

Will contain unit tests for domain behavior and application services.

## Dependency Direction

```text
Api -> Application -> Domain
Api -> Infrastructure -> Application -> Domain
Tests -> projects needed for testing
```

The important rule is that Domain has no dependency on EF Core, ASP.NET Core, or any external framework.
