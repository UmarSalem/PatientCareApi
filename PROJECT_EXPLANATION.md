# Project Explanation

PatientCareApi is a healthcare REST API practice project. The goal is to show clean, understandable ASP.NET Core Web API code that can be explained in an interview.

## What Each Layer Does

### Domain

The Domain layer contains the core business objects. In this project, those are `Patient`, `Treatment`, and `Appointment`.

The `Appointment` entity contains important behavior:

- `Complete()`
- `Cancel()`

This keeps appointment status changes inside the domain object instead of spreading that logic into controllers.

### Application

The Application layer contains use-case focused contracts:

- DTOs
- service interfaces
- repository interfaces

It will contain service implementations, validation, and mapping in the next Application feature.

This layer will coordinate business workflows without knowing database details.

### Infrastructure

The Infrastructure layer will contain EF Core and SQLite persistence.

It will implement repository interfaces defined by Application.

### Api

The Api layer will expose HTTP endpoints through controllers.

Controllers will stay thin. They should call services and return HTTP responses, not contain business logic.

## Why DTOs Are Used

DTOs protect the API from exposing database entities directly. They let the API control what data comes in and what data goes out.

For example, `CreatePatientRequest` represents the JSON body used to create a patient, while `PatientResponse` represents the data returned by the API.

## Why A Service Layer Is Used

Services keep controllers small and focused. A controller receives HTTP input, calls a service, and returns a response.

The service interfaces added in Feature 02 describe the operations the API needs, such as creating patients, adding treatments, and completing appointments.

## Why Repository Interfaces Are Used

Repository interfaces let Application describe what data operations it needs without depending on EF Core directly. Infrastructure provides the actual EF Core implementation.

This means Application can ask for `IPatientRepository` without knowing whether the data comes from SQLite, SQL Server, or another storage system.

## How EF Core Will Be Configured

EF Core will be configured in `Program.cs` using `AddDbContext`. The SQLite connection string will come from `appsettings.json`, not from a hardcoded path inside the `DbContext`.

## How Dependency Injection Will Work

The Api project will register services and repositories in the dependency injection container. Controllers will ask for service interfaces through constructors.

## Interview Explanation

You can explain this project like this:

> I built a healthcare Web API using clean architecture. The Domain layer contains the business entities and rules. The Application layer contains DTOs, service contracts, service logic, and repository contracts. Infrastructure handles EF Core and SQLite. The Api layer exposes controllers and middleware. This keeps business logic away from controllers and keeps EF Core outside the core business model.
