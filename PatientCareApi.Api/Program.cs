using Microsoft.EntityFrameworkCore;
using PatientCareApi.Api.Middleware;
using PatientCareApi.Application.Repositories;
using PatientCareApi.Application.Services;
using PatientCareApi.Infrastructure.Data;
using PatientCareApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Register controller support so HTTP endpoints can be implemented in controller classes.
builder.Services.AddControllers();

// Swagger/OpenAPI makes the API easy to inspect and test during development.
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("DefaultConnection connection string is missing.");
}

// Program.cs is the composition root: it connects interfaces to concrete implementations.
builder.Services.AddDbContext<PatientCareDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<ITreatmentRepository, TreatmentRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<ITreatmentService, TreatmentService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

var app = builder.Build();

// Global error handling keeps controllers clean by avoiding repeated try/catch blocks.
app.UseMiddleware<ErrorHandlingMiddleware>();

// Swagger UI is enabled only in development so it is available while learning and testing locally.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Keep incoming requests on HTTPS when the app is running with HTTPS enabled.
app.UseHttpsRedirection();

// Enables authorization checks for endpoints that require policies or authenticated users.
app.UseAuthorization();

// Connect attribute-routed controllers, such as /api/patients, to the request pipeline.
app.MapControllers();

app.Run();

// WebApplicationFactory uses this partial class to start the API in memory for integration tests.
public partial class Program
{
}
