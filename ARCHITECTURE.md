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

Contains DTOs, repository interfaces, and service interfaces. It will also contain service implementations, validation, and mapping in later features.

Application will depend only on Domain.

Current Application folders:

- `Dtos`
- `Repositories`
- `Services`

The repository interfaces describe the persistence operations the Application layer needs. The service interfaces describe the use cases that the Api layer will call.

### PatientCareApi.Infrastructure

Will contain EF Core `DbContext` and repository implementations.

Infrastructure will depend on Application and Domain.

### PatientCareApi.Api

Will contain controllers, dependency injection, Swagger/OpenAPI, appsettings, and global error handling middleware.

Api will depend on Application and Infrastructure.

### PatientCareApi.Tests

Will contain unit tests for domain behavior and application services.

## Dependency Direction

```text
Api -> Application -> Domain
Api -> Infrastructure -> Application -> Domain
Tests -> projects needed for testing
```

The important rule is that Domain has no dependency on EF Core, ASP.NET Core, or any external framework.
