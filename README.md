# PatientCareApi

PatientCareApi is an ASP.NET Core Web API practice project for managing patients, treatments, and appointments in a healthcare-style system.

The project is being built feature by feature so it can be demonstrated as both a coding challenge solution and a GitHub workflow practice project.

## Technologies

- .NET 9
- ASP.NET Core Web API
- Clean layered architecture
- EF Core with SQLite
- Swagger/OpenAPI
- xUnit
- GitHub Actions CI/CD
- Docker

## Current Status

Completed so far:

- Feature 01: solution structure and Domain model
- Feature 02: DTOs, repository interfaces, service interfaces, and Unit of Work contract
- Feature 03: Application services, validation helper, DTO mapping, and custom not-found exception
- Feature 04: EF Core DbContext, repository implementations, and Unit of Work implementation
- Feature 05: API dependency injection, Swagger UI, connection string, and REST controllers
- Feature 06: Global error handling middleware
- Feature 07: Initial EF Core migration and SQLite database creation
- Feature 08: Domain unit tests
- Feature 09: GitHub Actions CI pipeline
- Feature 10: Dockerfile and Docker image workflow
- Feature 11: API integration tests with in-memory SQLite

## How To Run

The API can run after packages are restored and the database is created through EF Core migrations.

```powershell
dotnet restore --configfile NuGet.Config
dotnet ef database update --project PatientCareApi.Infrastructure --startup-project PatientCareApi.Api
dotnet run --project PatientCareApi.Api
```

## How To Run Migrations

Create a new migration after changing the EF Core model:

```powershell
dotnet ef migrations add MigrationName --project PatientCareApi.Infrastructure --startup-project PatientCareApi.Api --output-dir Data\Migrations
```

Apply migrations to SQLite:

```powershell
dotnet ef database update --project PatientCareApi.Infrastructure --startup-project PatientCareApi.Api
```

## How To Run Tests

Run the test suite:

```powershell
dotnet test
```

The tests include fast Domain unit tests and API integration tests that start the Web API in memory.

## CI Pipeline

GitHub Actions runs automatically on pushes and pull requests.

The workflow:

- restores NuGet packages
- builds the solution in Release mode
- runs the test suite
- uploads test result files as a workflow artifact

## Docker

Build the Docker image locally if Docker is installed:

```powershell
docker build -t patient-care-api .
```

Run the container:

```powershell
docker run --rm -p 8080:8080 patient-care-api
```

Open:

```text
http://localhost:8080/swagger
```

The Docker image workflow can publish the image to GitHub Container Registry:

```text
ghcr.io/umarsalem/patient-care-api
```

## Swagger

Swagger/OpenAPI will be available when the API feature is completed:

```text
https://localhost:<port>/swagger
```

## Main Endpoint Plan

- `GET /api/patients`
- `GET /api/patients/{id}`
- `POST /api/patients`
- `PUT /api/patients/{id}`
- `DELETE /api/patients/{id}`
- `GET /api/treatments`
- `GET /api/patients/{id}/treatments`
- `POST /api/patients/{id}/treatments`
- `GET /api/appointments`
- `GET /api/patients/{id}/appointments`
- `POST /api/appointments`
- `PATCH /api/appointments/{id}/complete`
- `PATCH /api/appointments/{id}/cancel`

## GitHub Workflow

Each feature should be committed on its own branch and pushed to GitHub.

For Feature 11:

```powershell
git stash push -u -m "feature 11 api integration tests"
git checkout development
git pull origin development
git checkout -B feature/11-api-integration-tests
git stash pop
dotnet restore --configfile NuGet.Config
dotnet test --no-restore
git add .
git commit -m "Add API integration tests"
git push -u origin feature/11-api-integration-tests
```

See `GIT_WORKFLOW.md` for merge vs rebase notes.
