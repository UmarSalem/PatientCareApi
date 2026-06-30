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
- service implementations
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

The Api layer exposes HTTP endpoints through controllers.

Controllers will stay thin. They should call services and return HTTP responses, not contain business logic.

Current controllers:

- `PatientsController`
- `TreatmentsController`
- `AppointmentsController`

The API layer also contains `ErrorHandlingMiddleware`, which converts exceptions into HTTP error responses.

## Why DTOs Are Used

DTOs protect the API from exposing database entities directly. They let the API control what data comes in and what data goes out.

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

## How EF Core Migrations Work

Migrations are versioned database schema changes.

The first migration is `InitialCreate`. It creates:

- `Patients`
- `Treatments`
- `Appointments`
- foreign keys from treatments and appointments to patients
- indexes for patient lookup
- `__EFMigrationsHistory`, which EF Core uses to track applied migrations

The migration files are committed to Git. The generated SQLite database file is not committed because it is a local runtime artifact.

To apply migrations:

```powershell
dotnet ef database update --project PatientCareApi.Infrastructure --startup-project PatientCareApi.Api
```

## How Repository Implementations Work

Repository interfaces live in Application. Repository implementations live in Infrastructure because they use EF Core.

Read-only list queries use `AsNoTracking()` because EF Core does not need to track changes for data that is only being displayed.

Update and delete workflows return tracked entities, so the service can modify a domain object and `UnitOfWork` can save the change.

## How Dependency Injection Works

`Program.cs` registers interfaces with their implementations.

Examples:

```csharp
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();
```

This means controllers ask for interfaces, and ASP.NET Core creates the correct concrete classes at runtime.

## How The API Endpoints Work

Controllers receive HTTP requests, call Application services, and return HTTP responses.

For example, `PatientsController.Create` accepts a `CreatePatientRequest`, calls `IPatientService.CreateAsync`, and returns `201 Created`.

The controller does not validate business rules directly and does not talk to EF Core directly.

## How Error Handling Works

The API uses global exception handling middleware.

It maps:

- `NotFoundException` to `404 Not Found`
- `ValidationException` to `400 Bad Request`
- `ArgumentException` to `400 Bad Request`
- unexpected exceptions to `500 Internal Server Error`

This keeps controllers clean because they do not need repeated `try/catch` blocks.

In an interview, you can explain it like this:

> Application services throw meaningful exceptions. The API middleware catches those exceptions and translates them into proper HTTP status codes and JSON error responses.

## How Unit Tests Work

The first unit tests focus on Domain behavior.

They verify:

- creating a valid patient succeeds
- creating a patient with an empty first name fails
- completing an appointment changes the status to `Completed`

These tests are fast because they do not use the API, EF Core, SQLite, or Swagger.

In an interview, you can explain it like this:

> I started testing the Domain layer first because it contains core business rules and has no external dependencies. These tests confirm that the entities protect valid state and important behavior works as expected.

## How GitHub Actions CI Works

GitHub Actions is used for continuous integration.

The workflow runs automatically when code is pushed or when a pull request is opened against `development` or `main`.

The CI workflow performs three main checks:

1. Restore NuGet packages.
2. Build the solution.
3. Run the tests.

If any step fails, the pull request shows a failed check. This helps prevent broken code from being merged.

In an interview, you can explain it like this:

> I added a GitHub Actions CI pipeline so every feature branch and pull request is checked automatically. The pipeline restores dependencies, builds the solution, runs tests, and uploads test result artifacts.

## Interview Explanation

> I built a healthcare Web API using clean architecture. The Domain layer contains the business entities and rules. The Application layer contains DTOs, service contracts, service logic, repository contracts, and a Unit of Work contract. Infrastructure handles EF Core and SQLite. The Api layer exposes controllers and middleware.
