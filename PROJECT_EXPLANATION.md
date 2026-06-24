# Project Explanation

PatientCareApi is a healthcare REST API practice project. The goal is to show clean, understandable ASP.NET Core Web API code that can be explained in an interview.

## What Each Layer Does

### Domain

The Domain layer contains the core business objects: `Patient`, `Treatment`, and `Appointment`.

The `Appointment` entity contains important behavior:

- `Complete()`
- `Cancel()`

This keeps appointment status changes inside the domain object instead of spreading that logic into controllers.

### Application

The Application layer contains use-case focused code:

- DTOs
- service interfaces
- repository interfaces
- validation
- mapping
- application exceptions
- Unit of Work contract

This layer coordinates business workflows without knowing database details.

### Infrastructure

The Infrastructure layer contains EF Core and SQLite persistence.

It implements repository interfaces and `IUnitOfWork` from Application.

Important Infrastructure classes:

- `PatientCareDbContext`
- `PatientRepository`
- `TreatmentRepository`
- `AppointmentRepository`
- `UnitOfWork`

### Api

The Api layer will expose HTTP endpoints through controllers.

Controllers will stay thin. They should call services and return HTTP responses, not contain business logic.

## Why DTOs Are Used

DTOs protect the API from exposing database entities directly. They let the API control what data comes in and what data goes out.

For example, `CreatePatientRequest` represents the JSON body used to create a patient, while `PatientResponse` represents the data returned by the API.

## Why A Service Layer Is Used

Services keep controllers small and focused. A controller receives HTTP input, calls a service, and returns a response.

The service implementations validate requests, check whether required records exist, call domain entities, save through Unit of Work, and return DTO responses.

## Why Repository Interfaces Are Used

Repository interfaces let Application describe what data operations it needs without depending on EF Core directly. Infrastructure provides the actual EF Core implementation.

## Why Unit Of Work Is Used

Some projects call `SaveChangesAsync()` inside each repository or DAO method. That is simple and works well for small CRUD examples.

This project uses `IUnitOfWork` instead. Repositories describe data operations, while `IUnitOfWork` represents committing all pending changes.

The benefit is that a service can coordinate multiple repositories and save once at the end of the workflow.

In an interview, you can explain it like this:

> The service decides when the workflow is complete. The repository handles data access, and Unit of Work handles the save operation. The actual EF Core `SaveChangesAsync` call lives in Infrastructure.

## How EF Core Will Be Configured

EF Core entity mapping is configured in `PatientCareDbContext`. API startup will register the DbContext in `Program.cs` using `AddDbContext`. The SQLite connection string will come from `appsettings.json`, not from a hardcoded path inside the `DbContext`.

`PatientCareDbContext` defines the `Patients`, `Treatments`, and `Appointments` tables, configures required fields, max lengths, relationships, cascade delete, and stores `AppointmentStatus` as a readable string.

## How Repository Implementations Work

Repository interfaces live in Application. Repository implementations live in Infrastructure because they use EF Core.

Read-only list queries use `AsNoTracking()` because EF Core does not need to track changes for data that is only being displayed.

Update and delete workflows return tracked entities, so the service can modify a domain object and `UnitOfWork` can save the change.

## Interview Explanation

> I built a healthcare Web API using clean architecture. The Domain layer contains the business entities and rules. The Application layer contains DTOs, service contracts, service logic, repository contracts, and a Unit of Work contract. Infrastructure handles EF Core and SQLite. The Api layer exposes controllers and middleware.
