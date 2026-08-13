# Learning Notes

This file is updated after each major feature so the project can be explained step by step.

## Feature 01: Solution Architecture And Domain Model

### What Was Created

- A .NET solution named `PatientCareApi`
- Five projects:
  - `PatientCareApi.Domain`
  - `PatientCareApi.Application`
  - `PatientCareApi.Infrastructure`
  - `PatientCareApi.Api`
  - `PatientCareApi.Tests`
- Domain entities:
  - `Patient`
  - `Treatment`
  - `Appointment`
- Domain enum:
  - `AppointmentStatus`

### Why It Was Created

The solution structure separates responsibilities:

- Domain contains business rules.
- Application coordinates use cases.
- Infrastructure handles database access.
- Api exposes HTTP endpoints.
- Tests verify behavior.

The domain entities use private setters to protect state. Important state changes, like completing or cancelling an appointment, happen through domain methods.

### How To Explain It In An Interview

> I started by creating a clean architecture solution. The Domain project has no dependency on ASP.NET Core or EF Core. It contains the main healthcare concepts and protects important state with private setters and domain methods.

## Feature 02: Application DTOs And Contracts

### What Was Created

- Patient DTOs:
  - `CreatePatientRequest`
  - `UpdatePatientRequest`
  - `PatientResponse`
- Treatment DTOs:
  - `CreateTreatmentRequest`
  - `TreatmentResponse`
- Appointment DTOs:
  - `CreateAppointmentRequest`
  - `AppointmentResponse`
- Repository interfaces:
  - `IPatientRepository`
  - `ITreatmentRepository`
  - `IAppointmentRepository`
  - `IUnitOfWork`
- Service interfaces:
  - `IPatientService`
  - `ITreatmentService`
  - `IAppointmentService`

### Why It Was Created

DTOs define what data enters and leaves the API. Repository interfaces define what data operations the service layer needs. `IUnitOfWork` defines the save operation. Service interfaces define the use cases that controllers will call.

### How To Explain It In An Interview

> I added the Application layer contracts before writing controllers or database code. The DTOs shape API input and output. The service interfaces describe use cases, the repository interfaces describe persistence needs, and Unit of Work describes saving changes.

## Feature 03: Application Service Implementations

### What Was Created

- Custom exception:
  - `NotFoundException`
- Validation helper:
  - `RequestValidator`
- Mapping extension classes:
  - `PatientMappingExtensions`
  - `TreatmentMappingExtensions`
  - `AppointmentMappingExtensions`
- Service implementations:
  - `PatientService`
  - `TreatmentService`
  - `AppointmentService`

### Why It Was Created

This feature adds the real Application-layer workflows.

The services validate request DTOs, check that related records exist, create or update domain entities, call repository interfaces, save through `IUnitOfWork`, and return response DTOs.

This keeps controllers thin because controllers will only call services and return HTTP responses.

Using `IUnitOfWork` also keeps saving separate from repository methods. Repositories describe data access, and Unit of Work describes committing changes.

### Files Changed

- `PatientCareApi.Application/Dtos`
- `PatientCareApi.Application/Repositories`
- `PatientCareApi.Application/Services`
- `PatientCareApi.Application/Exceptions/NotFoundException.cs`
- `PatientCareApi.Application/Validation/RequestValidator.cs`
- `PatientCareApi.Application/Mapping`
- `README.md`
- `PROJECT_EXPLANATION.md`
- `ARCHITECTURE.md`
- `API_ENDPOINTS.md`
- `LEARNING_NOTES.md`

### How To Test It

Run:

```powershell
dotnet build --no-restore
```

### How To Explain It In An Interview

> I implemented the Application services as the use-case layer. The services validate DTOs, check for missing records, use repositories through interfaces, call domain methods for important behavior, save through Unit of Work, and return response DTOs. For example, completing an appointment loads the appointment, calls `appointment.Complete()`, saves changes through `IUnitOfWork`, and maps the result to `AppointmentResponse`.

### GitHub Commands For This Feature

If Feature 02 is already merged into `development`, use:

```powershell
git add .
git commit -m "Add application service implementations"
git push -u origin feature/03-application-services
```

If Feature 02 is not merged into `development`, create the Feature 03 PR against Feature 02:

```text
base: feature/02-application-contracts
compare: feature/03-application-services
```

## Feature 04: Infrastructure Persistence With EF Core

### What Was Created

- EF Core DbContext:
  - `PatientCareDbContext`
- Repository implementations:
  - `PatientRepository`
  - `TreatmentRepository`
  - `AppointmentRepository`
- Unit of Work implementation:
  - `UnitOfWork`
- EF Core SQLite package reference in Infrastructure

### Why It Was Created

The Application layer defines repository interfaces and `IUnitOfWork`. This feature adds the Infrastructure classes that implement those abstractions using EF Core.

This keeps EF Core out of the Domain and Application layers.

Repositories query and stage data changes. `UnitOfWork` commits those changes by calling `DbContext.SaveChangesAsync`.

### Files Changed

- `PatientCareApi.Infrastructure/PatientCareApi.Infrastructure.csproj`
- `PatientCareApi.Infrastructure/Data/PatientCareDbContext.cs`
- `PatientCareApi.Infrastructure/Repositories/PatientRepository.cs`
- `PatientCareApi.Infrastructure/Repositories/TreatmentRepository.cs`
- `PatientCareApi.Infrastructure/Repositories/AppointmentRepository.cs`
- `PatientCareApi.Infrastructure/Repositories/UnitOfWork.cs`
- `PatientCareApi.Application/Services/PatientService.cs`
## Feature 02: Application DTOs And Contracts

### What Was Created

- Patient DTOs:
  - `CreatePatientRequest`
  - `UpdatePatientRequest`
  - `PatientResponse`
- Treatment DTOs:
  - `CreateTreatmentRequest`
  - `TreatmentResponse`
- Appointment DTOs:
  - `CreateAppointmentRequest`
  - `AppointmentResponse`
- Repository interfaces:
  - `IPatientRepository`
  - `ITreatmentRepository`
  - `IAppointmentRepository`
- Service interfaces:
  - `IPatientService`
  - `ITreatmentService`
  - `IAppointmentService`

### Why It Was Created

This feature creates the Application layer contracts.

DTOs define what data enters and leaves the API. Repository interfaces define what data operations the service layer needs. Service interfaces define the use cases that controllers will call.

This keeps the API controllers thin and keeps EF Core details outside the Application layer.

### Files Changed

- `PatientCareApi.Application/Dtos/Patients/CreatePatientRequest.cs`
- `PatientCareApi.Application/Dtos/Patients/UpdatePatientRequest.cs`
- `PatientCareApi.Application/Dtos/Patients/PatientResponse.cs`
- `PatientCareApi.Application/Dtos/Treatments/CreateTreatmentRequest.cs`
- `PatientCareApi.Application/Dtos/Treatments/TreatmentResponse.cs`
- `PatientCareApi.Application/Dtos/Appointments/CreateAppointmentRequest.cs`
- `PatientCareApi.Application/Dtos/Appointments/AppointmentResponse.cs`
- `PatientCareApi.Application/Repositories/IPatientRepository.cs`
- `PatientCareApi.Application/Repositories/ITreatmentRepository.cs`
- `PatientCareApi.Application/Repositories/IAppointmentRepository.cs`
- `PatientCareApi.Application/Services/IPatientService.cs`
- `PatientCareApi.Application/Services/ITreatmentService.cs`
- `PatientCareApi.Application/Services/IAppointmentService.cs`
- `README.md`
- `PROJECT_EXPLANATION.md`
- `ARCHITECTURE.md`
- `API_ENDPOINTS.md`
- `LEARNING_NOTES.md`

### How To Test It

Run:

```powershell
dotnet restore --configfile NuGet.Config
dotnet build --no-restore
```

### Repository Implementation Explanation

`PatientRepository` handles patient database operations. `GetAllAsync` uses `AsNoTracking` for read-only performance. `GetByIdAsync` returns a tracked entity because update and delete workflows need EF Core to detect changes.

`TreatmentRepository` handles treatment queries and creation. It filters by `PatientId` in the database instead of loading every treatment into memory.

`AppointmentRepository` handles appointment queries and creation. `GetByIdAsync` returns a tracked appointment so `AppointmentService` can call `Complete()` or `Cancel()` and save the updated status.

`UnitOfWork` owns the final save operation. Repositories do not call `SaveChangesAsync` themselves.

### How To Explain It In An Interview

> I implemented the Infrastructure layer with EF Core. The Application layer defines repository interfaces, and Infrastructure implements them with `PatientCareDbContext`. Read-only queries use `AsNoTracking` for performance. Update scenarios return tracked entities so domain methods can change state and Unit of Work can save once at the end.

### GitHub Commands For This Feature

```powershell
git stash push -u -m "feature 04 infrastructure persistence"     # Save current uncommitted changes, including new files
git checkout development                                         # Switch to the development branch
git pull origin development                                      # Get latest development code from GitHub
git checkout -B feature/04-infrastructure-persistence            # Create/reset the feature branch from development
git stash pop                                                    # Bring saved Feature 04 changes onto this branch
dotnet restore --configfile NuGet.Config                         # Restore EF Core SQLite package
dotnet build --no-restore                                        # Confirm the project builds
git add .                                                        # Stage all changed files
git commit -m "Add infrastructure persistence"                   # Save the feature as a commit
git push -u origin feature/04-infrastructure-persistence         # Push the feature branch to GitHub
dotnet build
```

This confirms the Application project still compiles and the dependency rule is respected: Application depends only on Domain.

### How To Explain It In An Interview

You can say:

> I added the Application layer contracts before writing controllers or database code. The DTOs shape API input and output. The service interfaces describe use cases, and the repository interfaces describe persistence needs. This keeps the controller layer thin and keeps EF Core out of the Application layer.

### GitHub Commands For This Feature

Use this branch flow:

```powershell
git checkout development
git pull origin development
git checkout -b feature/02-application-contracts
git status
git add .
git commit -m "Add application DTOs and contracts"
git push -u origin feature/02-application-contracts
```

Create the pull request:

```text
base: development
compare: feature/04-infrastructure-persistence
compare: feature/02-application-contracts
```

## Feature 06: Global Error Handling Middleware

### What Was Created

- `ErrorHandlingMiddleware`
- `ErrorResponse`
- Middleware registration in `Program.cs`

### Why It Was Created

Without global error handling, every controller would need repeated `try/catch` blocks.

This middleware catches exceptions in one place and converts them into consistent JSON HTTP responses.

### Main Logic

The middleware maps exceptions to status codes:

- `NotFoundException` becomes `404 Not Found`
- `ValidationException` becomes `400 Bad Request`
- `ArgumentException` becomes `400 Bad Request`
- unexpected exceptions become `500 Internal Server Error`

The middleware also logs unexpected exceptions and avoids leaking internal exception details in production.

### Files Changed

- `PatientCareApi.Api/Middleware/ErrorHandlingMiddleware.cs`
- `PatientCareApi.Api/Middleware/ErrorResponse.cs`
- `PatientCareApi.Api/Program.cs`
- `README.md`
- `PROJECT_EXPLANATION.md`
- `ARCHITECTURE.md`
- `API_ENDPOINTS.md`
- `LEARNING_NOTES.md`

### How To Test It

Run:

```powershell
dotnet restore --configfile NuGet.Config
dotnet build --no-restore
dotnet run --project PatientCareApi.Api
```

Then call an endpoint with an id that does not exist, for example:

```text
GET /api/patients/999
```

Expected response:

```json
{
  "statusCode": 404,
  "message": "Patient with id 999 was not found."
}
```

### How To Explain It In An Interview

> I added global error handling middleware so controllers do not need repeated try/catch blocks. Services throw exceptions like `NotFoundException`, and the middleware translates them into correct HTTP responses such as 404, 400, or 500.

### GitHub Commands For This Feature

```powershell
git stash push -u -m "feature 06 error handling"          # Save current uncommitted changes, including new files
git checkout development                                  # Switch to development branch
git pull origin development                               # Get latest development code
git checkout -B feature/06-error-handling                 # Create/reset Feature 06 branch
git stash pop                                             # Bring Feature 06 changes back
dotnet restore --configfile NuGet.Config                  # Restore packages
dotnet build --no-restore                                 # Confirm build
git add .                                                 # Stage all files
git commit -m "Add global error handling middleware"       # Commit feature
git push -u origin feature/06-error-handling              # Push branch
```

Create the pull request:

```text
base: development
compare: feature/06-error-handling
```

## Feature 07: EF Core Migrations And Database Setup

### What Was Created

- Initial EF Core migration:
  - `InitialCreate`
- Migration model snapshot:
  - `PatientCareDbContextModelSnapshot`
- SQLite database was created locally by applying the migration.

### Why It Was Created

The DbContext describes the database model in C# code. Migrations turn that model into database schema changes.

This feature creates the first database schema for:

- Patients
- Treatments
- Appointments

### Main Logic

`InitialCreate` creates the main tables, primary keys, foreign keys, and indexes.

EF Core also creates `__EFMigrationsHistory` in the database. That table tracks which migrations have already been applied.

The migration files should be committed to Git. The generated `patientcare.db` file should not be committed because each developer can create it locally with `database update`.

### Files Changed

- `PatientCareApi.Infrastructure/Data/Migrations/20260625070821_InitialCreate.cs`
- `PatientCareApi.Infrastructure/Data/Migrations/20260625070821_InitialCreate.Designer.cs`
- `PatientCareApi.Infrastructure/Data/Migrations/PatientCareDbContextModelSnapshot.cs`
- `README.md`
- `PROJECT_EXPLANATION.md`
- `ARCHITECTURE.md`
- `API_ENDPOINTS.md`
- `LEARNING_NOTES.md`

### How To Test It

Run:

```powershell
dotnet restore --configfile NuGet.Config
dotnet build --no-restore
dotnet ef database update --project PatientCareApi.Infrastructure --startup-project PatientCareApi.Api
```

### How To Explain It In An Interview

> I added the initial EF Core migration to create the SQLite database schema. The migration creates tables for patients, treatments, and appointments, including relationships and indexes. I commit migration files to Git, but not the generated database file, because each environment can create its own database by running `dotnet ef database update`.

### GitHub Commands For This Feature

```powershell
git stash push -u -m "feature 07 efcore migrations"       # Save current uncommitted changes, including new files
git checkout development                                  # Switch to development branch
git pull origin development                               # Get latest development code
git checkout -B feature/07-efcore-migrations              # Create/reset Feature 07 branch
git stash pop                                             # Bring Feature 07 changes back
dotnet restore --configfile NuGet.Config                  # Restore packages
dotnet build --no-restore                                 # Confirm build
dotnet ef database update --project PatientCareApi.Infrastructure --startup-project PatientCareApi.Api  # Create/update local SQLite DB
git add .                                                 # Stage all files except ignored DB files
git commit -m "Add initial EF Core migration"             # Commit feature
git push -u origin feature/07-efcore-migrations           # Push branch
```

Create the pull request:

```text
base: development
compare: feature/07-efcore-migrations
```

## Feature 08: Domain Unit Tests

### What Was Created

- `PatientTests`
- `AppointmentTests`

### Why It Was Created

Unit tests verify important behavior without manually testing through Swagger or the database.

The first tests focus on Domain rules because Domain is independent from EF Core and ASP.NET Core.

### Tests Added

- Creating a valid patient should succeed.
- Creating a patient with an empty first name should fail.
- Completing an appointment should change status to `Completed`.

### Files Changed

- `PatientCareApi.Tests/Domain/PatientTests.cs`
- `PatientCareApi.Tests/Domain/AppointmentTests.cs`
- `README.md`
- `PROJECT_EXPLANATION.md`
- `ARCHITECTURE.md`
- `LEARNING_NOTES.md`

### How To Test It

Run:

```powershell
dotnet restore --configfile NuGet.Config
dotnet test --no-restore
```

### How To Explain It In An Interview

> I added unit tests for core domain behavior. These tests do not need a database or API server. They verify that a valid patient can be created, invalid patient names are rejected, and appointment completion changes the domain status correctly.

### GitHub Commands For This Feature

```powershell
git stash push -u -m "feature 08 unit tests"       # Save current uncommitted changes, including new files
git checkout development                           # Switch to development branch
git pull origin development                        # Get latest development code
git checkout -B feature/08-unit-tests              # Create/reset Feature 08 branch
git stash pop                                      # Bring Feature 08 changes back
dotnet restore --configfile NuGet.Config           # Restore packages
dotnet test --no-restore                           # Run tests
git add .                                          # Stage all files
git commit -m "Add domain unit tests"              # Commit feature
git push -u origin feature/08-unit-tests           # Push branch
```

Create the pull request:

```text
base: development
compare: feature/08-unit-tests
```

## Feature 09: GitHub Actions CI Pipeline

### What Was Created

- GitHub Actions workflow:
  - `.github/workflows/dotnet-ci.yml`

### Why It Was Created

CI means Continuous Integration.

It automatically checks the project when code is pushed or when a pull request is opened.

This helps catch build or test failures before code is merged into `development` or `main`.

### Main Logic

The workflow runs on:

- pushes to `main`
- pushes to `development`
- pushes to `feature/**`
- pull requests targeting `main`
- pull requests targeting `development`

The job runs on Ubuntu and performs:

1. Checkout repository.
2. Install .NET 9.
3. Restore NuGet packages.
4. Build the solution in Release mode.
5. Run tests.
6. Upload test results.

### Files Changed

- `.github/workflows/dotnet-ci.yml`
- `README.md`
- `PROJECT_EXPLANATION.md`
- `ARCHITECTURE.md`
- `LEARNING_NOTES.md`

### How To Test It

Run locally:

```powershell
dotnet restore --configfile NuGet.Config
dotnet build --configuration Release --no-restore
dotnet test --configuration Release --no-build
```

After pushing the branch, check the **Actions** tab in GitHub.

### How To Explain It In An Interview

> I added a GitHub Actions CI workflow. It runs automatically when I push code or open a pull request. The workflow restores packages, builds the solution, runs tests, and uploads test results. This shows that every feature is checked before merging.

### GitHub Commands For This Feature

```powershell
git stash push -u -m "feature 09 github actions ci"       # Save current uncommitted changes, including new files
git checkout development                                  # Switch to development branch
git pull origin development                               # Get latest development code
git checkout -B feature/09-github-actions-ci              # Create/reset Feature 09 branch
git stash pop                                             # Bring Feature 09 changes back
dotnet restore --configfile NuGet.Config                  # Restore packages
dotnet test --no-restore                                  # Run local tests before pushing
git add .                                                 # Stage all files
git commit -m "Add GitHub Actions CI pipeline"            # Commit feature
git push -u origin feature/09-github-actions-ci           # Push branch
```

Create the pull request:

```text
base: development
compare: feature/09-github-actions-ci
```

## Feature 10: Dockerfile And Docker Image Workflow

### What Was Created

- `Dockerfile`
- `.dockerignore`
- GitHub Actions workflow:
  - `.github/workflows/docker-image.yml`

### Why It Was Created

Docker makes the API easier to package and run consistently.

Instead of relying on a developer machine having the exact setup, Docker builds an image that contains the published API and the runtime needed to run it.

The GitHub Actions Docker workflow automates image building and publishing.

### Main Logic

The Dockerfile uses a multi-stage build:

1. Build stage:
   - uses `mcr.microsoft.com/dotnet/sdk:9.0`
   - restores packages
   - publishes the API
2. Runtime stage:
   - uses `mcr.microsoft.com/dotnet/aspnet:9.0`
   - copies the published app
   - runs `PatientCareApi.Api.dll`

The app listens on port `8080` inside the container.

The Docker workflow:

- builds on pull requests
- pushes the image only when not running on a pull request
- publishes to GitHub Container Registry

### Files Changed

- `Dockerfile`
- `.dockerignore`
- `.github/workflows/docker-image.yml`
- `README.md`
- `PROJECT_EXPLANATION.md`
- `ARCHITECTURE.md`
- `LEARNING_NOTES.md`

### How To Test It

If Docker is installed:

```powershell
docker build -t patient-care-api .
docker run --rm -p 8080:8080 patient-care-api
```

Open:

```text
http://localhost:8080/swagger
```

Local verification without Docker:

```powershell
dotnet restore --configfile NuGet.Config
dotnet test --no-restore
```

### How To Explain It In An Interview

> I added Docker support using a multi-stage Dockerfile. The first stage uses the .NET SDK to restore and publish the API. The final stage uses the smaller ASP.NET Core runtime image to run only the published output. I also added a GitHub Actions workflow that builds the Docker image and can publish it to GitHub Container Registry.

### GitHub Commands For This Feature

```powershell
git stash push -u -m "feature 10 docker image workflow"   # Save current uncommitted changes, including new files
git checkout development                                  # Switch to development branch
git pull origin development                               # Get latest development code
git checkout -B feature/10-docker-image-workflow          # Create/reset Feature 10 branch
git stash pop                                             # Bring Feature 10 changes back
dotnet restore --configfile NuGet.Config                  # Restore packages
dotnet test --no-restore                                  # Run local tests before pushing
git add .                                                 # Stage all files
git commit -m "Add Docker image workflow"                 # Commit feature
git push -u origin feature/10-docker-image-workflow       # Push branch
```

Create the pull request:

```text
base: development
compare: feature/10-docker-image-workflow
```

## Feature 11: API Integration Tests

### What Was Created

- API test factory:
  - `PatientCareApi.Tests/Api/PatientCareApiFactory.cs`
- API endpoint tests:
  - `PatientCareApi.Tests/Api/PatientEndpointTests.cs`
  - `PatientCareApi.Tests/Api/AppointmentEndpointTests.cs`
- Test project references and package:
  - `Microsoft.AspNetCore.Mvc.Testing`
  - references to `PatientCareApi.Api` and `PatientCareApi.Infrastructure`

### Why It Was Created

Unit tests prove small business rules. Integration tests prove the Web API works across layers.

These tests call real HTTP endpoints in memory, so they verify controller routing, dependency injection, services, repositories, EF Core, middleware, and JSON responses together.

### Main Logic

`PatientCareApiFactory` starts the API in memory by using `WebApplicationFactory`.

The factory removes the normal `PatientCareDbContext` registration and replaces it with an in-memory SQLite database.

This is important because SQLite in-memory keeps tests fast and isolated while still using EF Core relational database behavior.

### Files Changed

- `PatientCareApi.Api/Program.cs`
- `PatientCareApi.Tests/PatientCareApi.Tests.csproj`
- `PatientCareApi.Tests/Api/PatientCareApiFactory.cs`
- `PatientCareApi.Tests/Api/PatientEndpointTests.cs`
- `PatientCareApi.Tests/Api/AppointmentEndpointTests.cs`
- `README.md`
- `PROJECT_EXPLANATION.md`
- `ARCHITECTURE.md`
- `API_ENDPOINTS.md`
- `LEARNING_NOTES.md`

### How To Test It

```powershell
dotnet restore --configfile NuGet.Config
dotnet test --no-restore
```

Expected result:

```text
Passed: 6
Failed: 0
```

### How To Explain It In An Interview

> I added integration tests with `WebApplicationFactory`. These tests start the API in memory and call real HTTP endpoints. The test setup replaces the normal SQLite file database with in-memory SQLite, so the tests are isolated but still exercise EF Core, repositories, services, controllers, middleware, and JSON serialization.

### GitHub Commands For This Feature

```powershell
git stash push -u -m "feature 11 api integration tests"   # Save current uncommitted changes, including new files
git checkout development                                  # Switch to development branch
git pull origin development                               # Get latest development code
git checkout -B feature/11-api-integration-tests          # Create/reset Feature 11 branch
git stash pop                                             # Bring Feature 11 changes back
dotnet restore --configfile NuGet.Config                  # Restore packages
dotnet test --no-restore                                  # Run all unit and integration tests
git add .                                                 # Stage all files
git commit -m "Add API integration tests"                 # Commit feature
git push -u origin feature/11-api-integration-tests       # Push branch
```

Create the pull request:

```text
base: development
compare: feature/11-api-integration-tests
```
