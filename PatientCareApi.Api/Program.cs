var builder = WebApplication.CreateBuilder(args);

// Register MVC controller support so API endpoints can be defined in controller classes.
builder.Services.AddControllers();

// Register OpenAPI metadata generation. Swagger UI will be added in a later API feature.
builder.Services.AddOpenApi();

var app = builder.Build();

// Only expose development API metadata while working locally.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Keep incoming requests on HTTPS when the app is running with HTTPS enabled.
app.UseHttpsRedirection();

// Enables authorization checks for endpoints that require policies or authenticated users.
app.UseAuthorization();

// Connect attribute-routed controllers, such as /api/patients, to the request pipeline.
app.MapControllers();

app.Run();
