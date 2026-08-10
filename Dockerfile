# Build stage: uses the full SDK because restore, build, and publish need developer tooling.
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY NuGet.Config ./
COPY PatientCareApi.sln ./
COPY PatientCareApi.Domain/PatientCareApi.Domain.csproj PatientCareApi.Domain/
COPY PatientCareApi.Application/PatientCareApi.Application.csproj PatientCareApi.Application/
COPY PatientCareApi.Infrastructure/PatientCareApi.Infrastructure.csproj PatientCareApi.Infrastructure/
COPY PatientCareApi.Api/PatientCareApi.Api.csproj PatientCareApi.Api/
COPY PatientCareApi.Tests/PatientCareApi.Tests.csproj PatientCareApi.Tests/

RUN dotnet restore PatientCareApi.sln --configfile NuGet.Config

COPY . .

RUN dotnet publish PatientCareApi.Api/PatientCareApi.Api.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

# Runtime stage: smaller image with only the ASP.NET Core runtime and published app files.
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "PatientCareApi.Api.dll"]
